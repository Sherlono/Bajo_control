using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hdrone : MonoBehaviour
{
    //public PID script;
    private Rigidbody2D rb2d;

    public GameObject point1, point2;

    private float y_des, z_des, vy_des, vz_des, ay_des, az_des;
    public float phi_c, F, M;
    private float Ixx = 0.00025f;   // Mass moment of inertia (kg*m^2)
    private float L = 0.086f;       // Arm length (m)
    public float max_motor;         // 1.7658
    public float Kp_y, Kv_y, Kp_z, Kv_z, Kp_phi, Kv_phi;

    [SerializeField]
    private float F_clamped, M_clamped;

    void Trajectory(){
        if (Time.realtimeSinceStartup < 30.0f){
            y_des = point1.transform.position.x;
            z_des = point1.transform.position.y;
        }
        else
        {
            y_des = point2.transform.position.x;
            z_des = point2.transform.position.y;
        }

        vy_des = 0.0f;
        vz_des = 0.0f;
        ay_des = 0.0f;
        az_des = 0.0f;
    }

    void Controller()
    {
        phi_c = (1 / Physics.gravity.y) * (ay_des + Kv_y * (vy_des - rb2d.velocity.x) + Kp_y * (y_des - rb2d.position.x));          // Phi
        F     = rb2d.mass * (-Physics.gravity.y + az_des + Kv_z * (vz_des - rb2d.velocity.y) + Kp_z * (z_des - rb2d.position.y));   // Magnitud fuerza Neta
        M     = Ixx * (Kv_phi * (- rb2d.angularVelocity) + Kp_phi * (phi_c - rb2d.rotation + (360 * Mathf.RoundToInt(rb2d.rotation / 360))));           // Momento
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
                                      // Fuerza x,              Fuerza y
        rb2d.AddRelativeForce(new Vector2(0, F_clamped));
        rb2d.AddTorque(M_clamped / L);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        //rb2d.inertia = Ixx;
        Kp_y = 0.4f;
        Kv_y = 1.0f;
        Kp_z = 0.4f;
        Kv_z = 1.0f;
        Kp_phi = 18.0f;
        Kv_phi = 15.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        xdot();
    }
}
