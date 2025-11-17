using UnityEngine;

public class Scorboy_Claw : MonoBehaviour
{
    [Header("Hinges")]
    public SliderJoint2D claw_left, claw_right;

    private float clawSpeed = 10.0f;
    private bool IsOpen = true;
    private bool enable = true;

    // Functionality
    public void Toggle()
    {
        if (IsOpen) Close();
        else Open();
    }

    private void Clamp()
    {
        JointTranslationLimits2D left_limits = claw_left.limits;
        JointTranslationLimits2D right_limits = claw_right.limits;

        float distance = Vector2.Distance(claw_left.transform.position, claw_right.transform.position);
        float new_limit = 0.01f + (2.34f - distance) / 2.0f;    // 2.34f is the current distance between the homes of each claw part and it might be subject to changes in the future

        left_limits.max = new_limit;
        claw_left.limits = left_limits;

        right_limits.max = new_limit;
        claw_right.limits = right_limits;
        IsOpen = false;
        enable = true;
    }

    private void Relax()
    {
        JointMotor2D motor_left = claw_left.motor;
        JointMotor2D motor_right = claw_right.motor;
        motor_left.motorSpeed = -clawSpeed / 5;
        motor_right.motorSpeed = -clawSpeed / 5;
        claw_left.motor = motor_left;
        claw_right.motor = motor_right;
        IsOpen = true;
        enable = true;
    }

    private void Open()
    {
        if (enable)
        {
            //Debug.Log("Opening");
            // Reset Limits
            JointTranslationLimits2D left_limits = claw_left.limits;
            JointTranslationLimits2D right_limits = claw_right.limits;

            left_limits.max = 1.0f;
            claw_left.limits = left_limits;

            right_limits.max = 1.0f;
            claw_right.limits = right_limits;

            // Activate motors
            JointMotor2D motor_left = claw_left.motor;
            JointMotor2D motor_right = claw_right.motor;

            motor_left.motorSpeed = -clawSpeed;
            motor_right.motorSpeed = -clawSpeed;

            claw_left.motor = motor_left;
            claw_right.motor = motor_right;

            // Post opening actions
            enable = false;
            Invoke("Relax", 0.4f);
        }
    }

    private void Close()
    {
        if (enable)
        {
            //Debug.Log("Closing");
            JointMotor2D motor_left = claw_left.motor;
            JointMotor2D motor_right = claw_right.motor;
            motor_left.motorSpeed = clawSpeed;
            motor_right.motorSpeed = clawSpeed;
            claw_left.motor = motor_left;
            claw_right.motor = motor_right;
            enable = false;
            Invoke("Clamp", 0.3f);
        }
    }

    // Getters
    public bool Is_Open()
    {
        return IsOpen;
    }

    private void Start()
    {
        Open();
    }
}
