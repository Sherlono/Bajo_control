using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour
{
    public GameObject circle;
    public GameObject bar;
    public float value;
    private float min_position, max_position, max_value;

    // Start is called before the first frame update
    void Start()
    {
        min_position = transform.position.x;
        max_position = min_position + bar.GetComponent<SpriteRenderer>().bounds.size.x;
        max_value = 50;
    }

    private void OnMouseDrag()
    {
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x < min_position)
        {
            transform.position = new Vector3(min_position, transform.position.y, transform.position.z);
            value = 0;
        }
        else if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x <= max_position)
        {
            transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, transform.position.y, transform.position.z);
            value = (transform.position.x - min_position) * max_value / (max_position - min_position);
        }
        else
        {
            transform.position = new Vector3(max_position, transform.position.y, transform.position.z);
            value = max_value;
        }
    }
}