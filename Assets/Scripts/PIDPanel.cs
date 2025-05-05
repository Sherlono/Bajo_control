using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIDPanel : MonoBehaviour
{
    [HideInInspector]
    public int state = 0;   // 0: Boton no precionado, 1: Moviendo panel hacia abajo, 2: Panel detenido y fuera de vista

    private float activate_time;
    private float _y;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (state == 0)
        {
            _y = transform.position.y;
        }
        else if (state == 1)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - (5.0f * Mathf.Sqrt(Time.time - activate_time)), transform.position.z);
            if (transform.position.y < -1200)
            {
                state = 2;
                //GetComponent<Collider2D>().enabled = false;
            }
        }
        else if (state == 3)    // Restart was called
        {
            float next_y = transform.position.y + (5.0f * Mathf.Sqrt(Time.time - activate_time));
            if (next_y < _y)
            {
                transform.position = new Vector3(transform.position.x, next_y, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, _y, transform.position.z);
                state = 0;
            }
        }
    }

    public void Restart()
    {
        //GetComponent<Collider2D>().enabled = true;
        activate_time = Time.time;
        state = 3;
    }

    public void Check()
    {
        if (state == 0)
        {
            activate_time = Time.time;
            state++;
        }
    }
}
