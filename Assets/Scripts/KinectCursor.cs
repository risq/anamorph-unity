using UnityEngine;
using System.Collections;

public class KinectCursor : MonoBehaviour {

    public KinectButton[] kinectButtons;
    KinectButton activeKinectButton;
    Transform tr;

    float closestButtonDistance;
    int kinectButtonsLength;
    KinectButton closestButton;

    public bool cursorEnabled = false;

    // Use this for initialization
    void Start () {
        tr = GetComponent<Transform>();
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
                    activeKinectButton.OnCursorLeave();

                closestButton.OnCursorEnter();
                activeKinectButton = closestButton;
            }
        }
        else if (activeKinectButton)
        {
            activeKinectButton.OnCursorLeave();
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
            kinectButtons[i].OnCursorLeave();
        }
    }

}
