// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Rendering.HighDefinition;

// public class Floater : MonoBehaviour
// {
//     public Rigidbody rigidBody;
//     public float depthBeforeSubmerged = 1.2f;
//     public float displacementAmount = 3f;
//     public int floaterCounter = 1;
//     public float waterDrag = 1.5f;
//     public float waterAngularDrag = 0.5f;

//     private void FixedUpdate()
//     {
//         // rigidBody.AddForceAtPosition(Physics.gravity / floaterCounter, transform.position, ForceMode.Acceleration);
//         if (transform.position.y < 2f)
//         {
//             float displacementMultiplier = Mathf.Clamp01(-transform.position.y / depthBeforeSubmerged) * displacementAmount;
//             rigidBody.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), ForceMode.Acceleration);
//             rigidBody.AddForce(displacementMultiplier * -rigidBody.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
//             rigidBody.AddTorque(displacementMultiplier * -rigidBody.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
//         }
//     }
// }




// public float depthBeforeSubmerged = 1f;
// public float displacementAmount = 3f;
// public int floaterCounter = 1;
// public float waterDrag = 0.99f;
// public float waterAngularDrag = 0.5f;










// using UnityEngine;


// public class Floater : MonoBehaviour
// {
//     // public Rigidbody rigidBody;
//     // public float depthBeforeSubmerged = 1.2f;
//     // public float displacementAmount = 3f;
//     // public int floaterCounter = 1;
//     // public float waterDrag = 1.5f;
//     // public float waterAngularDrag = 0.5f;

//     // public float minimumHeightAboveWater = 3.5f;  // The height at which the boat should stay above water


//     public Rigidbody rigidBody;
//     public float depthBeforeSubmerged = 1.5f;
//     public float displacementAmount = 2.5f;
//     public int floaterCounter = 1;
//     public float waterDrag = 2f;
//     public float waterAngularDrag = 0.7f;

//     public float minimumHeightAboveWater = 2f;

//     private void FixedUpdate()
//     {
//         // Check if the boat is below the water level
//         if (transform.position.y < -1f)
//         {
//             // Calculate the buoyant force based on how submerged the boat is
//             float displacementMultiplier = Mathf.Clamp01(-transform.position.y / depthBeforeSubmerged) * displacementAmount;

//             // Apply buoyant force to lift the boat
//             rigidBody.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), ForceMode.Acceleration);

//             // Apply drag forces for smoothing the boat's movement
//             rigidBody.AddForce(displacementMultiplier * -rigidBody.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
//             rigidBody.AddTorque(displacementMultiplier * -rigidBody.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
//         }

//         // Ensure the boat stays above the water level
//         if (transform.position.y < minimumHeightAboveWater)
//         {
//             // Set the boat's Y position to the minimum height above the water
//             transform.position = new Vector3(transform.position.x, minimumHeightAboveWater, transform.position.z);
//         }
//     }
// }








// using UnityEngine;

// public class Floater : MonoBehaviour
// {

//     public Rigidbody rigidBody;
//     public float depthBeforeSubmerged = 2f; // Increased to make the boat respond more gradually to submersion
//     public float displacementAmount = 1.5f; // Reduced to decrease the buoyancy force
//     public int floaterCounter = 1;
//     public float waterDrag = 3f; // Increased for smoother motion
//     public float waterAngularDrag = 1f; // Increased for smoother angular movement

//     public float minimumHeightAboveWater = 1f; // Adjusted to reduce enforced height

//     private void FixedUpdate()
//     {
//         // Check if the boat is below the water level
//         if (transform.position.y < 0f)
//         {
//             // Calculate the buoyant force based on how submerged the boat is
//             float displacementMultiplier = Mathf.Clamp01(-transform.position.y / depthBeforeSubmerged) * displacementAmount;

//             // Apply buoyant force to lift the boat
//             rigidBody.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), ForceMode.Acceleration);

//             // Apply drag forces for smoothing the boat's movement
//             rigidBody.AddForce(-rigidBody.velocity * waterDrag * displacementMultiplier * Time.fixedDeltaTime, ForceMode.VelocityChange);
//             rigidBody.AddTorque(-rigidBody.angularVelocity * waterAngularDrag * displacementMultiplier * Time.fixedDeltaTime, ForceMode.VelocityChange);
//         }

//         // Slight correction to keep the boat at minimum height above water
//         if (transform.position.y < minimumHeightAboveWater)
//         {
//             rigidBody.AddForce(new Vector3(0f, (minimumHeightAboveWater - transform.position.y) * 10f, 0f), ForceMode.Acceleration);
//         }
//     }


// }






using UnityEngine;

public class Floater : MonoBehaviour
{
    public Rigidbody rigidBody;
    public float depthBeforeSubmerged = 2f; // Adjust for smoother submersion response
    public float displacementAmount = 1.5f; // Adjust buoyancy force
    public int floaterCounter = 1;
    public float waterDrag = 3f; // Increased for smoother motion
    public float waterAngularDrag = 1f; // Increased for smoother angular movement

    public float minimumHeightAboveWater = 1f; // Height above water level
    public float maxHeightAboveWater = 3f; // Limit upward movement to prevent flying

    private void FixedUpdate()
    {
        // Calculate how submerged the boat is
        if (transform.position.y < 0f)
        {
            float displacementMultiplier = Mathf.Clamp01(-transform.position.y / depthBeforeSubmerged) * displacementAmount;

            // Apply buoyant force to lift the boat
            rigidBody.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), ForceMode.Acceleration);

            // Apply drag forces for smoothing the boat's movement
            rigidBody.AddForce(-rigidBody.velocity * waterDrag * displacementMultiplier * Time.fixedDeltaTime, ForceMode.VelocityChange);
            rigidBody.AddTorque(-rigidBody.angularVelocity * waterAngularDrag * displacementMultiplier * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }

        // Correction to prevent the boat from flying too high
        if (transform.position.y > maxHeightAboveWater)
        {
            rigidBody.AddForce(new Vector3(0f, (maxHeightAboveWater - transform.position.y) * 10f, 0f), ForceMode.Acceleration);
        }

        // Keep the boat close to the water surface without snapping
        if (transform.position.y < minimumHeightAboveWater)
        {
            rigidBody.AddForce(new Vector3(0f, (minimumHeightAboveWater - transform.position.y) * 10f, 0f), ForceMode.Acceleration);
        }
    }
}


