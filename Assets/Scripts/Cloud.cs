using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Cloud : MonoBehaviour
{
    public GameObject start_obj;
    public GameObject end_obj;
    private float x_start, x_end, y_start;
    [SerializeField] private hdrone drone;
    private static float _speed = 0.01f;

    void Start()
    {
        x_start = start_obj.transform.position.x;
        x_end = end_obj.transform.position.x;
        y_start = start_obj.transform.position.y;
        if (drone) _speed = drone.x_wind / 5;
        else if (_speed != 0.01f) _speed = 0.01f;
    }

    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x + _speed, transform.position.y, transform.position.z);
        if (_speed > 0)
        {
            if (transform.position.x > x_end)
            {
                float y_target = Random.Range(y_start - 180.0f, y_start + 200.0f);
                transform.position = new Vector3(x_start, y_target, transform.position.z);
            }
        }
        else
        {
            if (transform.position.x < x_start)
            {
                float y_target = Random.Range(y_start - 180.0f, y_start + 200.0f);
                transform.position = new Vector3(x_end, y_target, transform.position.z);
            }
        }
    }
}
