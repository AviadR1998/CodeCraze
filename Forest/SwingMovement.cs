using System.Collections;
using UnityEngine;

public class SwingMovement : MonoBehaviour
{
    public float speed = 100f;
    private int targetRotations = 0;
    private float currentRotation = 0;
    private bool isSpinning = false;

    public AudioClip rotationSound;
    private AudioSource audioSource;
    public bool isCarouselRunning = false;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isSpinning && currentRotation < targetRotations)
        {

            float rotationAmount = speed * Time.deltaTime;
            transform.Rotate(0, rotationAmount, 0);
            currentRotation += Mathf.Abs(rotationAmount / 360);
            if (currentRotation >= targetRotations)
            {
                isSpinning = false;
            }
        }
    }


    public IEnumerator StartCarouselWithPauses(int numberOfRotations)
    {
          isCarouselRunning = true;
        for (int i = 0; i < numberOfRotations; i++)
        {
            targetRotations = 1;
            currentRotation = 0;
            isSpinning = true;


            while (isSpinning)
            {
                yield return null;
            }


            if (rotationSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(rotationSound);
            }


            yield return new WaitForSeconds(1f);
        }
          isCarouselRunning = false;
    }


    public void StartCarousel(int numberOfRotations)
    {
        StartCoroutine(StartCarouselWithPauses(numberOfRotations)); // התחלת ה-Coroutine
    }
}
