using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : MonoBehaviour {

    public AudioMixerSnapshot homeSnapshot;
    public AudioMixerSnapshot experienceSnapshot;

    public void ToExperienceSoundtrack()
    {
        experienceSnapshot.TransitionTo(0.3f);
    }

    public void ToHomeSoundtrack()
    {
        homeSnapshot.TransitionTo(0.3f);
    }
}
