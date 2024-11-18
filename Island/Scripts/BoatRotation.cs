using UnityEngine;
using UnityEngine.InputSystem;

public class BoatRotation : MonoBehaviour
{
    public float acceleration = 0.1f;
    public float deceleration = 0.05f;
    public float maxSpeed = 5f;
    public float maxYRotation = 80f, originYRotation = 0f;
    public float rotationSpeed = 45f;
    public bool workingWithXRotation = false;

    private float forwardSpeed = 0f;
    private float rotationInput = 0f;

    void Update()
    {
        HandleMovementInput();
        HandleRotationInput();

        // Apply rotation and movement
        ApplyRotation();
        ApplyMovement();
    }

    private void HandleMovementInput()
    {
        if (Keyboard.current.sKey.isPressed)
        {
            forwardSpeed += acceleration * Time.deltaTime;
        }
        else if (Keyboard.current.wKey.isPressed)
        {
            forwardSpeed -= acceleration * Time.deltaTime;
        }
        else
        {
            // Gradual deceleration
            if (forwardSpeed > 0)
            {
                forwardSpeed -= deceleration * Time.deltaTime;
                forwardSpeed = Mathf.Max(forwardSpeed, 0); // Prevent overshooting into negative
            }
            else if (forwardSpeed < 0)
            {
                forwardSpeed += deceleration * Time.deltaTime;
                forwardSpeed = Mathf.Min(forwardSpeed, 0); // Prevent overshooting into positive
            }
        }

        // Clamp forward speed to max speed limits
        forwardSpeed = Mathf.Clamp(forwardSpeed, -maxSpeed, maxSpeed);
    }

    private void HandleRotationInput()
    {
        rotationInput = 0f;

        if (Keyboard.current.dKey.isPressed)
        {
            rotationInput = 1f;
        }
        else if (Keyboard.current.aKey.isPressed)
        {
            rotationInput = -1f;
        }

        // Restrict rotation based on mode
        if (!workingWithXRotation)
        {
            float currentYRotation = NormalizeAngle(transform.eulerAngles.y);
            if ((currentYRotation >= maxYRotation && rotationInput > 0f) || (currentYRotation <= -maxYRotation && rotationInput < 0f))
            {
                rotationInput = 0f;
            }
        }
        else
        {
            float currentXRotation = transform.rotation.x;
            if ((currentXRotation >= 0.63f && rotationInput < 0f) || (currentXRotation <= 0.33f && rotationInput > 0f))
            {
                rotationInput = 0f;
            }
        }
    }

    private void ApplyRotation()
    {
        float angle = rotationInput * rotationSpeed * Time.deltaTime;

        // Rotate around Y-axis for statue
        transform.Rotate(Vector3.up, angle);

    }

    private void ApplyMovement()
    {
        Vector3 movement = Vector3.zero;

        if (!workingWithXRotation)
        {
            // Move along Z-axis (forward/backward) for statue
            movement = transform.forward * forwardSpeed * Time.deltaTime;
        }
        else
        {
            // Move along X-axis (forward/backward) for the island
            movement = new Vector3(forwardSpeed * Time.deltaTime, 0, 0);
        }

        transform.Translate(movement, Space.World);
    }



    private float NormalizeAngle(float angle)
    {
        if (angle > 180f)
        {
            angle -= 360f;
        }
        return angle;
    }
}
