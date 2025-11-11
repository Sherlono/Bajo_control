using jv;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class FactoryGameManager : MonoBehaviour
{
    [System.Serializable] public enum States
    {
        Start,
        Run,
        End,
    }

    [Header("Objects")]
    [HideInInspector]
    public Scorboy_Controller scorboy_controller;
    private List<GameObject> _scorboy_Objects = new();
    [HideInInspector]
    public Factory_Block Block;

    [Header("PID Stuff")]
    [SerializeField] private PID[] _pid_controllers;
    public float max_output;
    public float kp;
    public float ki;
    public float kd;

    [Header("Camera Stuff")]
    public Camera ui_camera;
    public float cam_offset;

    [Header("Botones")]
    [HideInInspector]
    public Button next_arm_btn;
    [HideInInspector]
    public Button save_state_btn;

    [Header("Data")]
    [SerializeField] private States state = States.Start;
    public int scorboy_max;
    private int _scorboy_index = 0;
    public float state_list_count;
    public float error_threshold;

    private void Create_State()
    {
        if(state == States.Start) _scorboy_Objects[_scorboy_index].GetComponent<Scorboy_Arm>().Create_State();
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
        for (int joint_index = 0; joint_index < Scorboy_Arm.JOINTS_COUNT; joint_index++)
        {
            angle_reached = angle_reached && (Mathf.Abs(scorboy_controller.Arm.Target_State().angles[joint_index] - scorboy_controller.Arm.Angle(joint_index)) <= error_threshold);
        }

        bool open_close_reached = scorboy_controller.Claw.Is_Open() == scorboy_controller.Arm.Target_State().claw_open;
        return angle_reached && open_close_reached;
    }

    private void Next_Arm_btn()
    {
        if (state == States.Start)
        {
            if ( scorboy_controller.Arm.State_List.Count != 0) 
            {
                if (_scorboy_index < scorboy_max - 1)    // Not the last scorboy
                {
                    scorboy_controller.Arm.LED_OFF();
                    scorboy_controller.Freeze_Scorboy();

                    _scorboy_index++;
                    scorboy_controller.Set_Scorboy(_scorboy_Objects[_scorboy_index]);

                    scorboy_controller.Arm.LED_ON();
                }
                else    // Is the last scorboy
                {
                    scorboy_controller.Arm.LED_OFF();
                    _scorboy_index = 0;
                    scorboy_controller.Set_Scorboy(_scorboy_Objects[_scorboy_index]);

                    scorboy_controller.Arm.LED_ON();

                    for (int joint_index = 0; joint_index < Scorboy_Arm.JOINTS_COUNT; joint_index++) _pid_controllers[joint_index].setpoint = scorboy_controller.Arm.Target_State().angles[joint_index];

                    state++;
                    
                    Block.Reset_Pos();
                }
            }
        }
        else
        {
            scorboy_controller.Arm.LED_OFF();
            _scorboy_index = 0;
            scorboy_controller.Set_Scorboy(_scorboy_Objects[_scorboy_index]);

            scorboy_controller.Arm.LED_ON();
            scorboy_controller.Arm.state_index = 0;

            for (int joint_index = 0; joint_index < Scorboy_Arm.JOINTS_COUNT; joint_index++) _pid_controllers[joint_index].setpoint = scorboy_controller.Arm.Target_State().angles[joint_index];

            state++;

            Block.Reset_Pos();
        }
    }
    private void Move_Camera_to_Target(Vector2 target)
    {
        Vector2 target_lerp = Vector2.Lerp(ui_camera.transform.position, target, 0.01f);
        ui_camera.transform.position = new Vector3(target_lerp.x, ui_camera.transform.position.y, ui_camera.transform.position.z);
    }

    private void Awake()
    {
        Scorboy_Arm.max_states = 9;

        for (int i = 0; i < scorboy_max; i++)
        {
            GameObject newScorboy = Instantiate(Resources.Load<GameObject>("Prefabs/Scorboy"),
                                                new Vector3(-7f + i*7.0f, -3.178f, 0),
                                                Quaternion.identity,
                                                GameObject.FindGameObjectWithTag("Canvas").transform);

            newScorboy.tag = "Controlable";
            Scorboy_Arm Arm = newScorboy.GetComponent<Scorboy_Arm>();
            if (i == 0) Arm.LED_ON();

            _scorboy_Objects.Add(newScorboy);
        }

        ui_camera.transform.position = new Vector3(_scorboy_Objects[_scorboy_index].transform.position.x + cam_offset, ui_camera.transform.position.y, ui_camera.transform.position.z);
    }
    private void Start()
    {
        foreach(PID pid_controller in _pid_controllers)
        {
            pid_controller.max_output = max_output;
            pid_controller.min_output = -max_output;
            pid_controller.kp_gain = kp;
            pid_controller.ki_gain = ki;
            pid_controller.kd_gain = kd;
        }

        next_arm_btn.onClick.AddListener(Next_Arm_btn);
        save_state_btn.onClick.AddListener(Create_State);
    }
    private void OnDestroy()
    {
        next_arm_btn.onClick.RemoveListener(Next_Arm_btn);
        save_state_btn.onClick.RemoveListener(Create_State);
    }


    void FixedUpdate()
    {
        switch (state)
        {
            case States.Start:
                state_list_count = scorboy_controller.Arm.State_List.Count;
                break;
            case States.Run:
                Go2State();

                if (State_Is_Reached())
                {
                    scorboy_controller.Freeze_Scorboy();
                    foreach (PID controller in _pid_controllers) controller.Reset_Memory();

                    if ( scorboy_controller.Arm.state_index < scorboy_controller.Arm.State_List.Count - 1)    // Scorboy not run out of states 
                    {
                        // Set PID controllers setpoint
                        scorboy_controller.Arm.state_index++;
                        for (int joint_index = 0; joint_index < Scorboy_Arm.JOINTS_COUNT; joint_index++) _pid_controllers[joint_index].setpoint = scorboy_controller.Arm.Target_State().angles[joint_index];
                    }
                    else if(_scorboy_index < scorboy_max - 1)    // Scorboy runs out of states 
                    {
                        scorboy_controller.Arm.state_index = 0;
                        _scorboy_index++;
                        scorboy_controller.Set_Scorboy(_scorboy_Objects[_scorboy_index]);
                        for (int joint_index = 0; joint_index < Scorboy_Arm.JOINTS_COUNT; joint_index++) _pid_controllers[joint_index].setpoint = scorboy_controller.Arm.Target_State().angles[joint_index];
                    }
                    else    // Ran out of scorboys
                    {
                        _scorboy_index = 0;
                        scorboy_controller.Arm.state_index = 0;
                        state++;
                        Debug.Log("All done!");
                    }
                }

                state_list_count = scorboy_controller.Arm.State_List.Count;

                break;
            case States.End:
                break;
        }
        Move_Camera_to_Target(new Vector2(_scorboy_Objects[_scorboy_index].transform.GetChild(0).transform.position.x + cam_offset, -3.463393f));
    }
}
