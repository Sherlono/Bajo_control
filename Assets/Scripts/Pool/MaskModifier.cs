using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskModifier : MonoBehaviour
{
    public GameObject highPoint;
    public GameObject Mask;
    public float _x, _target_x, _y, _target_y;
    public float _m, _a;

    // Start is called before the first frame update
    void Start()
    {

        _x = Mask.transform.position.x; // Initial x position
        _target_x = highPoint.GetComponent<Transform>().position.x;
        _y = transform.position.y; // Initial y position
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
    }
}
