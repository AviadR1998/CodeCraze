using System.Collections;
using UnityEngine;

public class SwingMovement : MonoBehaviour
{
    public float speed = 100f;
    // Speed of the rotation in degrees per second.
    private int targetRotations = 0;
    // Total number of full rotations (in terms of 360 degrees) to complete.
    private float currentRotation = 0;
    // Tracks the current amount of completed rotations.
    private bool isSpinning = false;
    public AudioClip rotationSound;
    private AudioSource audioSource;
    public bool isCarouselRunning = false;
    // Indicates if the carousel is actively spinning.

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Check if the swing is spinning and hasn't reached the target rotations yet.
        if (isSpinning && currentRotation < targetRotations)
        {
            //Mathematical calculations.
            float rotationAmount = speed * Time.deltaTime;
            transform.Rotate(0, rotationAmount, 0);
            currentRotation += Mathf.Abs(rotationAmount / 360);
            //Target reached.
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
            //Because isSpinning = true noq the update function is working.
            while (isSpinning)
            {
                // Wait until the rotation is complete (controlled by `Update`).
                yield return null;
            }
            //Sound when rotation is done.
            if (rotationSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(rotationSound);
            }
            //PAUSE for 1 second before starting the next rotation.
            yield return new WaitForSeconds(1f);
        }
        isCarouselRunning = false;
    }

    //We call this function from another script after th player decide the num of rotations.
    public void StartCarousel(int numberOfRotations)
    {
        StartCoroutine(StartCarouselWithPauses(numberOfRotations));
    }
}
