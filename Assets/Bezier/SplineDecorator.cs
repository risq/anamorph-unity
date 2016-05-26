using UnityEngine;
using System;

public class SplineDecorator : MonoBehaviour {

	public BezierSpline spline;
	public bool lookForward;
	public Transform item;

    public Transform[] instances;
    private float stepSize;
    private int count;

	private void Awake () {
        // CreateSpline(20);
    }

    public Transform[] CreateSpline(int objectsCount)
    {
        DeleteInstances();

        count = objectsCount;

        if (count <= 0)
        {
            return null;
        }
        if (spline.Loop || count == 1)
        {
            stepSize = 1f / count;
        }
        else
        {
            stepSize = 1f / (count - 1);
        }

        Array.Resize(ref instances, count);

        for (int i = 0; i < count; i++)
        {
            instances[i] = Instantiate(item) as Transform;
            Vector3 position = spline.GetPoint(i * stepSize);
            instances[i].transform.localPosition = position;
            if (lookForward)
            {
                instances[i].transform.LookAt(position + spline.GetDirection(i * stepSize));
            }
            instances[i].transform.parent = transform;
        }

        return instances;
    }

    private void DeleteInstances()
    {
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                Destroy(instances[i].gameObject);
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 position = spline.GetPoint(i * stepSize);
            instances[i].transform.position = position;
            if (lookForward)
            {
                instances[i].transform.LookAt(position + spline.GetDirection(i * stepSize));
            }
        }
    }
}