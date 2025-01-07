using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskModifier : MonoBehaviour
{
    public GameObject Mask;
    public float _x, _target_x, _y, _target_y;
    public float _m, _a;

    // Start is called before the first frame update
    void Start()
    {
        _x = Mask.transform.position.x;
        _target_x = 626.06f * Screen.height / 1080;
        _y = transform.position.y;
        _target_y = 140.0f * Screen.width / 1920;
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
