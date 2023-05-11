using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectScript : MonoBehaviour
{
    static AudioSource AudioSource;

    // Start is called before the first frame update
    void Start() {
        AudioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(AudioClip AudioClip, float volumeMultiplier = 1f, bool loop = false) {
        if (loop) {
            AudioSource.clip = AudioClip;
            AudioSource.loop = true;
            AudioSource.Play();
        } else {
            AudioSource.PlayOneShot(AudioClip, volumeMultiplier);
        }
    }

    public static void StopSound() {
        AudioSource.Stop();
    }
}
