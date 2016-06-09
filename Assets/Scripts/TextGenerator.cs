using UnityEngine;
using System.Collections;

public class TextGenerator : MonoBehaviour {

    public GameObject textGameObject;
    private Transform tr;


	// Use this for initialization
	void Start () {
        tr = GetComponent<Transform>();

        // CreateText("test");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    GameObject CreateText(string text)
    {
        GameObject instance = Instantiate<GameObject>(textGameObject);
        instance.transform.parent = tr;
        instance.transform.localPosition = new Vector3(0, 0, 0);
        instance.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
        SimpleHelvetica sh = instance.GetComponent<SimpleHelvetica>();
        sh.Text = text;
        sh.GenerateText();
        return instance;
    }
}
