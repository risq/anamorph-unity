using UnityEngine;
using System.Collections;

public class FloatingHUD : MonoBehaviour {
    public float amplitude = 10;
    public float speed = 0.5f;
    Vector3 initialPos;
    Transform tr;

	// Use this for initialization
	void Start () {
        tr = GetComponent<Transform>();
        initialPos = tr.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
        tr.localPosition = new Vector3(initialPos.x + Mathf.Cos(Time.time * speed) * amplitude, initialPos.y + Mathf.Sin(Time.time * speed) * amplitude, initialPos.z);
	}
}
