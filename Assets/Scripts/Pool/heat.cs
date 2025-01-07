using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class heat : MonoBehaviour
{
    public PID script;
    public float power;

    private Vector3 initial_pos;

    // Start is called before the first frame update
    void Start()
    {
        initial_pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        power = script.u;
        float red_level = 0.4666667f + power * 0.005333333f;   // 1/x = 1-0.46666667
        float green_level = 0.7019608f - power * 0.007019608f;
        float blue_level = 0.9921569f - power * 0.009921569f;

        GetComponent<Image>().color = new Color(red_level, green_level, blue_level, 1f);
    }
}
