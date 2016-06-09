using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityStandardAssets.ImageEffects;
using System;

public enum IdentityComposante { None, Activity, Influence, PassiveIdentity, Mood, Interests };
public enum IdentityCircle { None, Public, Private, Pro };
public enum ExperienceState { Home, Sync, Loading, Experience, End };

public class GUIManager : MonoBehaviour, KinectGestures.GestureListenerInterface
{
    public CanvasGroup HomeScreen;
    public CanvasGroup SyncScreen;
    public CanvasGroup LoadingScreen;

    public Image overlay;

    public HUDGroup activityHUD;
    public HUDGroup influenceHUD;
    public HUDGroup passiveIdentityHUD;
    public HUDGroup moodHUD;
    public HUDGroup interestsHUD;

    public KinectCursor leftHandCursor;
    public KinectCursor rightHandCursor;

    IdentityComposante currentIdentityComposante;
    IdentityCircle currentIdentityCircle;
    ExperienceState currentState = ExperienceState.Home;

    Tweener overlayFadeTweener;
    Tweener grayScaleTween;

    Grayscale grayscaleEffect;
    RandomGlitchEnabler glitchEnabler;

    const float overlayFadeAmount = 0.5f;
    const float overlayFadeTime = 1f;

    // Use this for initialization
    void Start () {
        glitchEnabler = Camera.main.GetComponent<RandomGlitchEnabler>();
        grayscaleEffect = Camera.main.GetComponent<Grayscale>();
        grayscaleEffect.rampOffset = -1f;
        grayscaleEffect.enabled = true;
        SetActiveHUD(IdentityComposante.None);

        ShowHomeScreen();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if (currentState == ExperienceState.Sync)
                ShowHomeScreen();
            else if (currentState == ExperienceState.Loading)
                ShowSyncScreen();
            else if (currentState == ExperienceState.Experience)
                ShowLoadingScreen();
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            if (currentState == ExperienceState.Home)
                ShowSyncScreen();
            else if (currentState == ExperienceState.Sync)
                ShowLoadingScreen();
            else if (currentState == ExperienceState.Loading)
                ShowExperienceScreen();
        }
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

    public void OnRemoteRegistered()
    {
        Debug.Log("OnRemoteRegistered");
        if (currentState == ExperienceState.Home || currentState == ExperienceState.Sync)
        {
            ShowLoadingScreen();
        }
    }

    internal void OnDataLoaded()
    {
        Debug.Log("OnDataLoaded");
        ShowExperienceScreen();
    }

    void ShowHomeScreen()
    {
        Debug.Log("ShowHomeScreen");
        HomeScreen.DOKill();
        SyncScreen.DOKill();
        LoadingScreen.DOKill();

        HomeScreen.DOFade(1, 1);
        SyncScreen.DOFade(0, 1);
        LoadingScreen.DOFade(0, 1);

        currentState = ExperienceState.Home;
    }

    void ShowSyncScreen()
    {
        HomeScreen.DOKill();
        SyncScreen.DOKill();
        LoadingScreen.DOKill();

        HomeScreen.DOFade(0, 1);
        SyncScreen.DOFade(1, 1);
        LoadingScreen.DOFade(0, 1);

        currentState = ExperienceState.Sync;
    }

    void ShowLoadingScreen()
    {
        HomeScreen.DOKill();
        SyncScreen.DOKill();
        LoadingScreen.DOKill();

        HomeScreen.DOFade(0, 1);
        SyncScreen.DOFade(0, 1);
        LoadingScreen.DOFade(1, 1);

        currentState = ExperienceState.Loading;
    }

    void ShowExperienceScreen()
    {
        HomeScreen.DOKill();
        SyncScreen.DOKill();
        LoadingScreen.DOKill();

        HomeScreen.DOFade(0, 1);
        SyncScreen.DOFade(0, 1);
        LoadingScreen.DOFade(0, 1).OnComplete(() =>
        {
            FadeInScreen();
        });

        currentState = ExperienceState.Experience;
    }

    void FadeOutScreen()
    {
        grayscaleEffect.rampOffset = 0f;
        glitchEnabler.DoGlitch();
        grayscaleEffect.enabled = true;
        grayScaleTween.Kill();
        grayScaleTween = DOTween.To(() => grayscaleEffect.rampOffset, x => grayscaleEffect.rampOffset = x, -1f, .5f);
    }

    void FadeInScreen()
    {
        grayScaleTween.Kill();
        grayScaleTween = DOTween.To(() => grayscaleEffect.rampOffset, x => grayscaleEffect.rampOffset = x, 0, 1.5f).OnComplete(OnFadeInScreenComplete);
    }

    void OnFadeInScreenComplete()
    {
        glitchEnabler.DoGlitch();
        grayscaleEffect.enabled = false;
    }

    public void UserDetected(long userId, int userIndex)
    {
        if (currentState == ExperienceState.Home)
        {
            ShowSyncScreen();
        }
        else if (currentState == ExperienceState.Experience)
        {
            FadeInScreen();
        }
    }

    public void UserLost(long userId, int userIndex)
    {
        if (currentState == ExperienceState.Sync)
        {
            ShowHomeScreen();
        }
        else if (currentState == ExperienceState.Experience)
        {
            FadeOutScreen();
        }
    }

    public void GestureInProgress(long userId, int userIndex, KinectGestures.Gestures gesture, float progress, KinectInterop.JointType joint, Vector3 screenPos)
    {

    }

    public bool GestureCompleted(long userId, int userIndex, KinectGestures.Gestures gesture, KinectInterop.JointType joint, Vector3 screenPos)
    {
        return false;
    }

    public bool GestureCancelled(long userId, int userIndex, KinectGestures.Gestures gesture, KinectInterop.JointType joint)
    {
        return false;
    }
}
