using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scorboy_Controller : MonoBehaviour
{
    [HideInInspector] public GameObject Scorbot_obj;
    [HideInInspector] public Scorboy_Arm Arm;
    [HideInInspector] public Scorboy_Claw Claw;

    public Button[] buttons = new Button[10];

    public TextMeshProUGUI State_number_display;
    private float joint_speed = 20.0f;
    private int prev_state_count;

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

    public void Toggle_Interactable()
    {
        foreach (Button btn in buttons) btn.interactable = !btn.interactable;
    }

    private void Start()
    {
        Scorbot_obj = GameObject.FindGameObjectWithTag("Controlable");
        Claw = Scorbot_obj.transform.GetChild(0).GetComponent<Scorboy_Claw>();
        Arm = Scorbot_obj.gameObject.GetComponent<Scorboy_Arm>();

        prev_state_count = 0;

        buttons[0].onClick.AddListener(delegate { Claw.Toggle(); });

        for (int i = 0; i < 3; i++)
        {
            int joint_index = i, btn_index = 1 + i * 3;
            buttons[btn_index].onClick.AddListener(delegate { Arm.Joint_Set(joint_index, -joint_speed); });
            buttons[btn_index + 1].onClick.AddListener(delegate { Arm.Joint_Set(joint_index, 0); });
            buttons[btn_index + 2].onClick.AddListener(delegate { Arm.Joint_Set(joint_index, joint_speed); });
        }
    }

    private void OnDestroy()
    {
        buttons[0].onClick.RemoveListener(delegate { Claw.Toggle(); });
        for (int i = 0; i < 3; i++)
        {
            int joint_index = i, btn_index = 1 + i * 3;
            buttons[btn_index].onClick.RemoveListener(delegate { Arm.Joint_Set(joint_index, -joint_speed); });
            buttons[btn_index + 1].onClick.RemoveListener(delegate { Arm.Joint_Set(joint_index, 0); });
            buttons[btn_index + 2].onClick.RemoveListener(delegate { Arm.Joint_Set(joint_index, joint_speed); });
        }
    }

    private void FixedUpdate()
    {
        if(prev_state_count != Arm.State_List.Count) State_number_display.text = Arm.State_List.Count.ToString();
        prev_state_count = Arm.State_List.Count;
    }
}
