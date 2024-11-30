using UnityEngine;
using UnityEngine.InputSystem;



//This script controls a sailing boat, sails it and control the rotation
public class RotateBoat : MonoBehaviour
{
    public float acceleration = 0.1f;
    public float deceleration = 0.05f;
    public float maxSpeed = 5f, maxYRotation = 80f, originYRotation = 0, maxHeight = 1f, minHeight = -0.2f;
    public float rotationSpeed = 45f;
    public Transform downBarrier, upBarrier;
    public bool limitXPosition = false, limitZPosition = false, goingBack = false;
    public char xyzRotation = 'y';
    private float forwardSpeed = 0f;
    private float rotationInput = 0f;
    private float originZPosition, originXPosition;

    public float barrierLimit = 60f; // Maximum absolute Z position allowed

    private void Start()
    {
        originZPosition = transform.position.z;
        originXPosition = transform.position.x;
    }


    void Update()
    {
        // Check input for forward and backward movement
        if (Keyboard.current.wKey.isPressed)
        {
            forwardSpeed -= acceleration * Time.deltaTime;
        }
        else if (Keyboard.current.sKey.isPressed)
        {
            forwardSpeed += acceleration * Time.deltaTime;
        }
        else
        {
            // Gradual deceleration
            if (forwardSpeed > 0)
            {
                forwardSpeed -= deceleration * Time.deltaTime;
            }
            else if (forwardSpeed < 0)
            {
                forwardSpeed += deceleration * Time.deltaTime;
            }
        }

        // Clamp forward speed to max speed limits
        forwardSpeed = Mathf.Clamp(forwardSpeed, -maxSpeed, maxSpeed);

        // Check input for rotation
        rotationInput = 0f;
        if (Keyboard.current.dKey.isPressed)
        {
            rotationInput = 1f;
        }
        else if (Keyboard.current.aKey.isPressed)
        {
            rotationInput = -1f;
        }



        if (xyzRotation == 'y')
        {
            float boatYRotation = transform.eulerAngles.y;
            if (boatYRotation > 180)
            {
                boatYRotation -= 360;
            }
            if ((boatYRotation >= maxYRotation && rotationInput == -1f) || (boatYRotation <= -maxYRotation && rotationInput == 1f))
            {
                rotationInput = 0;
            }
        }
        else if (xyzRotation == 'x')
        {
            float boatXRotation = transform.rotation.x;
            if ((boatXRotation <= 0.33f && rotationInput == 1f) || (boatXRotation >= 0.63f && rotationInput == -1f))
            {
                rotationInput = 0;
            }
        }
        else
        {

            float boatZRotation = transform.rotation.z;
            float rightLimit = -0.2f, leftLimit = 0.2f;
            if (goingBack)
            {
                rightLimit = 0.35f;
                leftLimit = 0.65f;
            }
            if ((boatZRotation <= rightLimit && rotationInput == 1f) || (boatZRotation >= leftLimit && rotationInput == -1f))
            {
                rotationInput = 0;
            }

        }





        float angle = rotationInput * rotationSpeed * Time.deltaTime;
        // Apply rotation and movement
        transform.Rotate(Vector3.up, angle);

        // Moves the boat based on its own forward direction
        if (xyzRotation == 'y')
        {
            transform.Translate(transform.forward * forwardSpeed * Time.deltaTime);
        }
        else if (xyzRotation == 'x')
        {
            transform.Translate(new Vector3(transform.forward.x, transform.forward.y, -transform.right.z) * forwardSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(new Vector3(-transform.forward.x, transform.forward.y, transform.right.z) * forwardSpeed * Time.deltaTime);
        }



        //Restrict Z position
        if (limitZPosition)
        {
            Vector3 position = transform.position;
            position.z = Mathf.Clamp(position.z, originZPosition - barrierLimit, originZPosition + barrierLimit);
            transform.position = position;

            if (transform.position.x > downBarrier.position.x)
            {
                transform.position = new Vector3(downBarrier.position.x, transform.position.y, transform.position.z);
            }
            else if (transform.position.x < upBarrier.position.x)
            {
                transform.position = new Vector3(upBarrier.position.x, transform.position.y, transform.position.z);
            }
        }

        //Restrict X position
        if (limitXPosition)
        {
            Vector3 position = transform.position;
            position.x = Mathf.Clamp(position.x, originXPosition - barrierLimit, originXPosition + barrierLimit);
            transform.position = position;

            if (transform.position.z > downBarrier.position.z)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, downBarrier.position.z);
            }
            else if (transform.position.z < upBarrier.position.z)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, upBarrier.position.z);
            }
        }

        Vector3 positionUpdate = transform.position;
        positionUpdate.y = Mathf.Clamp(positionUpdate.y, minHeight, maxHeight);
        transform.position = positionUpdate;


    }
}




