using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class RandomGlitchEnabler : MonoBehaviour {

    public GlitchEffect glitchEffect;
    public float averageActivationTime = 5;
    public float averageDeactivationTime = 0.5f;
    public float minIntensity = 0f;
    public float maxIntensity = 2f;

    void Start()
    {
        glitchEffect.enabled = false;
        StartCoroutine(ActivationLoop());
    }

    IEnumerator ActivationLoop()
    {
        while (true)
        {
            glitchEffect.intensity = Random.Range(minIntensity, maxIntensity);
            glitchEffect.enabled = true;
            yield return new WaitForSeconds(Random.Range(0, averageDeactivationTime * 2));
            glitchEffect.enabled = false;
            yield return new WaitForSeconds(Random.Range(0, averageActivationTime * 2));
        }
    }
}
