using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour
{
    static AudioSource AudioSource;
    public AudioClip AudioClip;

    // Start is called before the first frame update
    void Start() {
        AudioSource = GetComponent<AudioSource>();
        AudioSource.PlayOneShot(AudioClip);
        AudioSource.PlayScheduled(AudioSettings.dspTime + AudioClip.length);
    }
}
