using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : MonoBehaviour {

    public AudioMixerSnapshot homeSnapshot;
    public AudioMixerSnapshot experienceSnapshot;
    public AudioMixerSnapshot inUISnapshot;
    public AudioMixerSnapshot photoSnapshot;

    public AudioClip homeSound;
    public AudioClip experienceSound;
    public AudioClip[] UISounds;
    public AudioClip[] validateSounds;
    public AudioClip unvalidateSounds;
    public AudioClip photoSound;
    public AudioClip beepSound;

    public AudioMixerGroup HomeMixerGroup;
    public AudioMixerGroup ExperienceMixerGroup;
    public AudioMixerGroup UISoundsMixerGroup;
    public AudioMixerGroup GlitchesMixerGroup;
    public AudioMixerGroup PhotoMixerGroup;

    public AudioClip noiseSound;

    AudioSource HomeSource;
    AudioSource ExperienceSource;
    AudioSource UISoundsSource;
    AudioSource noiseSoundSource;
    AudioSource photoSoundSource;

    void Start()
    {
        HomeSource = gameObject.AddComponent<AudioSource>();
        HomeSource.outputAudioMixerGroup = HomeMixerGroup;
        HomeSource.clip = homeSound;
        HomeSource.playOnAwake = false;
        HomeSource.loop = true;

        ExperienceSource = gameObject.AddComponent<AudioSource>();
        ExperienceSource.outputAudioMixerGroup = ExperienceMixerGroup;
        ExperienceSource.clip = experienceSound;
        ExperienceSource.playOnAwake = false;
        ExperienceSource.loop = true;

        UISoundsSource = gameObject.AddComponent<AudioSource>();
        UISoundsSource.outputAudioMixerGroup = UISoundsMixerGroup;
        UISoundsSource.playOnAwake = false;
        UISoundsSource.loop = false;

        noiseSoundSource = gameObject.AddComponent<AudioSource>();
        noiseSoundSource.outputAudioMixerGroup = GlitchesMixerGroup;
        noiseSoundSource.clip = noiseSound;
        noiseSoundSource.playOnAwake = false;
        noiseSoundSource.loop = true;

        photoSoundSource = gameObject.AddComponent<AudioSource>();
        photoSoundSource.outputAudioMixerGroup = PhotoMixerGroup;
        photoSoundSource.playOnAwake = false;
        photoSoundSource.loop = false;
    }

    public void ToHomeSoundtrack()
    {
        HomeSource.Play();
        homeSnapshot.TransitionTo(1f);
    }

    public void ToExperienceSoundtrack()
    {
        ExperienceSource.Play();
        experienceSnapshot.TransitionTo(0.5f);
    }

    public void ToPhotoSoundtrack()
    {
        photoSnapshot.TransitionTo(3.5f);
    }

    public void AfterPhotoSoundtrack()
    {
        experienceSnapshot.TransitionTo(2f);
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

    public void PlayValidateSound()
    {
        UISoundsSource.PlayOneShot(validateSounds[Random.Range(0, validateSounds.Length)]);
    }

    public void PlayUnvalidateSound()
    {
        UISoundsSource.PlayOneShot(unvalidateSounds);
    }

    public void PlayPhotoSound()
    {
        photoSoundSource.PlayOneShot(photoSound);
    }

    public void PlayPhotoBeep()
    {
        photoSoundSource.PlayOneShot(beepSound);
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
