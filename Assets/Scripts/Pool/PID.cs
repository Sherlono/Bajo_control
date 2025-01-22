using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PID : MonoBehaviour
{
    public hpool hp;
    public Setpointer sp;

    public Slider kp_slider, ti_slider, td_slider;

    [SerializeField]
    private float setpoint;
    [SerializeField]
    private float dh, p;
    public float h, u;
    private float _prev_error, _integral, _memory;
    private float y_offset;

    [SerializeField]
    private float factor = 1.54f;

    public List<int> splashlist;


    float Calculate()
    {
        float error = setpoint - h;

        _integral += error * Time.deltaTime;
        float kp = kp_slider.value * error;
        float ki = ti_slider.value * _integral;
        float kd = td_slider.value * (error - _prev_error) / Time.deltaTime;
        _prev_error = error;

        return kp + ki + kd;
    }

    float Integrator()
    {
        _memory += dh * Time.deltaTime;
        return _memory;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Inicializar valores PID
        setpoint = sp.value;
        y_offset = transform.localPosition.y;
        h = (transform.localPosition.y - y_offset) / factor;
        dh = 0;
        _prev_error = setpoint - h;
        _memory = h;
        // Otros
        splashlist = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        setpoint = sp.value;

        if (sp.state == 1)  // Si se dio el valor del setpoint correctamente
        {
            p = 0;
            if (splashlist.Count != 0)
            {
                for(int i = 0; i < splashlist.Count; i++)
                {
                    if (splashlist[i] != 0) // Si no se ha vaciado el splash
                    {
                        p += 6.5f;
                        splashlist[i] -= 1;
                    }
                    else
                    {
                        splashlist.Remove(i);
                    }
                }
            }
            h = (transform.localPosition.y - y_offset) / factor;    // Nivel sin el desplazamiento inicial en el eje y
            u = Calculate();    // Señal PID

            dh = hp.A * h + hp.B * u + p; // Salida dh

            float w_level = Integrator();       // Integrador para obtener h despues del actuamiento

            transform.localPosition = new Vector3(transform.localPosition.x, w_level + y_offset, transform.localPosition.z);
        }
    }
}
