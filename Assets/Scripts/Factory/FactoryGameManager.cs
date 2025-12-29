using jv;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class FactoryGameManager : MonoBehaviour
{
    public static FactoryGameManager instance;

    public static event Action<int> onStartTask, onTaskTrigger;
    public static event Action<int, bool> onHighlight;

    public TaskAction ManagerTask;

    [System.Serializable] public enum States
    {
        Config_Arms,
        Arm_Auto,
        Machine_running,
        End,
    }

    [Header("Objects")]
    [SerializeField] private GameObject[] _scorboyObjects;
    //[SerializeField] private int[] _scorboySequence;

    public Scorboy_Controller scorboy_controller;

    [SerializeField] private Factory_Block _block;

    [Header("PID Stuff")]
    public float max_output;
    public float kp, ki, kd;
    public float error_threshold;
    public PID[] _pid_controllers;

    [Header("Camera Stuff")]
    public Camera ui_camera;
    private const float cam_offset = 2.7f;

    [Header("Botones")]
    [HideInInspector] public Button next_arm_btn;
    [HideInInspector] public Button save_state_btn;
    [HideInInspector] public Button reset_block_btn;

    [Header("Data")]
    private States state = States.Config_Arms;
    //public int scorboy_max;

    [SerializeField] private int _scorboy_index = 0;
    [SerializeField] private int _task_index = 0;
    [SerializeField] private int _loseTime = 0;

    private float _cam_target;

    private void Create_Pose()
    {
        if(state == States.Config_Arms) _scorboyObjects[_scorboy_index].GetComponent<Scorboy_Arm>().Create_Pose();
    }

    private void Go_2_Pose()
    {
        for(int joint_index = 0; joint_index < Scorboy_Arm.JOINTS_COUNT; joint_index++){
            _pid_controllers[joint_index].h = scorboy_controller.Arm.Angle(joint_index);    // Feed current state to PID
            _pid_controllers[joint_index].u = _pid_controllers[joint_index].Calculate();    // Calculate output

            scorboy_controller.Arm.Joint_Set(joint_index, _pid_controllers[joint_index].u); // Actuate joints
        }

        if (scorboy_controller.Claw.Is_Open() != scorboy_controller.Arm.State_List[scorboy_controller.Arm.state_index].claw_open) scorboy_controller.Claw.Toggle();     // Actuate Claw
    }

    private void Complete_Task(int id)
    {
        if (id == _task_index)
        {
            _task_index++;
            _scorboy_index++;

            scorboy_controller.Set_Scorboy(_scorboyObjects[_scorboy_index]);
            scorboy_controller.Arm.LED_ON();
            for (int joint_index = 0; joint_index < Scorboy_Arm.JOINTS_COUNT; joint_index++) _pid_controllers[joint_index].setpoint = scorboy_controller.Arm.Target_Pose().angles[joint_index];
            state = States.Arm_Auto;
        }
    }

    private void Next_Arm_btn()
    {
        if (state == States.Config_Arms && scorboy_controller.Arm.State_List.Count > 1)
        {
            if (!Is_Last_Scorboy()) // Setup stage
            {
                scorboy_controller.Arm.LED_OFF();
                scorboy_controller.Arm.Freeze();

                onHighlight?.Invoke(_task_index, false);
                onTaskTrigger?.Invoke(_task_index);

                _scorboy_index++;
                scorboy_controller.Set_Scorboy(_scorboyObjects[_scorboy_index]);

                _task_index++;
                onHighlight?.Invoke(_task_index, true);

                scorboy_controller.Arm.LED_ON();
            }
            else    // Scorboys Autopilot begins
            {
                scorboy_controller.Arm.LED_OFF();
                scorboy_controller.Arm.Freeze();

                onHighlight?.Invoke(_task_index, false);

                _scorboy_index = 0;
                _task_index = 0;
                scorboy_controller.Set_Scorboy(_scorboyObjects[_scorboy_index]);

                scorboy_controller.Arm.LED_ON();

                for (int joint_index = 0; joint_index < Scorboy_Arm.JOINTS_COUNT; joint_index++) _pid_controllers[joint_index].setpoint = scorboy_controller.Arm.Target_Pose().angles[joint_index];

                _block.Reset_Block();
                    
                scorboy_controller.Toggle_Interactable();
                next_arm_btn.interactable = false;
                save_state_btn.interactable = false;
                reset_block_btn.interactable = false;

                state = States.Arm_Auto;
            }
        }
    }
    
    private void Move_Camera_to_Target(Vector2 target)
    {
        Vector2 target_lerp = Vector2.Lerp(ui_camera.transform.position, target, 0.01f);
        ui_camera.transform.position = new Vector3(target_lerp.x, target_lerp.y, ui_camera.transform.position.z);
    }
    
    private void Next_Scorboy_Pose()
    {
        scorboy_controller.Arm.state_index++;
        for (int joint_index = 0; joint_index < Scorboy_Arm.JOINTS_COUNT; joint_index++) _pid_controllers[joint_index].setpoint = scorboy_controller.Arm.Target_Pose().angles[joint_index];
    }

    private bool Pose_Is_Reached()
    {
        bool angle_reached = true;
        float error;

        for (int joint_index = 0; joint_index < Scorboy_Arm.JOINTS_COUNT; joint_index++)
        {
            error = scorboy_controller.Arm.Target_Pose().angles[joint_index] - scorboy_controller.Arm.Angle(joint_index);
            angle_reached = angle_reached && (Mathf.Abs(error) <= error_threshold);
        }

        bool open_close_reached = scorboy_controller.Claw.Is_Open() == scorboy_controller.Arm.Target_Pose().claw_open;
        return angle_reached && open_close_reached;
    }
    private bool Is_Last_Pose()
    {
        return scorboy_controller.Arm.state_index >= scorboy_controller.Arm.State_List.Count - 1;
    }
    private bool Is_Last_Scorboy()
    {
        return _scorboy_index >= _scorboyObjects.Length - 1;
    }

    private void Awake()
    {
        instance = this;

        if (instance != null && instance != this) Destroy(this);

        next_arm_btn.onClick.AddListener(Next_Arm_btn);
        save_state_btn.onClick.AddListener(Create_Pose);

        TaskAction.onCompletion += Complete_Task;

        Scorboy_Arm.max_states = 10;

        ui_camera.transform.position = new Vector3(_scorboyObjects[0].transform.position.x + cam_offset, ui_camera.transform.position.y, ui_camera.transform.position.z);
    }
    private void OnDestroy()
    {
        next_arm_btn.onClick.RemoveListener(Next_Arm_btn);
        save_state_btn.onClick.RemoveListener(Create_Pose);
    }

    private void Start()
    {
        onHighlight?.Invoke(_task_index, true);

        foreach (PID pid_controller in _pid_controllers)
        {
            pid_controller.max_output = max_output;
            pid_controller.min_output = -max_output;
            pid_controller.kp_gain = kp;
            pid_controller.ki_gain = ki;
            pid_controller.kd_gain = kd;
        }

        for (int i = 0; i < _scorboyObjects.Length; i++)
        {
            Create_Pose();
            _scorboy_index++;
        }
        _scorboy_index = 0;
    }

    void FixedUpdate()
    {
        switch (state)
        {
            case States.Config_Arms:
                Move_Camera_to_Target(new Vector2(_scorboyObjects[_scorboy_index].transform.GetChild(0).transform.position.x + cam_offset, ui_camera.transform.position.y));
                break;

            case States.Arm_Auto:
                _loseTime++;
                if (_loseTime == 3900)
                {
                    ManagerTask.fail = true;
                    state = States.End;
                }
                Go_2_Pose();

                if (Pose_Is_Reached())
                {
                    _loseTime = 0;
                    scorboy_controller.Arm.Freeze();
                    foreach (PID controller in _pid_controllers) controller.Reset_Memory();

                    if (!Is_Last_Pose()) Next_Scorboy_Pose();
                    else if (!Is_Last_Scorboy())
                    {
                        scorboy_controller.Arm.LED_OFF();
                        scorboy_controller.Arm.state_index = 0;

                        onStartTask?.Invoke(_task_index);

                        state = States.Machine_running;
                    }
                    else
                    {
                        scorboy_controller.Arm.state_index = 0;
                        scorboy_controller.Arm.LED_OFF();
                        _scorboy_index = 0;

                        onStartTask?.Invoke(_task_index);

                        state = States.End;
                    }
                }

                _cam_target = _scorboyObjects[_scorboy_index].transform.GetChild(0).transform.position.x + cam_offset;
                break;

            case States.Machine_running:
                _cam_target = _block.transform.position.x + cam_offset;
                break;

            case States.End:
                _cam_target = _block.transform.position.x + cam_offset;
                break;
        }

        Move_Camera_to_Target(new Vector2(_cam_target, ui_camera.transform.position.y));
    }
}
