using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scorboy_Controller : MonoBehaviour
{
    [HideInInspector] public GameObject Scorbot_obj;
    [HideInInspector] public Scorboy_Arm Arm;
    [HideInInspector] public Scorboy_Claw Claw;

    [HideInInspector] public Button claw_toggle;
    [HideInInspector] public Button claw_minus, claw_stop, claw_plus;
    [HideInInspector] public Button joint2_minus, joint2_stop, joint2_plus;
    [HideInInspector] public Button joint1_minus, joint1_stop, joint1_plus;

    public TextMeshProUGUI State_number_display;
    private float joint_speed = 20.0f;
    private int prev_state_count;

    private void Start()
    {
        Scorbot_obj = GameObject.FindGameObjectWithTag("Controlable");
        Claw = Scorbot_obj.transform.GetChild(0).GetComponent<Scorboy_Claw>();
        Arm = Scorbot_obj.gameObject.GetComponent<Scorboy_Arm>();

        prev_state_count = 0;

        claw_toggle.onClick.AddListener(delegate { Claw.Toggle(); });
        claw_minus.onClick.AddListener(delegate { Arm.Joint_Set(2, -joint_speed); });
        claw_stop.onClick.AddListener(delegate { Arm.Joint_Set(2, 0); });
        claw_plus.onClick.AddListener(delegate { Arm.Joint_Set(2, joint_speed); });
        joint2_minus.onClick.AddListener(delegate { Arm.Joint_Set(1, -joint_speed); });
        joint2_stop.onClick.AddListener(delegate { Arm.Joint_Set(1, 0); });
        joint2_plus.onClick.AddListener(delegate { Arm.Joint_Set(1, joint_speed); });
        joint1_minus.onClick.AddListener(delegate { Arm.Joint_Set(0, -joint_speed); });
        joint1_stop.onClick.AddListener(delegate { Arm.Joint_Set(0, 0); });
        joint1_plus.onClick.AddListener(delegate { Arm.Joint_Set(0, joint_speed); });
    }

    private void OnDestroy()
    {
        claw_toggle.onClick.RemoveListener(delegate { Claw.Toggle(); });
        claw_minus.onClick.RemoveListener(delegate { Arm.Joint_Set(2, -joint_speed); });
        claw_stop.onClick.RemoveListener(delegate { Arm.Joint_Set(2, 0); });
        claw_plus.onClick.RemoveListener(delegate { Arm.Joint_Set(2, joint_speed); });
        joint2_minus.onClick.RemoveListener(delegate { Arm.Joint_Set(1, -joint_speed); });
        joint2_stop.onClick.RemoveListener(delegate { Arm.Joint_Set(1, 0); });
        joint2_plus.onClick.RemoveListener(delegate { Arm.Joint_Set(1, joint_speed); });
        joint1_minus.onClick.RemoveListener(delegate { Arm.Joint_Set(0, -joint_speed); });
        joint1_stop.onClick.RemoveListener(delegate { Arm.Joint_Set(0, 0); });
        joint1_plus.onClick.RemoveListener(delegate { Arm.Joint_Set(0, joint_speed); });
    }

    public void Set_Scorboy(GameObject new_scorboy)
    {
        Scorbot_obj = new_scorboy;
        Arm = new_scorboy.GetComponent<Scorboy_Arm>();
        Claw = new_scorboy.transform.GetChild(0).GetComponent<Scorboy_Claw>();
    }

    public void Freeze_Scorboy()
    {
        for(int joint_index = 0; joint_index < 3; joint_index++) Arm.Joint_Set(joint_index, 0);
    }

    private void FixedUpdate()
    {
        if(prev_state_count != Arm.State_List.Count) State_number_display.text = Arm.State_List.Count.ToString();
        prev_state_count = Arm.State_List.Count;
    }
}
