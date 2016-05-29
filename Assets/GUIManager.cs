using UnityEngine;
using System.Collections;

public enum IdentityComposante { None, Activity, Influence, PassiveIdentity, Mood, Interests };
public enum IdentityCircle { None, Public, Private, Pro };

public class GUIManager : MonoBehaviour {

    public HUDGroup activityHUD;
    public HUDGroup influenceHUD;
    public HUDGroup passiveIdentityHUD;
    public HUDGroup moodHUD;
    public HUDGroup interestsHUD;

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

            activityHUD.Show();
            influenceHUD.Hide();
            passiveIdentityHUD.Hide();
            moodHUD.Hide();
            interestsHUD.Hide();
        }
        else if (activeHUD == IdentityComposante.Influence)
        {
            currentIdentityComposante = IdentityComposante.Influence;

            activityHUD.Hide();
            influenceHUD.Show();
            passiveIdentityHUD.Hide();
            moodHUD.Hide();
            interestsHUD.Hide();
        }
        else if (activeHUD == IdentityComposante.PassiveIdentity)
        {
            currentIdentityComposante = IdentityComposante.PassiveIdentity;

            activityHUD.Hide();
            influenceHUD.Hide();
            passiveIdentityHUD.Show();
            moodHUD.Hide();
            interestsHUD.Hide();
        }
        else if (activeHUD == IdentityComposante.Mood)
        {
            currentIdentityComposante = IdentityComposante.Mood;

            activityHUD.Hide();
            influenceHUD.Hide();
            passiveIdentityHUD.Hide();
            moodHUD.Show();
            interestsHUD.Hide();
        }
        else if (activeHUD == IdentityComposante.Interests)
        {
            currentIdentityComposante = IdentityComposante.Interests;

            activityHUD.Hide();
            influenceHUD.Hide();
            passiveIdentityHUD.Hide();
            moodHUD.Hide();
            interestsHUD.Show();
        }
        else
        {
            currentIdentityComposante = IdentityComposante.None;

            activityHUD.Hide();
            influenceHUD.Hide();
            passiveIdentityHUD.Hide();
            moodHUD.Hide();
            interestsHUD.Hide();
        }
    }
}
