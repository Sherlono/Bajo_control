using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public static event Action onEnter;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        onEnter?.Invoke();
        Debug.Log("Hazard touched!");
    }
}
