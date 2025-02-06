using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour
{
    public GameObject bar;
    public float value;
    [SerializeField]
    private float max_value;
    private float min_position, max_position;

    // Start is called before the first frame update
    void Start()
    {
        min_position = bar.GetComponent<Transform>().position.x - (bar.GetComponent<SpriteRenderer>().bounds.size.x / 2);
        max_position = bar.GetComponent<Transform>().position.x + (bar.GetComponent<SpriteRenderer>().bounds.size.x / 2);
        transform.position = new Vector3(min_position, bar.GetComponent<Transform>().position.y, transform.position.z);
    }

    private void Update()
    {
        min_position = bar.GetComponent<Transform>().position.x - (bar.GetComponent<SpriteRenderer>().bounds.size.x / 2);
        max_position = bar.GetComponent<Transform>().position.x + (bar.GetComponent<SpriteRenderer>().bounds.size.x / 2);
    }

    public void Reset()
    {
        value = 0;
        transform.position = new Vector3(min_position, bar.GetComponent<Transform>().position.y, transform.position.z);
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