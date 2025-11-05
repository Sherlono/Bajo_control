using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jv;

public class Pool : MonoBehaviour
{
    public PID controller;

    [Header("Objetos")]
    public hpool hp;
    public Setpointer sp;

    public Slider kp_slider, ti_slider, td_slider;

    [Header("Parametros Motor")]
    [SerializeField] private float maxPump;
    [SerializeField] private float minPump;

    
    [Header("Misc")]
    [SerializeField]
    private bool _paused;
    private float factor = 1.54f;   // Comentar de donde viene
    private float y_offset;

    public List<int> splashlist = new();

    public void Pause(bool p) { _paused = p;}

    public bool IsPaused() { return _paused; }

    // Start is called before the first frame update
    void Start()
    {
        // Inicializar valores PID
        _paused = false;
        controller.setpoint = sp.value;
        y_offset = transform.localPosition.y;
        controller.h = (transform.localPosition.y - y_offset) / factor;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_paused)
        {
            controller.kp_gain = kp_slider.value;
            controller.ki_gain = ti_slider.value;
            controller.kd_gain = td_slider.value;

            controller.setpoint = sp.value;

            if (sp.state == 1)  // Si se dio el valor del setpoint correctamente
            {
                controller.p = 0;
                if (splashlist.Count != 0)
                {
                    for (int i = 0; i < splashlist.Count; i++)
                    {
                        if (splashlist[i] != 0) // Si no se ha vaciado el splash
                        {
                            if (splashlist[i] > 0)
                            {
                                controller.p += 6.5f;
                                splashlist[i] -= 1;
                            }
                            else
                            {
                                controller.p -= 1.625f;
                                splashlist[i] += 1;
                            }
                        }
                        else
                        {
                            splashlist.Remove(i);
                        }
                    }
                }

                controller.h = (transform.localPosition.y - y_offset) / factor;    // Nivel sin el desplazamiento inicial en el eje y
                controller.u = controller.Calculate();    // Señal PID

                controller.dh = hp.A * controller.h + hp.B * controller.u + controller.p; // Salida dh

                float w_level = controller.Integrator();       // Integrador para obtener h despues del actuamiento

                transform.localPosition = new Vector3(transform.localPosition.x, w_level + y_offset, transform.localPosition.z);
            }
        }
    }
}
