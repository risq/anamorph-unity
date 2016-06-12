using UnityEngine;
using System.Collections;

public enum CursorType { None, LeftCursor, RightCursor };

public class KinectCursor : MonoBehaviour {

    public KinectButton[] kinectButtons;
    public CursorType cursorType;

    KinectButton activeKinectButton;
    Transform tr;

    float closestButtonDistance;
    int kinectButtonsLength;
    KinectButton closestButton;
    AudioManager audioManager;

    public bool cursorEnabled = false;

    // Use this for initialization
    void Start () {
        tr = GetComponent<Transform>();
        audioManager = FindObjectOfType<AudioManager>();
        kinectButtonsLength = kinectButtons.Length;
        StartCoroutine(StartTestingButtons());
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    void TestActiveKinectButton()
    {
        closestButtonDistance = float.MaxValue;
        closestButton = null;

        for (int i = 0; i < kinectButtonsLength; i++)
        {
            float hitTestDistance = kinectButtons[i].HitTest(tr.position);
            //Debug.Log("hitTestDistance " + hitTestDistance + " " + kinectButtons[i].buttonType);
            if (hitTestDistance >= 0 && hitTestDistance < closestButtonDistance)
            {
                closestButton = kinectButtons[i];
            }
        }

        if (closestButton)
        {
            if (closestButton != activeKinectButton)
            {
                if (activeKinectButton)
                    activeKinectButton.OnCursorLeave(cursorType);

                closestButton.OnCursorEnter(cursorType);
                audioManager.PlayUISound();
                activeKinectButton = closestButton;
            }
        }
        else if (activeKinectButton)
        {
            activeKinectButton.OnCursorLeave(cursorType);
            activeKinectButton = null;
        }
    }

    IEnumerator StartTestingButtons()
    {
        while (true)
        {
            if (cursorEnabled)
            {
                TestActiveKinectButton();
            }
            
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void DisableAll()
    {
        for (int i = 0; i < kinectButtonsLength; i++)
        {
            kinectButtons[i].OnCursorLeave(cursorType);
        }
    }

}
