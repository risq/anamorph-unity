using UnityEngine;
using System;
using System.Collections;

public enum VertexAnimationDirection
{
	Forward,
	Backward,
	ForwardBackward,
	BackwardForward,
}

/// <summary>
/// Interpolates between 2 meshes in the shader.
/// </summary>
[RequireComponent(typeof(MeshFilter))]
public class VertexAnimationBehavior : MonoBehaviour 
{
	#region variables for looping animation
	/// <summary>
	/// The direction of the looped animation
	/// </summary>
	public VertexAnimationDirection m_directon = VertexAnimationDirection.Forward;
	
	/// <summary>
	/// The duration of one loop
	/// </summary>
	public float m_loopDuration;
	
	/// <summary>
	/// How often the animation should loop. 0 means looping infinitely
	/// </summary>
	public int m_loopNTimes;
	
	private System.Func<float, float> m_easingFunction;
	private System.Func<float, float> m_easingFunction2;
	private float m_startTime;
	#endregion
	
	/// <summary>
	/// The second keyframe to interpolate between
	/// </summary>
	public Mesh m_otherKeyframe;

	// Use this for initialization
	void Awake ()
	{
		if (null != m_otherKeyframe)
			SetKeyframe2(m_otherKeyframe.vertices, m_otherKeyframe.normals);
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
	
	void Start ()
	{
		// Start a looping animation if one has been configured in the inspector
		if (m_loopDuration > .0001f)
		{
			if (m_loopNTimes > 0)
				LoopNTimes(m_directon, m_loopDuration, m_loopNTimes);
			else 
				LoopForever(m_directon, m_loopDuration);
		}
	}
	
	private static Color PackNormal(Vector3 normal)
	{
		normal.Normalize();
		normal = normal * .5f + new Vector3(.5f, .5f, .5f);
		Color packed = new Color(normal.x, normal.y, normal.z, 0.0f);
		return packed;
	}
	
	private static Color[] PackNormals(Vector3[] normals)
	{
		Color[] packed = new Color[normals.Length];
		for (int i=0; i < normals.Length; ++i)
		{
			packed[i] = PackNormal(normals[i]);
		}
		return packed;
	}
	
	/// <summary>
	/// Sets new vertex data for keyframe 1.
	/// Don't use this too often since new vetex data has to be uploaded to the GPU.
	/// </summary>
	public void SetKeyframe1(Mesh mesh)
	{
		SetKeyframe1(mesh.vertices, mesh.normals);
	}
	
	/// <summary>
	/// Sets new vertex data for keyframe 1.
	/// Don't use this too often since new vetex data has to be uploaded to the GPU.
	/// </summary>
	public void SetKeyframe1(Vector3[] vertexPositions, Vector3[] vertexNormals)
	{
		var myMesh = GetComponent<MeshFilter>().mesh;
		if (myMesh.vertices.Length != vertexPositions.Length)
		{
			Debug.LogError("#vertices of the two keyframes is not equal.");
			return;
		}
		myMesh.vertices = vertexPositions;
		myMesh.normals = vertexNormals;
	}
	
	/// <summary>
	/// Sets new vertex data for keyframe 2.
	/// Don't use this too often since new vetex data has to be uploaded to the GPU.
	/// </summary>
	public void SetKeyframe2(Mesh mesh)
	{
		SetKeyframe2(mesh.vertices, mesh.normals);
	}
	
	/// <summary>
	/// Sets new vertex data for keyframe 2.
	/// Don't use this too often since new vetex data has to be uploaded to the GPU.
	/// </summary>
	public void SetKeyframe2(Vector3[] vertexPositions, Vector3[] vertexNormals)
	{
		var myMesh = GetComponent<MeshFilter>().mesh;
		if (myMesh.vertices.Length != vertexPositions.Length)
		{
			Debug.LogError("#vertices of the two keyframes is not equal.");
			return;
		}
		
		// Store the vertices of the second mesh on the GPU => in the tangent
		Vector4[] data = new Vector4[myMesh.vertices.Length];
		for (int i=0; i < data.Length; ++i)
		{
			data[i] = new Vector4(vertexPositions[i].x, vertexPositions[i].y, vertexPositions[i].z);
		}
		myMesh.tangents = data;
		
		// Store the normals of the second mesh on the GPU => in the vertex-colors
		myMesh.colors = PackNormals(vertexNormals);
	}
	
	/// <summary>
	/// Stops the looping animation, regardless of whether it was a forever-loop or a n-times-loop
	/// </summary>
	public void StopAnimation()
	{
		StopAllCoroutines();
	}
	
	/// <summary>
	/// Loops the animation forever
	/// </summary>
	/// <param name='direction'>
	/// Direction of the animation
	/// </param>
	/// <param name='loopDuration'>
	/// How many seconds 1 loop takes
	/// </param>
	/// <param name='easingFunction'>
	/// Easing function for the animation, or for the first part of the animation, respectively, if ForwardBackward or BackwardForward animation
	/// </param>
	/// <param name='easingFunctionBack'>
	/// Easing function for the second part of an ForwardBackward or BackwardForward animation
	/// </param>
	public void LoopForever(
		VertexAnimationDirection direction, 
		float loopDuration, 
		System.Func<float, float> easingFunction = null, 
		System.Func<float, float> easingFunctionBack = null)
	{
		StopAnimation();
		
		m_directon = direction;
		m_loopDuration = loopDuration;
		if (null != easingFunction)
			m_easingFunction = easingFunction;
		else 
			m_easingFunction = (x) => { return x; };
		if (null != easingFunctionBack)
			m_easingFunction2 = easingFunctionBack;
		else 
			m_easingFunction2 = m_easingFunction;
		
		StartCoroutine(DoLoop());
	}
	
	/// <summary>
	/// Loops the animation n times
	/// </summary>
	/// <param name='direction'>
	/// Direction of the animation
	/// </param>
	/// <param name='loopDuration'>
	/// How many seconds 1 loop takes
	/// </param>
	/// <param name='loopCount'>
	/// the n
	/// </para>
	/// <param name='easingFunction'>
	/// Easing function for the animation, or for the first part of the animation, respectively, if ForwardBackward or BackwardForward animation
	/// </param>
	/// <param name='easingFunctionBack'>
	/// Easing function for the second part of an ForwardBackward or BackwardForward animation
	/// </param>
	public void LoopNTimes(
		VertexAnimationDirection direction, 
		float loopDuration, 
		int loopCount,
		System.Func<float, float> easingFunction = null, 
		System.Func<float, float> easingFunctionBack = null)
	{
		StopAnimation();
		
		m_directon = direction;
		m_loopDuration = loopDuration;
		m_loopNTimes = loopCount;
		if (null != easingFunction)
			m_easingFunction = easingFunction;
		else 
			m_easingFunction = (x) => { return x; };
		if (null != easingFunctionBack)
			m_easingFunction2 = easingFunctionBack;
		else 
			m_easingFunction2 = m_easingFunction;
		
		StartCoroutine(DoLoop());
	}
	
	/// <summary>
	/// The coroutine which calculates the animation's ratio-property
	/// </summary>
	private IEnumerator DoLoop()
	{
		m_startTime = Time.time;
		while (true)
		{
			float running = Time.time - m_startTime;
			int loops = (int)(running / m_loopDuration);
			
			if (m_loopNTimes > 0)
			{
				// set animation's final position and break out of looping animation
				if (loops >= m_loopNTimes)
				{
					switch (m_directon)
					{
					case VertexAnimationDirection.Forward:
						GetComponent<Renderer>().material.SetFloat("_Ratio", 1f);
						break;
					case VertexAnimationDirection.Backward:
						GetComponent<Renderer>().material.SetFloat("_Ratio", 0f);
						break;
					case VertexAnimationDirection.ForwardBackward:
						GetComponent<Renderer>().material.SetFloat("_Ratio", 0f);
						break;
					case VertexAnimationDirection.BackwardForward:
						GetComponent<Renderer>().material.SetFloat("_Ratio", 1f);
						break;
					}
					
					yield break;
				}
			}
			
			float ratio = (running - loops * m_loopDuration) / m_loopDuration;
			switch (m_directon)
			{
			case VertexAnimationDirection.Forward:
				ratio = m_easingFunction(ratio);
				break;
			case VertexAnimationDirection.Backward:
				ratio = m_easingFunction(1.0f - ratio);
				break;
			case VertexAnimationDirection.ForwardBackward:
				ratio *= 2.0f;
				if (ratio < 1.0f)
					ratio = m_easingFunction(ratio);
				else
					ratio = m_easingFunction2(2.0f - ratio);
				break;
			case VertexAnimationDirection.BackwardForward:
				ratio = 1.0f - 2.0f * ratio;
				if (ratio > 0.0f)
					ratio = m_easingFunction(ratio);
				else
					ratio = m_easingFunction2(-ratio);
				break;
			}
			GetComponent<Renderer>().material.SetFloat("_Ratio", ratio);
			
			yield return null;
		}
	}
}
