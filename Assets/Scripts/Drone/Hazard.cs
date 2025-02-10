using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    private hdrone drone;
    // Start is called before the first frame update
    void Start()
    {
        drone = GameObject.FindAnyObjectByType<hdrone>().GetComponent<hdrone>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        drone.Power(false);
    }
}
