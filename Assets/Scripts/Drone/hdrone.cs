using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class hdrone : MonoBehaviour
{
    [HideInInspector] public Vector2 targetpoint;

    // Setpoint
    public float x_des, y_des;
    const float vx_des = 0.0f, vy_des = 0.0f, ax_des = 0.0f, ay_des = 0.0f;

    public float x_wind, y_wind;

    [Header("Ganancias PID")]
    public float Kp_x;
    public float Kv_x;
    public float Kp_y;
    public float Kv_y;
    public float Kp_phi;
    public float Kv_phi;

    [Header("Fuerza")]
    [SerializeField] private float _maxMotor;
    private float _F_clamped, _M_clamped;

    public float battery;
    public const float NORMALEFFICIENCY = 0.977f;
    private float _batteryEfficiency = NORMALEFFICIENCY;
    public float Efficiency { set { _batteryEfficiency = value;} }

    [SerializeField] private Rigidbody2D rb2d;
    private float phi_c, F, M;
    const float Ixx = 0.00025f; // Mass moment of inertia (kg*m^2)
    const float L = 0.086f;     // Arm length (m)
    private bool _power;
    public bool Power { get { return _power; } set { _power = value; } }

    void Controller()
    {
        phi_c = (-1 / 9.81f) * (ax_des + Kv_x * (vx_des - rb2d.linearVelocity.x) + Kp_x * (x_des - rb2d.position.x));               // Phi
        F     = 100 * rb2d.mass * (9.81f + ay_des + Kv_y * (vy_des - rb2d.linearVelocity.y) + Kp_y * (y_des - rb2d.position.y));    // Magnitud fuerza Neta
        M     = Ixx * (Kv_phi * (- rb2d.angularVelocity) + Kp_phi * (phi_c - rb2d.rotation));                                       // Momento
    }

    void Clamp() {
        float u_left = 0.5f * (F - (M / L));
        float u_right = 0.5f * (F + (M / L));

        float u_left_clamped = Mathf.Min(Mathf.Max(0, u_left), _maxMotor);
        float u_right_clamped = Mathf.Min(Mathf.Max(0, u_right), _maxMotor);

        _F_clamped = u_left_clamped + u_right_clamped;
        _M_clamped = (u_right_clamped - u_left_clamped) * L;
    }

    void Apply_Forces()
    {
        Controller();
        Clamp();

        rb2d.AddRelativeForce(new Vector2(0, _F_clamped));
        rb2d.AddTorque(100 * rb2d.mass * _M_clamped / Ixx);

        rb2d.AddForce(new Vector2(x_wind, y_wind));
    }

    private void Awake()
    {
        Hazard.onEnter += delegate { Power = false; };
        Obstacle.onCollide += delegate { Power = false; };
    }

    private void OnDestroy()
    {
        Hazard.onEnter -= delegate { Power = false; };
        Obstacle.onCollide -= delegate { Power = false; };
    }

    void FixedUpdate()
    {
        if (battery <= 0) _power = false;
        if (_power == true)
        {
            x_des = targetpoint.x;
            y_des = targetpoint.y;

            Apply_Forces();

            battery -= (_F_clamped + _M_clamped) * (1 - _batteryEfficiency);
        }
    }

}
