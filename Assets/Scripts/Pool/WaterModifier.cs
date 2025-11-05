using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterModifier : MonoBehaviour
{
    private Pool pool;
    private GameObject highPoint;
    private GameObject Mask;
    private GameObject Overflow;
    private GameObject Surface;
    private float _x, _target_x, _y, _target_y, _of_y;
    private float _m, _a;

    // Start is called before the first frame update
    void Start()
    {
        Surface = GameObject.Find("Water_Surface");
        pool = Surface.GetComponent<Pool>();
        highPoint = GameObject.Find("1.5m mark");
        Mask = GameObject.Find("SurfaceMask");

        Overflow = GameObject.Find("WaterOverflow");
        _of_y = Overflow.transform.position.y;

        _x = Mask.transform.position.x; // Initial x position
        _target_x = highPoint.GetComponent<Transform>().position.x;
        _y = transform.position.y;      // Initial y position
        _target_y = highPoint.GetComponent<Transform>().position.y;

        _m =  (_y - _target_y) / (_x - _target_x);
        _a = _y - _x * _m;
    }

    // Update is called once per frame
    void Update()
    {
        float diff = transform.position.y - _y;
        if (diff < _target_y - _y)
        {
            Mask.transform.position = new Vector3((transform.position.y - _a) / _m, transform.position.y, Mask.transform.position.z);
        }
        else
        {
            Mask.transform.position = new Vector3(9999.0f, transform.position.y, Mask.transform.position.z);
        }

        if (pool.controller.h >= 3.0f)
        {
            Overflow.transform.position = new Vector3(Overflow.transform.position.x, 240f, Overflow.transform.position.z);
            Color tmp = Surface.GetComponent<SpriteRenderer>().color;
            tmp.a = 0f;
            Surface.GetComponent<SpriteRenderer>().color = tmp;
        }
        else
        {
            Overflow.transform.position = new Vector3(Overflow.transform.position.x, _of_y, Overflow.transform.position.z);
            Color tmp = Surface.GetComponent<SpriteRenderer>().color;
            tmp.a = 0.5f;
            Surface.GetComponent<SpriteRenderer>().color = tmp;
        }
    }
}
