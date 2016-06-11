using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class RandomGlitchEnabler : MonoBehaviour {

    public GlitchEffect glitchEffect;
    public float averageActivationTime = 5;
    public float averageDeactivationTime = 0.5f;
    public float minIntensity = 0f;
    public float maxIntensity = 2f;

    AudioManager audioManager;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        glitchEffect.enabled = false;
        StartCoroutine(ActivationLoop());
    }

    IEnumerator ActivationLoop()
    {
        while (true)
        {
            glitchEffect.intensity = Random.Range(minIntensity, maxIntensity);
            glitchEffect.enabled = true;
            audioManager.StartNoiseSound();
            yield return new WaitForSeconds(Random.Range(0, averageDeactivationTime * 2));
            glitchEffect.enabled = false;
            audioManager.StopNoiseSound();
            yield return new WaitForSeconds(Random.Range(0, averageActivationTime * 2));
        }
    }

    IEnumerator ActivateGlitch()
    {
        glitchEffect.intensity = Random.Range(minIntensity, maxIntensity);
        glitchEffect.enabled = true;
        audioManager.StartNoiseSound();
        yield return new WaitForSeconds(Random.Range(0, averageDeactivationTime * 2));
        glitchEffect.enabled = false;
        audioManager.StopNoiseSound();
    }

    public void DoGlitch()
    {
        StartCoroutine(ActivateGlitch());
    }
}
