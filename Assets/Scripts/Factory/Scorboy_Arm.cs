using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class Scorboy_Arm : MonoBehaviour
{
    [Header("Claw script")]
    public Scorboy_Claw Claw;

    [Header("Hinges")]
    public HingeJoint2D hj2d_1;
    public HingeJoint2D hj2d_2;

    [Header("LED Sprite Renderer")]
    public SpriteRenderer LED_sr;

    [System.Serializable] public struct ArmState
    {
        public ArmState(float a_1, float a_2, float a_3, bool isopen)
        {
            angle_1 = a_1;
            angle_2 = a_2;
            angle_3 = a_3;
            claw_open = isopen;
        }

        public float angle_1, angle_2, angle_3;
        public bool claw_open;
    }

    [SerializeField] private List<ArmState> State_List = new();

    public void Create_State()
    {
        //Scorboy_Claw Claw
        if(State_List.Count < 8) State_List.Add(new ArmState(Angle_1(), Angle_2(), Claw.Angle(), Claw.Is_Open()));
    }

    // Functionality
    public void Joint_1_Set(int value)
    {
        JointMotor2D hingeMotor = hj2d_1.motor;
        hingeMotor.motorSpeed = value;
        hj2d_1.motor = hingeMotor;
    }
    public void Joint_2_Set(int value)
    {
        JointMotor2D hingeMotor = hj2d_2.motor;
        hingeMotor.motorSpeed = value;
        hj2d_2.motor = hingeMotor;
    }

    // Getters
    public float Angle_1()
    {
        return hj2d_1.jointAngle;
    }
    public float Angle_2()
    {
        return hj2d_2.jointAngle;
    }

    public void LED_ON()
    {
        LED_sr.color = Color.green;
    }
    public void LED_OFF()
    {
        LED_sr.color = Color.red;
    }
}