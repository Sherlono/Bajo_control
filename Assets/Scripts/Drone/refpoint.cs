using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class refpoint : MonoBehaviour
{
    public static event Action onReached;

    public static int pointCount;
    public int id;

    private void Awake()
    {
        id = pointCount;
        pointCount++;
    }

    private void OnDestroy()
    {
        pointCount--;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(id == DroneGameManager.currentPoint)
        {
            GetComponent<SpriteRenderer>().color = new Color(0, 0.9f, 0, 1f);
            onReached?.Invoke();
        }

    }
}
