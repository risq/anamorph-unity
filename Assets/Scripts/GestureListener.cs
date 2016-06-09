using UnityEngine;
using System.Collections;
using System;
using UnityStandardAssets.ImageEffects;
using DG.Tweening;

public class GestureListener : MonoBehaviour, KinectGestures.GestureListenerInterface
{
    Tweener grayScaleTween;
    Grayscale grayscaleEffect;
    RandomGlitchEnabler glitchEnabler;

    public bool GestureCancelled(long userId, int userIndex, KinectGestures.Gestures gesture, KinectInterop.JointType joint)
    {
        return false;
    }

    public bool GestureCompleted(long userId, int userIndex, KinectGestures.Gestures gesture, KinectInterop.JointType joint, Vector3 screenPos)
    {
        return false;
    }

    public void GestureInProgress(long userId, int userIndex, KinectGestures.Gestures gesture, float progress, KinectInterop.JointType joint, Vector3 screenPos)
    {

    }

    public void UserDetected(long userId, int userIndex)
    {
        Debug.Log("User Detected !!");
        FadeInScreen();
    }

    public void UserLost(long userId, int userIndex)
    {
        Debug.Log("User Lost !!");
        FadeOutScreen();
    }

    // Use this for initialization
    void Start () {
        glitchEnabler = Camera.main.GetComponent<RandomGlitchEnabler>();
        grayscaleEffect = Camera.main.GetComponent<Grayscale>();
        grayscaleEffect.rampOffset = -1f;
        grayscaleEffect.enabled = true;
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
        grayScaleTween = DOTween.To(() => grayscaleEffect.rampOffset, x => grayscaleEffect.rampOffset = x, 0, 1.5f).OnComplete(OnFadeInComplete);
    }

    void OnFadeInComplete()
    {
        glitchEnabler.DoGlitch();
        grayscaleEffect.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
