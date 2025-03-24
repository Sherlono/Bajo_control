using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hdrone : MonoBehaviour
{
    [HideInInspector]
    public Vector2 targetpoint;
    [HideInInspector]
    public float x_des, y_des, vx_des, vy_des, ax_des, ay_des;  // Setpoint
    public float x_wind, y_wind;

    [Header("Ganancias PID")]
    public float Kp_x;
    public float Kv_x;
    public float Kp_y;
    public float Kv_y;
    public float Kp_phi;
    public float Kv_phi;

    [Header("Fuerza")]
    [SerializeField]
    private float max_motor;         // 1.7658
    [SerializeField]
    private float F_clamped, M_clamped;

    private Rigidbody2D rb2d;
    private float phi_c, F, M;
    private float Ixx = 0.00025f;   // Mass moment of inertia (kg*m^2) 0.00025f
    private float L = 0.086f;       // Arm length (m)
    private bool _power;

    void Trajectory(){
        if (Time.realtimeSinceStartup < 30.0f){
            x_des = targetpoint.x;    // [m]
            y_des = targetpoint.y;    // [m]
        }
        else
        {
            x_des = targetpoint.x;    // [m]
            y_des = targetpoint.y;    // [m]
        }

        vx_des = 0.0f;
        vy_des = 0.0f;
        ax_des = 0.0f;
        ay_des = 0.0f;
    }

    void Controller()
    {
        phi_c = (-1 / 9.81f) * (ax_des + Kv_x * (vx_des - rb2d.velocity.x) + Kp_x * (x_des - rb2d.position.x));             // Phi
        F     = 100 * rb2d.mass * (9.81f + ay_des + Kv_y * (vy_des - rb2d.velocity.y) + Kp_y * (y_des - rb2d.position.y));  // Magnitud fuerza Neta
        M     = Ixx * (Kv_phi * (- rb2d.angularVelocity) + Kp_phi * (phi_c - rb2d.rotation));                               // Momento
    }


    void Clamp() {
        float u1 = 0.5f * (F - (M / L));
        float u2 = 0.5f * (F + (M / L));

        float u1_clamped = Mathf.Min(Mathf.Max(0, u1), max_motor);
        float u2_clamped = Mathf.Min(Mathf.Max(0, u2), max_motor);
        F_clamped = u1_clamped + u2_clamped;
        M_clamped = (u2_clamped - u1_clamped) * L;
    }

    void xdot(){
        Trajectory();
        Controller();
        Clamp();
        
        rb2d.AddRelativeForce(new Vector2(0, F_clamped));
        rb2d.AddTorque(100 * rb2d.mass * M_clamped / Ixx);

        rb2d.AddForce(new Vector2(x_wind, y_wind));
    }

    public void Power(bool on)
    {
        _power = on;
    }

    public bool IsPowered() { return _power; }

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_power == true)
        {
            xdot();
        }
    }

}
