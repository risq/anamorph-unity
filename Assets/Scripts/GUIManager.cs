using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityStandardAssets.ImageEffects;
using System;

public enum IdentityComposante { None, Activity, Influence, PassiveIdentity, Mood, Interests };
public enum IdentityCircle { None, Public, Private, Pro, Global };
public enum ExperienceState { Home, Sync, Loading, Experience, Photo };

public delegate void OnFadeDelegate();

public class GUIManager : MonoBehaviour, KinectGestures.GestureListenerInterface
{
    public HUDScreen HomeScreen;
    public HUDScreen SyncScreen;
    public HUDScreen LoadingScreen;
    public HUDScreen PhotoScreen;

    public Image overlay;
    public Image fullOverlay;

    public HUDGroup activityHUD;
    public HUDGroup influenceHUD;
    public HUDGroup passiveIdentityHUD;
    public HUDGroup moodHUD;
    public HUDGroup interestsHUD;

    public KinectCursor leftHandCursor;
    public KinectCursor rightHandCursor;
    public KinectCursor leftHandPhotoCursor;
    public KinectCursor rightHandPhotoCursor;

    public CanvasGroup fixedBackUI;
    public CanvasGroup fixedFrontUI;
    public CanvasGroup interfaceUI;
    public CanvasGroup HUDsUI;

    public float overlayFadeAmount = 0.82f;
    public float overlayFadeTime = 1f;

    HUDGroup currentHUD;

    IdentityComposante currentIdentityComposante;
    IdentityCircle currentIdentityCircle;
    ExperienceState currentState = ExperienceState.Home;

    Tweener overlayFadeTweener;
    Tweener grayScaleTween;

    Grayscale grayscaleEffect;
    RandomGlitchEnabler glitchEnabler;
    AudioManager audioManager;
    PhotoManager photoManager;

    bool started = false;
    bool photoButtonActivating = false;

    // Use this for initialization
    void Start () {
        audioManager = FindObjectOfType<AudioManager>();
        glitchEnabler = Camera.main.GetComponent<RandomGlitchEnabler>();
        grayscaleEffect = Camera.main.GetComponent<Grayscale>();
        photoManager = PhotoScreen.GetComponent<PhotoManager>();

        grayscaleEffect.rampOffset = -1f;
        grayscaleEffect.enabled = true;

        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(1);
        ShowHomeScreen();
        SetActiveHUD(IdentityComposante.None);
        started = true;
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
        else if (Input.GetKeyUp(KeyCode.H))
        {
            HideUI();
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            ShowUI();
        }
    }

    public void OnCursorValidate(KinectButton.ButtonType buttonType)
    {
        audioManager.PlayValidateSound();

        if (buttonType == KinectButton.ButtonType.Activity && !photoButtonActivating)
        {
            SetActiveHUD(IdentityComposante.Activity);
        }
        else if (buttonType == KinectButton.ButtonType.Influence && !photoButtonActivating)
        {
            SetActiveHUD(IdentityComposante.Influence);
        }
        else if (buttonType == KinectButton.ButtonType.PassiveIdentity && !photoButtonActivating)
        {
            SetActiveHUD(IdentityComposante.PassiveIdentity);
        }
        else if (buttonType == KinectButton.ButtonType.Mood && !photoButtonActivating)
        {
            SetActiveHUD(IdentityComposante.Mood);
        }
        else if (buttonType == KinectButton.ButtonType.Interests && !photoButtonActivating)
        {
            SetActiveHUD(IdentityComposante.Interests);
        }
        else if (currentHUD != null && buttonType == KinectButton.ButtonType.Private && !photoButtonActivating)
        {
            currentHUD.Filter(IdentityCircle.Private);
        }
        else if (currentHUD != null && buttonType == KinectButton.ButtonType.Public && !photoButtonActivating)
        {
            currentHUD.Filter(IdentityCircle.Public);
        }
        else if (currentHUD != null && buttonType == KinectButton.ButtonType.Pro && !photoButtonActivating)
        {
            currentHUD.Filter(IdentityCircle.Pro);
        }
        else if (buttonType == KinectButton.ButtonType.Photo && currentHUD == null)
        {
            ShowPhotoScreen();
        }
    }

