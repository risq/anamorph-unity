using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : MonoBehaviour {

    public AudioMixerSnapshot homeSnapshot;
    public AudioMixerSnapshot experienceSnapshot;
    public AudioMixerSnapshot inUISnapshot;

    public AudioClip[] UISounds;
    public AudioMixerGroup UISoundsMixerGroup;
    public AudioMixerGroup GlitchesMixerGroup;

    public AudioClip noiseSound;

    AudioSource UISoundsSource;
    AudioSource noiseSoundSource;

    void Start()
    {
        UISoundsSource = gameObject.AddComponent<AudioSource>();
        UISoundsSource.outputAudioMixerGroup = UISoundsMixerGroup;

        noiseSoundSource = gameObject.AddComponent<AudioSource>();
        noiseSoundSource.outputAudioMixerGroup = GlitchesMixerGroup;
        noiseSoundSource.clip = noiseSound;
        noiseSoundSource.loop = true;
    }

    public void ToExperienceSoundtrack()
    {
        experienceSnapshot.TransitionTo(0.5f);
    }

    public void ToHomeSoundtrack()
    {
        homeSnapshot.TransitionTo(0.5f);
    }

    public void OnUIOpen()
    {
        inUISnapshot.TransitionTo(0.6f);
    }

    public void OnUIClose()
    {
        experienceSnapshot.TransitionTo(0.3f);
    }

    public void PlayUISound()
    {
        UISoundsSource.PlayOneShot(UISounds[Random.Range(0, UISounds.Length)]);
    }

    public void StartNoiseSound()
    {
        noiseSoundSource.Play();
    }

    public void StopNoiseSound()
    {
        noiseSoundSource.Stop();
    }
}
