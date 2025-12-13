using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterModifier : MonoBehaviour
{
    private Pool pool;
    [SerializeField] private GameObject Mask;
    [SerializeField] private GameObject Overflow;
    private SpriteRenderer _spriteRenderer;

    private Vector2 halfway_mark;
    private float _init_y, _m, _a;

    private bool WaterOverflow()
    {
        return pool.controller.h >= 3.0f && pool.setPoint.state == 1;
    }

    void Start()
    {
        pool = GetComponent<Pool>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        halfway_mark = transform.GetChild(0).transform.position;

        float _init_x = Mask.transform.position.x;   // Initial x position

        _init_y = transform.position.y;              // Initial y position
        _m = (_init_y - halfway_mark.y) / (_init_x - halfway_mark.x);
        _a = _init_y - _init_x * _m;
    }

    void Update()
    {
        float water_level = transform.position.y - _init_y;
        if (water_level < halfway_mark.y - _init_y)
        {
            if (!Mask.activeSelf) Mask.SetActive(true);
            Mask.transform.position = new Vector3((transform.position.y - _a) / _m, transform.position.y, Mask.transform.position.z);
        }
        else
        {
            Mask.SetActive(false);
        }

        Overflow.SetActive(WaterOverflow());
        _spriteRenderer.enabled = !WaterOverflow();
    }
}