    public void OnCursorSemivalidate(KinectButton.ButtonType buttonType)
    {
        if (buttonType == KinectButton.ButtonType.Photo && currentHUD == null)
        {
            photoButtonActivating = true;
            leftHandCursor.cursorEnabled = false;
            rightHandCursor.cursorEnabled = false;

            leftHandCursor.DisableAll();
            rightHandCursor.DisableAll();

            overlayFadeTweener.Kill();
            overlayFadeTweener = overlay.DOFade(overlayFadeAmount, overlayFadeTime * 5);
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
        else if (buttonType == KinectButton.ButtonType.Private || buttonType == KinectButton.ButtonType.Public || buttonType == KinectButton.ButtonType.Pro)
        {
            
        }
        else if (buttonType == KinectButton.ButtonType.Photo && photoButtonActivating)
        {
            photoButtonActivating = false;
            leftHandCursor.cursorEnabled = true;
            overlayFadeTweener.Kill();
            overlayFadeTweener = overlay.DOFade(0, overlayFadeTime);
        }
    }

    void SetActiveHUD(IdentityComposante activeHUD)
    {
        overlayFadeTweener.Kill();

        if (activeHUD == IdentityComposante.Activity)
        {
            currentIdentityComposante = IdentityComposante.Activity;

            activityHUD.Show();
            influenceHUD.Hide();
            passiveIdentityHUD.Hide();
            moodHUD.Hide();
            interestsHUD.Hide();

            currentHUD = activityHUD;
        }
        else if (activeHUD == IdentityComposante.Influence)
        {
            currentIdentityComposante = IdentityComposante.Influence;

            activityHUD.Hide();
            influenceHUD.Show();
            passiveIdentityHUD.Hide();
            moodHUD.Hide();
            interestsHUD.Hide();

            currentHUD = influenceHUD;
        }
        else if (activeHUD == IdentityComposante.PassiveIdentity)
        {
            currentIdentityComposante = IdentityComposante.PassiveIdentity;

            activityHUD.Hide();
            influenceHUD.Hide();
            passiveIdentityHUD.Show();
            moodHUD.Hide();
            interestsHUD.Hide();

            currentHUD = passiveIdentityHUD;
        }
        else if (activeHUD == IdentityComposante.Mood)
        {
            currentIdentityComposante = IdentityComposante.Mood;

            activityHUD.Hide();
            influenceHUD.Hide();
            passiveIdentityHUD.Hide();
            moodHUD.Show();
            interestsHUD.Hide();

            currentHUD = moodHUD;
        }
        else if (activeHUD == IdentityComposante.Interests)
        {
            currentIdentityComposante = IdentityComposante.Interests;

            activityHUD.Hide();
            influenceHUD.Hide();
            passiveIdentityHUD.Hide();
            moodHUD.Hide();
            interestsHUD.Show();

            currentHUD = interestsHUD;
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

            currentHUD = null;

            rightHandCursor.cursorEnabled = false;
            rightHandPhotoCursor.cursorEnabled = true;
            leftHandPhotoCursor.cursorEnabled = true;

            if (currentState == ExperienceState.Experience)
                audioManager.OnUIClose();
        }

        if (currentHUD)
        {
            overlayFadeTweener = overlay.DOFade(overlayFadeAmount, overlayFadeTime);
            rightHandCursor.cursorEnabled = true;
            rightHandPhotoCursor.cursorEnabled = false;
            leftHandPhotoCursor.cursorEnabled = false;

            leftHandPhotoCursor.DisableAll();
            rightHandPhotoCursor.DisableAll();

            currentHUD.Filter(IdentityCircle.Global);
            audioManager.OnUIOpen();
        }

        rightHandCursor.DisableAll();
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
        fullOverlay.DOFade(1, 0);

        audioManager.ToHomeSoundtrack();
        grayscaleEffect.rampOffset = -1f;

        HideUI();

        HomeScreen.FadeIn();
        SyncScreen.FadeOut();
        LoadingScreen.FadeOut();
        PhotoScreen.FadeOut();

        currentState = ExperienceState.Home;
        leftHandCursor.cursorEnabled = false;
        leftHandPhotoCursor.cursorEnabled = false;
        rightHandPhotoCursor.cursorEnabled = false;
        audioManager.PlayValidateSound();
    }

    void ShowSyncScreen()
    {
        audioManager.ToHomeSoundtrack();
        grayscaleEffect.rampOffset = -1f;

        HomeScreen.FadeOut();
        SyncScreen.FadeIn();
        LoadingScreen.FadeOut();
        PhotoScreen.FadeOut();

        currentState = ExperienceState.Sync;
        leftHandCursor.cursorEnabled = false;
        leftHandPhotoCursor.cursorEnabled = false;
        rightHandPhotoCursor.cursorEnabled = false;
        audioManager.PlayValidateSound();
    }

    void ShowLoadingScreen()
    {
        audioManager.ToHomeSoundtrack();
        grayscaleEffect.rampOffset = -1f;

        HomeScreen.FadeOut();
        SyncScreen.FadeOut();
        LoadingScreen.FadeIn();
        PhotoScreen.FadeOut();

        currentState = ExperienceState.Loading;
        leftHandCursor.cursorEnabled = false;
        leftHandPhotoCursor.cursorEnabled = false;
        rightHandPhotoCursor.cursorEnabled = false;
        audioManager.PlayValidateSound();
    }

    void ShowExperienceScreen()
    {
        fullOverlay.DOFade(0, 0);

        audioManager.ToExperienceSoundtrack();
        SetActiveHUD(IdentityComposante.None);

        HomeScreen.FadeOut();
        SyncScreen.FadeOut();
        LoadingScreen.canvasGroup.DOFade(0, 1).OnComplete(() =>
        {
            FadeInScreen();
            ShowUI(5f);
        });
        PhotoScreen.FadeOut();

        currentState = ExperienceState.Experience;
    }

    void ShowPhotoScreen()
    {
        HomeScreen.FadeOut();
        SyncScreen.FadeOut();
        LoadingScreen.FadeOut();
        PhotoScreen.FadeIn();

        currentState = ExperienceState.Photo;
        SetActiveHUD(IdentityComposante.None);

        HideUI();

        leftHandCursor.enabled = false;
        leftHandPhotoCursor.enabled = false;
        rightHandCursor.enabled = false;
        rightHandCursor.enabled = false;

        audioManager.ToPhotoSoundtrack();
        audioManager.PlayValidateSound();

        photoManager.TakePhoto();
    }


    void FadeOutScreen(TweenCallback OnFadeOut = null)
    {
        grayscaleEffect.rampOffset = 0f;
        glitchEnabler.DoGlitch();
        grayscaleEffect.enabled = true;
        grayScaleTween.Kill();
        grayScaleTween = DOTween.To(() => grayscaleEffect.rampOffset, x => grayscaleEffect.rampOffset = x, -1f, .5f).OnComplete(OnFadeOut);
        leftHandCursor.cursorEnabled = false;
        leftHandPhotoCursor.cursorEnabled = false;
        rightHandPhotoCursor.cursorEnabled = false;

        leftHandCursor.DisableAll();
        leftHandPhotoCursor.DisableAll();
        rightHandCursor.DisableAll();
        rightHandPhotoCursor.DisableAll();
    }

    void FadeInScreen()
    {
        grayScaleTween.Kill();
        grayScaleTween = DOTween.To(() => grayscaleEffect.rampOffset, x => grayscaleEffect.rampOffset = x, 0, 1.5f).OnComplete(OnFadeInScreenComplete);
    }

    public void FadeOutInScreen(TweenCallback OnFadeOut)
    {
        FadeOutScreen(() =>
        {
            OnFadeOut();
            FadeInScreen();
        });
    }

     void OnFadeInScreenComplete()
    {
        glitchEnabler.DoGlitch();
        grayscaleEffect.enabled = false;
        leftHandCursor.cursorEnabled = true;
        leftHandPhotoCursor.cursorEnabled = true;
        rightHandPhotoCursor.cursorEnabled = true;
    }

    public void HideUI()
    {
        fixedBackUI.DOFade(0, overlayFadeTime);
        fixedFrontUI.DOFade(0, overlayFadeTime);
        interfaceUI.DOFade(0, overlayFadeTime);
        HUDsUI.DOFade(0, overlayFadeTime);
    }

    public void ShowUI(float time = 1f)
    {
        Debug.Log("showUI - " + time);
        fixedBackUI.DOFade(1, time);
        fixedFrontUI.DOFade(1, time);
        interfaceUI.DOFade(1, time);
        HUDsUI.DOFade(1, time);
    }

    public void UserDetected(long userId, int userIndex)
    {
        if (currentState == ExperienceState.Home)
        {
            if (started)
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
        else if (currentState == ExperienceState.Photo)
        {
            ShowHomeScreen();
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
