using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public bool colided = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        float collisionForce = collision.relativeVelocity.magnitude;

        if (collisionForce > 80.0f)     
        {
            //Debug.Log("Strong collision detected! Force: " + collisionForce);
            colided = true;
        }
    }
}
