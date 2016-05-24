using UnityEngine;
using System.Collections;

public enum IdentityComposante { None, Activity, Influence, PassiveIdentity, Mood, Interests };
public enum IdentityCircle { None, Public, Private, Pro };

public class GUIManager : MonoBehaviour {

    public GameObject activityHUD;
    public GameObject influenceHUD;
    public GameObject passiveIdentityHUD;
    public GameObject moodHUD;
    public GameObject interestsHUD;

    IdentityComposante currentIdentityComposante;
    IdentityCircle currentIdentityCircle;

    // Use this for initialization
    void Start () {
        SetActiveHUD(IdentityComposante.None);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnCursorValidate(KinectButton.ButtonType buttonType)
    {
        if (buttonType == KinectButton.ButtonType.Activity)
        {
            SetActiveHUD(IdentityComposante.Activity);
        }
        else if (buttonType == KinectButton.ButtonType.Influence)
        {
            SetActiveHUD(IdentityComposante.Influence);
        }
        else if (buttonType == KinectButton.ButtonType.PassiveIdentity)
        {
            SetActiveHUD(IdentityComposante.PassiveIdentity);
        }
        else if (buttonType == KinectButton.ButtonType.Mood)
        {
            SetActiveHUD(IdentityComposante.Mood);
        }
        else if (buttonType == KinectButton.ButtonType.Interests)
        {
            SetActiveHUD(IdentityComposante.Interests);
        }
    }

    public void OnCursorUnvalidate(KinectButton.ButtonType buttonType)
    {
        if (buttonType == KinectButton.ButtonType.Activity && currentIdentityComposante == IdentityComposante.Activity ||
            buttonType == KinectButton.ButtonType.Influence && currentIdentityComposante == IdentityComposante.Influence ||
            buttonType == KinectButton.ButtonType.PassiveIdentity && currentIdentityComposante == IdentityComposante.PassiveIdentity ||
            buttonType == KinectButton.ButtonType.Mood && currentIdentityComposante == IdentityComposante.Mood ||
            buttonType == KinectButton.ButtonType.Interests && currentIdentityComposante == IdentityComposante.Interests)
        {
            SetActiveHUD(IdentityComposante.None);
        }
    }

    void SetActiveHUD(IdentityComposante activeHUD)
    {
        if (activeHUD == IdentityComposante.Activity)
        {
            currentIdentityComposante = IdentityComposante.Activity;

            activityHUD.SetActive(true);
            influenceHUD.SetActive(false);
            passiveIdentityHUD.SetActive(false);
            moodHUD.SetActive(false);
            interestsHUD.SetActive(false);
        }
        else if (activeHUD == IdentityComposante.Influence)
        {
            currentIdentityComposante = IdentityComposante.Influence;

            activityHUD.SetActive(false);
            influenceHUD.SetActive(true);
            passiveIdentityHUD.SetActive(false);
            moodHUD.SetActive(false);
            interestsHUD.SetActive(false);
        }
        else if (activeHUD == IdentityComposante.PassiveIdentity)
        {
            currentIdentityComposante = IdentityComposante.PassiveIdentity;

            activityHUD.SetActive(false);
            influenceHUD.SetActive(false);
            passiveIdentityHUD.SetActive(true);
            moodHUD.SetActive(false);
            interestsHUD.SetActive(false);
        }
        else if (activeHUD == IdentityComposante.Mood)
        {
            currentIdentityComposante = IdentityComposante.Mood;

            activityHUD.SetActive(false);
            influenceHUD.SetActive(false);
            passiveIdentityHUD.SetActive(false);
            moodHUD.SetActive(true);
            interestsHUD.SetActive(false);
        }
        else if (activeHUD == IdentityComposante.Interests)
        {
            currentIdentityComposante = IdentityComposante.Interests;

            activityHUD.SetActive(false);
            influenceHUD.SetActive(false);
            passiveIdentityHUD.SetActive(false);
            moodHUD.SetActive(false);
            interestsHUD.SetActive(true);
        }
        else
        {
            currentIdentityComposante = IdentityComposante.None;

            activityHUD.SetActive(false);
            influenceHUD.SetActive(false);
            passiveIdentityHUD.SetActive(false);
            moodHUD.SetActive(false);
            interestsHUD.SetActive(false);
        }
    }
}
