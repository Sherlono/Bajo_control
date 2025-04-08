using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public bool colided = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Calculate the collision force
        float collisionForce = collision.relativeVelocity.magnitude;

        // Check if the collision force exceeds the threshold
        if (collisionForce > 90.0f)
        {
            // Implement your action here
            Debug.Log("Strong collision detected! Force: " + collisionForce);
            // Example action: Destroy the GameObject
            colided = true;
        }
    }
}
