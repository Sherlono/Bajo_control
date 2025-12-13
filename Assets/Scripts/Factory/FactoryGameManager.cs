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

    public static event Action LoseAction;

    [System.Serializable] public enum States
    {
        Config_Arms,
        Run_Arms,
        Run_Machines,
        End,
    }

    [Header("Objects")]
    [SerializeField] private List<GameObject> _scorboy_Objects = new();
    public Scorboy_Controller scorboy_controller;

    [SerializeField] private Factory_Machine[] _machine_Objects;
    [SerializeField] private Conveyor_Belt _conveyor;
    [SerializeField] private Factory_Block Block;

    [Header("PID Stuff")]
    public float max_output;
    public float kp, ki, kd;
    public PID[] _pid_controllers;

    [Header("Camera Stuff")]
    public Camera ui_camera;
    const float cam_offset = 2.7f;

    [Header("Botones")]
    [HideInInspector] public Button next_arm_btn;
    [HideInInspector] public Button save_state_btn;
    [HideInInspector] public Button reset_block_btn;

    [Header("Data")]
    public int scorboy_max;
    private int _scorboy_index = 0;
    public float error_threshold;
    private States state = States.Config_Arms;

    private void Create_State()
    {
        if(state == States.Config_Arms) _scorboy_Objects[_scorboy_index].GetComponent<Scorboy_Arm>().Create_State();
    }
    private void Go2State()
    {
        for(int joint_index = 0; joint_index < Scorboy_Arm.JOINTS_COUNT; joint_index++){
            _pid_controllers[joint_index].h = scorboy_controller.Arm.Angle(joint_index);    // Feed current state to PID
            _pid_controllers[joint_index].u = _pid_controllers[joint_index].Calculate();    // Calculate output

            scorboy_controller.Arm.Joint_Set(joint_index, _pid_controllers[joint_index].u); // Actuate joints
        }

        if (scorboy_controller.Claw.Is_Open() != scorboy_controller.Arm.State_List[scorboy_controller.Arm.state_index].claw_open) scorboy_controller.Claw.Toggle();     // Actuate Claw
    }
    private bool State_Is_Reached()
    {
        bool angle_reached = true;
        float error;

        for (int joint_index = 0; joint_index < Scorboy_Arm.JOINTS_COUNT; joint_index++)
        {
            error = scorboy_controller.Arm.Target_State().angles[joint_index] - scorboy_controller.Arm.Angle(joint_index);
            angle_reached = angle_reached && (Mathf.Abs(error) <= error_threshold);
        }

        bool open_close_reached = scorboy_controller.Claw.Is_Open() == scorboy_controller.Arm.Target_State().claw_open;
        return angle_reached && open_close_reached;
    }

    private void Next_Arm_btn()
    {
        if (state == States.Config_Arms && scorboy_controller.Arm.State_List.Count != 0)
        {
            if (!Is_Last_Scorboy()) // Setup stage
            {
                scorboy_controller.Arm.LED_OFF();
                scorboy_controller.Freeze_Scorboy();

                _machine_Objects[_scorboy_index].Set_Highlight(false);
                _machine_Objects[_scorboy_index].Transform_Block();

                _scorboy_index++;
                scorboy_controller.Set_Scorboy(_scorboy_Objects[_scorboy_index]);
                if (_scorboy_index < _machine_Objects.Length) _machine_Objects[_scorboy_index].Set_Highlight(true);
                if (_scorboy_index == _scorboy_Objects.Count - 1) _conveyor.Set_Highlight(true);

                scorboy_controller.Arm.LED_ON();
            }
            else    // Scorboys Autopilot begins
            {
                scorboy_controller.Arm.LED_OFF();
                scorboy_controller.Freeze_Scorboy();

                if(_scorboy_index < _machine_Objects.Length) _machine_Objects[_scorboy_index].Set_Highlight(false);
                _conveyor.Set_Highlight(false);

                _scorboy_index = 0;
                scorboy_controller.Set_Scorboy(_scorboy_Objects[_scorboy_index]);

                scorboy_controller.Arm.LED_ON();

                for (int joint_index = 0; joint_index < Scorboy_Arm.JOINTS_COUNT; joint_index++) _pid_controllers[joint_index].setpoint = scorboy_controller.Arm.Target_State().angles[joint_index];

                Block.Reset_Block();
                    
                scorboy_controller.Toggle_Interactable();
                next_arm_btn.interactable = false;
                save_state_btn.interactable = false;
                reset_block_btn.interactable = false;

                state = States.Run_Arms;
            }
        }
    }
    
    private void Move_Camera_to_Target(Vector2 target)
    {
        Vector2 target_lerp = Vector2.Lerp(ui_camera.transform.position, target, 0.01f);
        ui_camera.transform.position = new Vector3(target_lerp.x, target_lerp.y, ui_camera.transform.position.z);
    }

    private bool Is_Last_State()
    {
        return scorboy_controller.Arm.state_index >= scorboy_controller.Arm.State_List.Count - 1;
    }
    private bool Is_Last_Scorboy()
    {
        return _scorboy_index >= scorboy_max - 1;
    }

    private void Awake()
    {
        instance = this;

        Scorboy_Arm.max_states = 15;

        /*for (int i = 0; i < scorboy_max; i++)
        {
            GameObject newScorboy = Instantiate(Scorboy_Prefab,
                                                new Vector3(-7.0f + i*7.0f, -3.0f, -2),
                                                Quaternion.identity);

            newScorboy.tag = "Controlable";
            Scorboy_Arm Arm = newScorboy.GetComponent<Scorboy_Arm>();
            if (i == 0) Arm.LED_ON();

            _scorboy_Objects.Add(newScorboy);
        }*/

        ui_camera.transform.position = new Vector3(_scorboy_Objects[_scorboy_index].transform.position.x + cam_offset, ui_camera.transform.position.y, ui_camera.transform.position.z);
    }
    private void Start()
    {
        _machine_Objects[0].Set_Highlight(true);

        foreach (PID pid_controller in _pid_controllers)
        {
            pid_controller.max_output = max_output;
            pid_controller.min_output = -max_output;
            pid_controller.kp_gain = kp;
            pid_controller.ki_gain = ki;
            pid_controller.kd_gain = kd;
        }

        next_arm_btn.onClick.AddListener(Next_Arm_btn);
        save_state_btn.onClick.AddListener(Create_State);
        reset_block_btn.onClick.AddListener(delegate { Block.Load_Saved_Block(); });
    }
    private void OnDestroy()
    {
        instance = null;

        next_arm_btn.onClick.RemoveListener(Next_Arm_btn);
        save_state_btn.onClick.RemoveListener(Create_State);
        reset_block_btn.onClick.RemoveListener(delegate { Block.Load_Saved_Block(); });
    }


    void FixedUpdate()
    {
        switch (state)
        {
            case States.Config_Arms:
                Move_Camera_to_Target(new Vector2(_scorboy_Objects[_scorboy_index].transform.GetChild(0).transform.position.x + cam_offset, ui_camera.transform.position.y));
                break;

            case States.Run_Arms:
                Go2State();

                if (State_Is_Reached())
                {
                    scorboy_controller.Freeze_Scorboy();
                    foreach (PID controller in _pid_controllers) controller.Reset_Memory();

                    if (!Is_Last_State())
                    {
                        scorboy_controller.Arm.state_index++;
                        for (int joint_index = 0; joint_index < Scorboy_Arm.JOINTS_COUNT; joint_index++) _pid_controllers[joint_index].setpoint = scorboy_controller.Arm.Target_State().angles[joint_index];
                    }
                    else if(!Is_Last_Scorboy())
                    {
                        scorboy_controller.Arm.LED_OFF();
                        scorboy_controller.Arm.state_index = 0;
                        _machine_Objects[_scorboy_index].enable = true;
                        state = States.Run_Machines;
                    }
                    else
                    {
                        _scorboy_index = 0;
                        scorboy_controller.Arm.state_index = 0;
                        state = States.End;

                        scorboy_controller.Arm.LED_OFF();

                        _conveyor.Set_Enable(true);
                    }
                }

                Move_Camera_to_Target(new Vector2(_scorboy_Objects[_scorboy_index].transform.GetChild(0).transform.position.x + cam_offset, ui_camera.transform.position.y));
                break;

            case States.Run_Machines:
                if (!_machine_Objects[_scorboy_index].Block_is_Inside())
                {
                    LoseAction?.Invoke();
                    state = States.End;
                }
                else if (_machine_Objects[_scorboy_index].done)
                {
                    _scorboy_index++;
                    scorboy_controller.Set_Scorboy(_scorboy_Objects[_scorboy_index]);
                    scorboy_controller.Arm.LED_ON();
                    for (int joint_index = 0; joint_index < Scorboy_Arm.JOINTS_COUNT; joint_index++) _pid_controllers[joint_index].setpoint = scorboy_controller.Arm.Target_State().angles[joint_index];
                    state = States.Run_Arms;
                }
                Move_Camera_to_Target(new Vector2(Block.transform.position.x + cam_offset, ui_camera.transform.position.y));
                break;

            case States.End:
                Move_Camera_to_Target(new Vector2(Block.transform.position.x + cam_offset, ui_camera.transform.position.y));
                break;
        }
    }
}
