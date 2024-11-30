using UnityEngine;


//This script play a sound
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
