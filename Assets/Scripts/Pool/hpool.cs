using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hpool : MonoBehaviour
{
    public PID pool;
    public float A, B;
    private float stage1, stage2;

    // Start is called before the first frame update
    void Start()
    {
        A = -0.1171f; B = 0.0316f;
        stage1 = 1.5f;
        stage2 = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (pool.h < 0)
        {
            A = 0; B = 0.0316f;
        }
        else if (pool.h < stage1)
        {
            A = -0.1171f; B = 0.0316f;
        }
        else if (pool.h < stage2)
        {
            A = -0.0313f; B = 0.02f;
        }
        else
        {
            A = -0.0313f; B = 0;
        }

    }
}
