using UnityEngine;
using System.Collections;

public class SineScaler : MonoBehaviour {

    public float minScale = 0f;
    public float maxScale = 1f;
    public float speed = 1;

    private float scale;
    private float range;
    private float offset;
    Transform tr;

	// Use this for initialization
	void Start () {
        tr = GetComponent<Transform>();
        range = (maxScale - minScale) / 2;
        offset = range + minScale;
	}
	
	// Update is called once per frame
	void Update () {
	    scale = offset + Mathf.Sin(Time.time * speed) * range;
        tr.localScale = new Vector3(scale, scale, scale);
    }
}
