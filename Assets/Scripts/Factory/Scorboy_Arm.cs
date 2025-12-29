using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class Scorboy_Arm : MonoBehaviour
{
    public const int JOINTS_COUNT = 3;
    public static int max_states;

    [System.Serializable] public struct ArmPose
    {
        public ArmPose(float a_1, float a_2, float a_3, bool isopen)
        {
            angles = new float[3];
            angles[0] = a_1;
            angles[1] = a_2;
            angles[2] = a_3;
            claw_open = isopen;
        }

        public float[] angles;
        public bool claw_open;
    }

    [Header("Hinges")]
    public HingeJoint2D[] joints = new HingeJoint2D[JOINTS_COUNT];

    [Header("Claw script")]
    public Scorboy_Claw Claw;

    [HideInInspector] public SpriteRenderer LED_sr;

    public List<ArmPose> State_List = new();
    public int state_index = 0;

    public void Create_Pose()
    {
        if(State_List.Count < max_states) State_List.Add(new ArmPose(Angle(0), Angle(1), Angle(2), Claw.Is_Open()));
    }

    // Functionality
    public void Joint_Set(int joint_index, float value)
    {
        JointMotor2D hingeMotor = joints[joint_index].motor;
        hingeMotor.motorSpeed = value;
        joints[joint_index].motor = hingeMotor;
    }

    public void Freeze()
    {
        for (int joint_index = 0; joint_index < 3; joint_index++) Joint_Set(joint_index, 0);
    }

    public void LED_ON()
    {
        LED_sr.color = Color.green;
    }
    public void LED_OFF()
    {
        LED_sr.color = Color.red;
    }

    // Getters
    public float Angle(int joint_index)
    {
        return joints[joint_index].jointAngle;
    }
    public ArmPose Target_Pose()
    {
        return State_List[state_index];
    }

}