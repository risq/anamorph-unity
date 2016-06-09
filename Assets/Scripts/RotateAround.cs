using UnityEngine;
using System.Collections;

public class RotateAround : MonoBehaviour {

    public GameObject rotationCenterObject;
    public float speed = 50f;
    public enum Axis { X, Y, Z };
    public Axis axis = Axis.X;

    private Transform rotationCenter;

	// Use this for initialization
	void Start () {
        if (rotationCenterObject)
            rotationCenter = rotationCenterObject.transform;
        else
            rotationCenter = transform.parent.transform;
    }
	
	// Update is called once per frame
	void Update () {
        if (axis == Axis.X)
            transform.RotateAround(rotationCenter.position, transform.TransformDirection(Vector3.right), speed * Time.deltaTime);
        else if (axis == Axis.Y)
            transform.RotateAround(rotationCenter.position, transform.TransformDirection(Vector3.up), speed * Time.deltaTime);
        else if (axis == Axis.Z)
            transform.RotateAround(rotationCenter.position, transform.TransformDirection(Vector3.forward), speed * Time.deltaTime);
    }
}
