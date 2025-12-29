using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public static event Action onCollide;

    void OnCollisionEnter2D(Collision2D collision)
    {
        float collisionForce = collision.relativeVelocity.magnitude;

        if (collisionForce > 80.0f)     
        {
            onCollide?.Invoke();
            Debug.Log("Collision detected!");
        }
    }
}
