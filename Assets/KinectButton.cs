using UnityEngine;
using System.Collections;

public class KinectButton : MonoBehaviour {

    public enum ButtonType { None, Activity, Influence, PassiveIdentity, Mood, Interests, Public, Private, Pro };

    public float radius = 1f;
    public ButtonType buttonType;

    Vector2 pos;
    Animator animator;
    GUIManager guiManager;

    // Use this for initialization
    void Start () {
        pos = new Vector2(transform.position.x, transform.position.y);
        animator = GetComponent<Animator>();
        guiManager = (GUIManager)FindObjectOfType(typeof(GUIManager));
    }
	
	// Update is called once per frame
	void Update () {

    }

    public float HitTest(Vector2 cursorPos)
    {
        float distance = Vector2.Distance(pos, cursorPos);

        if (distance <= radius)
        {
            return distance;
        }
        else
        {
            return -1f;
        }
    }

    public void OnCursorEnter()
    {
        animator.SetBool("active", true);
    }

    public void OnCursorLeave()
    {
        animator.SetBool("active", false);
    }

    public void Validate()
    {
        guiManager.OnCursorValidate(buttonType);
    }

    public void Unvalidate()
    {
        guiManager.OnCursorUnvalidate(buttonType);
    }
}
