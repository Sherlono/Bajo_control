using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scorboy_Controller : MonoBehaviour
{
    public GameObject Scorbot_obj;
    public Scorboy_Arm Arm;
    public Scorboy_Claw Claw;

    [HideInInspector] public Button clawToggleBtn;
    [HideInInspector] public Button allLeftBtn;
    [HideInInspector] public Button allRightBtn;
    [HideInInspector] public Button[] buttons = new Button[6];

    [HideInInspector] public TextMeshProUGUI StateNumberDisplay;
    const float joint_speed = 20.0f;
    private int _prev_state_count;

    public void Set_Scorboy(GameObject new_scorboy)
    {
        Scorbot_obj = new_scorboy;
        Arm = new_scorboy.GetComponent<Scorboy_Arm>();
        Claw = new_scorboy.transform.GetChild(0).GetComponent<Scorboy_Claw>();
    }

    public void Toggle_Interactable()
    {
        clawToggleBtn.interactable = !clawToggleBtn.interactable;
        allLeftBtn.interactable = !allLeftBtn.interactable;
        allRightBtn.interactable = !allRightBtn.interactable;
        foreach (Button btn in buttons) btn.interactable = !btn.interactable;
    }

    private void All_Set(float speed)
    {
        Arm.Joint_Set(0, speed);
        Arm.Joint_Set(1, speed);
        Arm.Joint_Set(2, speed);
    }

    private void Start()
    {
        Scorbot_obj = GameObject.FindGameObjectWithTag("Controlable");
        Claw = Scorbot_obj.transform.GetChild(0).GetComponent<Scorboy_Claw>();
        Arm = Scorbot_obj.gameObject.GetComponent<Scorboy_Arm>();

        _prev_state_count = 0;

        clawToggleBtn.onClick.AddListener(delegate { Claw.Toggle(); });
        allLeftBtn.gameObject.GetComponent<MouseHoldable>().MouseHold.AddListener(delegate { All_Set(-joint_speed); });
        allLeftBtn.gameObject.GetComponent<MouseHoldable>().MouseRelease.AddListener(delegate { All_Set(0); });
        allRightBtn.gameObject.GetComponent<MouseHoldable>().MouseHold.AddListener(delegate { All_Set(joint_speed); });
        allRightBtn.gameObject.GetComponent<MouseHoldable>().MouseRelease.AddListener(delegate { All_Set(0); });

        for (int i = 0; i < 3; i++)
        {
            int joint_index = i, btn_index = i * 2;
            MouseHoldable holdable_1 = buttons[btn_index].gameObject.GetComponent<MouseHoldable>();
            MouseHoldable holdable_2 = buttons[btn_index + 1].gameObject.GetComponent<MouseHoldable>();
            holdable_1.MouseHold.AddListener(delegate { Arm.Joint_Set(joint_index, -joint_speed); });
            holdable_1.MouseRelease.AddListener(delegate { Arm.Joint_Set(joint_index, 0); });
            holdable_2.MouseHold.AddListener(delegate { Arm.Joint_Set(joint_index, joint_speed); });
            holdable_2.MouseRelease.AddListener(delegate { Arm.Joint_Set(joint_index, 0); });
        }
    }

    private void OnDestroy()
    {
        clawToggleBtn.onClick.RemoveListener(delegate { Claw.Toggle(); });
        allLeftBtn.gameObject.GetComponent<MouseHoldable>().MouseHold.RemoveListener(delegate { All_Set(-joint_speed); });
        allLeftBtn.gameObject.GetComponent<MouseHoldable>().MouseRelease.RemoveListener(delegate { All_Set(0); });
        allRightBtn.gameObject.GetComponent<MouseHoldable>().MouseHold.RemoveListener(delegate { All_Set(joint_speed); });
        allRightBtn.gameObject.GetComponent<MouseHoldable>().MouseRelease.RemoveListener(delegate { All_Set(0); });

        for (int i = 0; i < 3; i++)
        {
            int joint_index = i, btn_index = i * 2;
            MouseHoldable holdable_1 = buttons[btn_index].gameObject.GetComponent<MouseHoldable>();
            MouseHoldable holdable_2 = buttons[btn_index + 1].gameObject.GetComponent<MouseHoldable>();
            holdable_1.MouseHold.RemoveListener(delegate { Arm.Joint_Set(joint_index, -joint_speed); });
            holdable_1.MouseRelease.RemoveListener(delegate { Arm.Joint_Set(joint_index, 0); });
            holdable_2.MouseHold.RemoveListener(delegate { Arm.Joint_Set(joint_index, joint_speed); });
            holdable_2.MouseRelease.RemoveListener(delegate { Arm.Joint_Set(joint_index, 0); });
        }
    }

    private void FixedUpdate()
    {
        if(_prev_state_count != Arm.State_List.Count)
        {
            StateNumberDisplay.text = Arm.State_List.Count.ToString();
        }
        _prev_state_count = Arm.State_List.Count;
    }
}
