using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip AcSound;

    public void PlaySoundClip()
    {
        audioSource.clip = AcSound;
        audioSource.Play();
    }
}
