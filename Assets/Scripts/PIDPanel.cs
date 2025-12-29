using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIDPanel : MonoBehaviour
{
    public static PIDPanel instance;
    [SerializeField] private int state = 0;   // 0: Boton no precionado, 1: Moviendo panel hacia abajo, 2: Panel detenido y fuera de vista
    public static int GetState { get { return instance.state; } }

    public static event Action<int> onReady, onExitScreen;

    [SerializeField, HideInInspector] private Slider KpSlider, TiSlider, TdSlider;
    public static float Kp { get { return instance.KpSlider.value; } }
    public static float Ti { get { return instance.TiSlider.value; } }
    public static float Td { get { return instance.TdSlider.value; } }

    public float maxSliderValue;
    private int checkCount = 0;

    const float lerp_t = 0.005f;

    public static void Restart()
    {
        instance.state = 3;
        instance.KpSlider?.Reset();
        instance.TiSlider?.Reset();
        instance.TdSlider?.Reset();
    }

    public void Check()
    {
        if (state == 0)
        {
            onReady?.Invoke(checkCount);
            checkCount++;
            state++;
        }
    }

    private void Awake()
    {
        instance = this;
        if (instance != null && instance != this) Destroy(this);
    }

    void FixedUpdate()
    {
        if (state == 1)         // Moving out of screen
        {
            transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, -610, lerp_t), transform.localPosition.z);
            if (transform.localPosition.y <= -605)
            {
                state++;
                onExitScreen?.Invoke(checkCount - 1);
            }
        }
        else if (state == 3)    // Restart was called
        {
            transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, 0, lerp_t), transform.localPosition.z);
            if (transform.localPosition.y > -0.1f) state = 0;
        }
    }

}
