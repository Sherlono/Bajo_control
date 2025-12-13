using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jv;

public class Pool : MonoBehaviour
{
    public PID controller;

    [HideInInspector] public Setpointer setPoint;
    [HideInInspector] public Slider kp_slider, ti_slider, td_slider;

    [Header("Misc")]
    private StateSpace _state_space;
    [SerializeField] private bool _paused;
    private float _factor;   // Transformada
    public bool enable;

    public List<int> splashlist = new();

    private const float y_offset = -3, stage1 = 1.5f, stage2 = 3.0f;

    public void Pause(bool p) { _paused = p;}

    public bool IsPaused() { return _paused; }

    private void UpdateSpaceState()
    {
        if (controller.h < 0)
        {
            _state_space.A = 0; _state_space.B = 0.0316f; _state_space.E = 1;
        }
        else if (controller.h < stage1)   // Mitad inferior
        {
            _state_space.A = -0.1171f; _state_space.B = 0.0316f; _state_space.E = 1;
        }
        else if (controller.h < stage2)   // Mitad superior
        {
            _state_space.A = -0.0313f; _state_space.B = 0.02f; _state_space.E = 1;
        }
        else // Sobre limite
        {
            Pause(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _state_space = new StateSpace(0f, 0.0316f, 1f);
        _paused = false;
        controller.setpoint = setPoint.value;
        _factor = 3 / 4.59f;
        controller.h = (transform.localPosition.y - y_offset) / _factor;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_paused)
        {
            controller.kp_gain = kp_slider.value;
            controller.ki_gain = ti_slider.value;
            controller.kd_gain = td_slider.value;

            controller.setpoint = setPoint.value;


            if (setPoint.state == 1)  // Si se dio el valor del setpoint correctamente
            {
                UpdateSpaceState();

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

                if(enable)
                {
                    controller.h = (transform.localPosition.y - y_offset) * _factor;    // Nivel sin el desplazamiento inicial en el eje y
                    controller.u = controller.Calculate();    // Señal PID

                    controller.dh = _state_space.Solve_Next_State(controller.h, controller.u, controller.p); 

                    float w_level = controller.Integrator();       // Integrador para obtener h despues del actuamiento

                    transform.localPosition = new Vector3(transform.localPosition.x, w_level + y_offset, transform.localPosition.z);
                }
            }
        }
    }
}
