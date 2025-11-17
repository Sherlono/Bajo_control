using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Cloud : MonoBehaviour
{
    public GameObject start_obj;
    public GameObject end_obj;
    private float x_start, x_end, y_start;
    private float speed = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        x_start = start_obj.transform.position.x;
        x_end = end_obj.transform.position.x;
        y_start = start_obj.transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
        if (transform.position.x > x_end)
        {
            float y_target = Random.Range(y_start - 180.0f, y_start + 200.0f);
            transform.position = new Vector3(x_start, y_target, transform.position.z);
        }
    }
}
