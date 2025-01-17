using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shaker : MonoBehaviour
{
    public PID script;
    public float power;

    private Vector3 initial_pos;
    private float limit;


    // Start is called before the first frame update
    void Start()
    {
        limit = 100;
        initial_pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        power = script.u;

        if (power >= limit)
        {
            transform.position = new Vector3(initial_pos.x + (Random.value - 0.5f) * 7.116239f, initial_pos.y + (Random.value - 0.5f) * 7.116239f, transform.position.z);
        }
        else
        {
            transform.position = initial_pos;
        }
    }
}
