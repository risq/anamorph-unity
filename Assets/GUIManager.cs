using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public enum IdentityComposante { None, Activity, Influence, PassiveIdentity, Mood, Interests };
public enum IdentityCircle { None, Public, Private, Pro };

public class GUIManager : MonoBehaviour {
    public Image overlay;
    public HUDGroup activityHUD;
    public HUDGroup influenceHUD;
    public HUDGroup passiveIdentityHUD;
    public HUDGroup moodHUD;
    public HUDGroup interestsHUD;

    IdentityComposante currentIdentityComposante;
    IdentityCircle currentIdentityCircle;

    Tweener overlayFadeTweener;
    const float overlayFadeAmount = 0.5f;
    const float overlayFadeTime = 1f;

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
        overlayFadeTweener.Kill();

        if (activeHUD == IdentityComposante.Activity)
        {
            currentIdentityComposante = IdentityComposante.Activity;
            overlayFadeTweener = overlay.DOFade(overlayFadeAmount, overlayFadeTime);

            activityHUD.Show();
            influenceHUD.Hide();
            passiveIdentityHUD.Hide();
            moodHUD.Hide();
            interestsHUD.Hide();
        }
        else if (activeHUD == IdentityComposante.Influence)
        {
            currentIdentityComposante = IdentityComposante.Influence;
            overlayFadeTweener = overlay.DOFade(overlayFadeAmount, overlayFadeTime);


            activityHUD.Hide();
            influenceHUD.Show();
            passiveIdentityHUD.Hide();
            moodHUD.Hide();
            interestsHUD.Hide();
        }
        else if (activeHUD == IdentityComposante.PassiveIdentity)
        {
            currentIdentityComposante = IdentityComposante.PassiveIdentity;
            overlayFadeTweener = overlay.DOFade(overlayFadeAmount, overlayFadeTime);

            activityHUD.Hide();
            influenceHUD.Hide();
            passiveIdentityHUD.Show();
            moodHUD.Hide();
            interestsHUD.Hide();
        }
        else if (activeHUD == IdentityComposante.Mood)
        {
            currentIdentityComposante = IdentityComposante.Mood;
            overlayFadeTweener = overlay.DOFade(overlayFadeAmount, overlayFadeTime);

            activityHUD.Hide();
            influenceHUD.Hide();
            passiveIdentityHUD.Hide();
            moodHUD.Show();
            interestsHUD.Hide();
        }
        else if (activeHUD == IdentityComposante.Interests)
        {
            currentIdentityComposante = IdentityComposante.Interests;
            overlayFadeTweener = overlay.DOFade(overlayFadeAmount, overlayFadeTime);

            activityHUD.Hide();
            influenceHUD.Hide();
            passiveIdentityHUD.Hide();
            moodHUD.Hide();
            interestsHUD.Show();
        }
        else
        {
            currentIdentityComposante = IdentityComposante.None;
            overlayFadeTweener = overlay.DOFade(0, overlayFadeTime);

            activityHUD.Hide();
            influenceHUD.Hide();
            passiveIdentityHUD.Hide();
            moodHUD.Hide();
            interestsHUD.Hide();
        }
    }
}
