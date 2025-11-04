using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*namespace jv
{
    public class PID_controller
    {

    }
}*/

public class PID : MonoBehaviour
{
    public hpool hp;
    public Setpointer sp;

    public Slider kp_slider, ti_slider, td_slider;

    
    [Header("Variables sistema")]
    [SerializeField]
    private float setpoint;
    public float h;
    public float u;
    private float dh, p;

    private float _prev_error, _integral, _memory;

    [Header("Parametros Motor")]
    [SerializeField]
    private float maxPump;
    [SerializeField]
    private float minPump;

    
    [Header("Misc")]
    [SerializeField]
    private bool _paused;
    private float factor = 1.54f;   // Comentar de donde viene
    private float y_offset;

    public List<int> splashlist = new();

    public void Pause(bool p){ _paused = p;}

    public bool IsPaused() { return _paused; }

    float Clamp(float value)
    {
        value = value > maxPump ? maxPump : value;
        return value < minPump ? minPump : value;
    }


    float Calculate()
    {
        float error = setpoint - h;

        _integral += error * Time.fixedDeltaTime;

        float kp = kp_slider.value * error;
        float ki = ti_slider.value * _integral;
        float kd = td_slider.value * (error - _prev_error) / Time.fixedDeltaTime;

        _prev_error = error;

        return Clamp(kp + ki + kd);
    }

    float Integrator()
    {
        _memory += dh * Time.fixedDeltaTime;
        return _memory;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Inicializar valores PID
        _paused = false;
        setpoint = sp.value;
        y_offset = transform.localPosition.y;
        h = (transform.localPosition.y - y_offset) / factor;
        dh = 0;
        _prev_error = setpoint - h;
        _memory = h;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_paused)
        {
            setpoint = sp.value;

            if (sp.state == 1)  // Si se dio el valor del setpoint correctamente
            {
                p = 0;
                if (splashlist.Count != 0)
                {
                    for (int i = 0; i < splashlist.Count; i++)
                    {
                        if (splashlist[i] != 0) // Si no se ha vaciado el splash
                        {
                            if (splashlist[i] > 0)
                            {
                                p += 6.5f;
                                splashlist[i] -= 1;
                            }
                            else
                            {
                                p -= 1.625f;
                                splashlist[i] += 1;
                            }
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
}
