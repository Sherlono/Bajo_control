using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayPID : MonoBehaviour
{
    private float activate_time;
    public int state = 0;   // 0: Boton no precionado, 1: Moviendo panel hacia abajo, 2: Panel detenido y fuera de vista

    // Update is called once per frame
    void Update()
    {
        if(state == 1)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - ( 20.0f * Mathf.Sqrt(Time.time - activate_time)), transform.position.z);
            if (transform.position.y < -1000)
            {
                state++;
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }

    private void OnMouseUp()
    {
        if (state == 0)
        {
            activate_time = Time.time;
            state++;
        }
    }
}
