using UnityEngine;
using System.Collections;

public class SmoothFollowBone : MonoBehaviour {

    public Transform target;
    private Transform tr;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.3F;

    // Use this for initialization
    void Start () {
        tr = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        tr.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime);
        tr.rotation = target.rotation;
	}
}
