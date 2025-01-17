using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using

public class Setpointer : MonoBehaviour
{
    public PlayPID PlayPID;
    public float value;
    private float min_position, max_position, max_value;
    private float last_time;
    public int state = 0;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        last_time = Time.time;
        min_position = transform.position.y;
        max_position = 239;
        max_value = 3;
    }
    private void Update()
    {
        if (PlayPID.state == 2 && Time.time - last_time > 0.5f)
        {
            if (!Input.GetMouseButton(0)) //
            {
                GetComponent<SpriteRenderer>().enabled = !(GetComponent<SpriteRenderer>().enabled);
                last_time = Time.time;
            }
        }
    }


    private void OnMouseUp()
    {
        if(PlayPID.state == 2)
        {
            state++;
            GetComponent<Collider2D>().enabled = false;
            this.enabled = false;
        }
    }

    private void OnMouseDrag()
    {
        if (state == 0 && PlayPID.state == 2)   // 
        {
            GetComponent<SpriteRenderer>().enabled = true;
            if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y < min_position)
            {
                transform.position = new Vector3(transform.position.x, min_position, transform.position.z);
                value = 0;
            }
            else if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y <= max_position)
            {
                transform.position = new Vector3(transform.position.x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, transform.position.z);
                value = (transform.position.y - min_position) * max_value / (max_position - min_position);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, max_position, transform.position.z);
                value = max_value;
            }
        }
    }
}
