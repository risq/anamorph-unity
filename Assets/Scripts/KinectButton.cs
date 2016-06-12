using UnityEngine;
using System.Collections;

public class KinectButton : MonoBehaviour {

    public enum ButtonType { None, Activity, Influence, PassiveIdentity, Mood, Interests, Public, Private, Pro, Photo };

    public float radius = 1f;
    public ButtonType buttonType;

    public Vector2 pos;
    public bool twoHandsButton = false;

    bool leftHandActive = false;
    bool rightHandActive = false;

    Animator animator;
    GUIManager guiManager;
    float distance;

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
        distance = Vector2.Distance(pos, cursorPos);
        return distance <= radius ? distance : -1f;
    }

    public void OnCursorEnter(CursorType cursorType)
    {
        if (twoHandsButton)
        {
            if (cursorType == CursorType.LeftCursor)
                leftHandActive = true;
            else if (cursorType == CursorType.RightCursor)
                rightHandActive = true;

            if (leftHandActive && rightHandActive)
                animator.SetBool("active", true);
            else
                animator.SetBool("semiActive", true);

            guiManager.OnCursorSemivalidate(buttonType);
        }
        else
        {
            animator.SetBool("active", true);
        }
       
    }

    public void OnCursorLeave(CursorType cursorType)
    {
        if (twoHandsButton)
        {
            if (cursorType == CursorType.LeftCursor)
                leftHandActive = false;
            else if (cursorType == CursorType.RightCursor)
                rightHandActive = false;

            animator.SetBool("active", false);

            if (!leftHandActive && !rightHandActive)
                animator.SetBool("semiActive", false);
        }
        else
        {
            animator.SetBool("active", false);
        }
    }

    public void OnLeftHandEnter()
    {
        leftHandActive = true;
    }

    public void OnRightHandEnter()
    {
        rightHandActive = true;
    }

    public void OnLeftHandLeave()
    {
        leftHandActive = false;
    }

    public void OnRightHandLeave()
    {
        rightHandActive = false;
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
