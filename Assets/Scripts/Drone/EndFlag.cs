using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EndFlag : MonoBehaviour
{
    public bool win = false;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        win = true;
    }
}
