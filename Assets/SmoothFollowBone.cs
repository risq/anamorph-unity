using UnityEngine;
using System.Collections;

public class SmoothFollowBone : MonoBehaviour {

    public Transform target;
    public float speed = .1f;
    
    private Transform tr;
    private Vector3 velocity = Vector3.zero;
    private Quaternion initialRotation;

    // Use this for initialization
    void Start () {
        tr = GetComponent<Transform>();
        initialRotation = tr.rotation;
	}
	
	// Update is called once per frame
	void Update () {
        tr.position = Vector3.Lerp(tr.position, target.position, Time.time * speed);
        tr.rotation = Quaternion.Slerp(tr.rotation, Quaternion.Lerp(initialRotation, target.rotation, 0.5f) * initialRotation, Time.time * speed);
	}
}
