// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.InputSystem;
// using UnityEngine.Rendering;
// public class RotateBoat : MonoBehaviour
// {

//     public Transform boatPosition;
//     public float speed;
//     private float rlSpeed = 0;
//     // Remove the need for an external Transform and use the attached object's transform directly
//     private void Update()
//     {
//         if (Keyboard.current.jKey.IsPressed())
//         {
//             // Rotate the boat object itself
//             // transform.Rotate(0, 0, 10f);
//             // print("E key pressed - Boat rotated");

//             boatPosition.RotateAround(boatPosition.position, Vector3.up, speed);
//             rlSpeed += speed;
//             Debug.Log("Rotating Boat around Y-axis: " + boatPosition.eulerAngles);
//         }

//         else if (Keyboard.current.gKey.IsPressed())
//         {
//             // Rotate the boat object itself
//             // transform.Rotate(0, 0, 10f);
//             // print("E key pressed - Boat rotated");

//             boatPosition.RotateAround(boatPosition.position, Vector3.down, speed);
//             rlSpeed -= speed;
//             Debug.Log("Rotating Boat around Y-axis: " + boatPosition.eulerAngles);
//         }

//         else if (Keyboard.current.yKey.IsPressed())
//         {
//             // Rotate the boat object itself
//             // transform.Rotate(0, 0, 10f);
//             // print("E key pressed - Boat rotated");

//             boatPosition.position = new Vector3(boatPosition.position.x + rlSpeed, boatPosition.position.y, boatPosition.position.z - speed);
//             Debug.Log("Rotating Boat around Y-axis: " + boatPosition.eulerAngles);
//         }

//         else if (Keyboard.current.hKey.IsPressed())
//         {
//             // Rotate the boat object itself
//             // transform.Rotate(0, 0, 10f);
//             // print("E key pressed - Boat rotated");

//             boatPosition.position = new Vector3(boatPosition.position.x, boatPosition.position.y, boatPosition.position.z + speed);
//             Debug.Log("Rotating Boat around Y-axis: " + boatPosition.eulerAngles);
//         }
//     }
// }


using UnityEngine;
using UnityEngine.InputSystem;

public class RotateBoat : MonoBehaviour
{
    public float acceleration = 0.1f;
    public float deceleration = 0.05f;
    public float maxSpeed = 5f, maxYRotation = 80f, originYRotation = 0;
    public float rotationSpeed = 45f;
    public bool workingWithXRotation = false;
    private float forwardSpeed = 0f;
    private float rotationInput = 0f;


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

        float boatYRotation = transform.eulerAngles.y;
        float boatXRotation = transform.rotation.x;
        if (boatYRotation > 180)
        {
            boatYRotation -= 360;
        }

        if (!workingWithXRotation && ((boatYRotation >= maxYRotation && rotationInput == -1f) || (boatYRotation <= -maxYRotation && rotationInput == 1f)))
        {
            rotationInput = 0;
        }
        else if (workingWithXRotation && ((boatXRotation <= 0.33f && rotationInput == 1f) || (boatXRotation >= 0.63f && rotationInput == -1f)))
        {
            rotationInput = 0;
        }


        float angle = rotationInput * rotationSpeed * Time.deltaTime;
        // Apply rotation and movement
        transform.Rotate(Vector3.up, angle);

        // Moves the boat based on its own forward direction
        if (!workingWithXRotation)
        {
            transform.Translate(transform.forward * forwardSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(new Vector3(transform.forward.x, transform.forward.y, -transform.right.z) * forwardSpeed * Time.deltaTime);
        }

    }

}



