using UnityEngine;
using System.Collections;

public class KinectCursor : MonoBehaviour {

    public KinectButton[] kinectButtons;
    KinectButton activeKinectButton;
    Transform tr;

    float closestButtonDistance;
    int kinectButtonsLength;
    KinectButton closestButton;

    // Use this for initialization
    void Start () {
        tr = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        kinectButtonsLength = kinectButtons.Length;
        StartCoroutine(StartTestingButtons());
    }

    void TestActiveKinectButton()
    {
        closestButtonDistance = float.MaxValue;
        closestButton = null;

        for (int i = 0; i < kinectButtonsLength; i++)
        {
            float hitTestDistance = kinectButtons[i].HitTest(tr.position);

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
            TestActiveKinectButton();
            yield return new WaitForSeconds(0.3f);
        }
    }

}
