using UnityEngine;
using System.Collections;

public class KinectCursor : MonoBehaviour {

    public KinectButton[] kinectButtons;

    KinectButton activeKinectButton;
    Transform tr;

	// Use this for initialization
	void Start () {
        tr = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {

        StartCoroutine(StartTestingButtons());

    }

    void TestActiveKinectButton()
    {
        float closestButtonDistance = Mathf.Infinity;
        KinectButton closestButton = null;

        foreach (KinectButton kinectButton in kinectButtons)
        {
            float hitTestDistance = kinectButton.HitTest(tr.position);

            if (hitTestDistance >= 0 && hitTestDistance < closestButtonDistance)
            {
                closestButton = kinectButton;
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
            yield return new WaitForSeconds(0.1f);
        }
    }

}
