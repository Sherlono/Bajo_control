using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class Scorboy_Controller : MonoBehaviour
{
    public GameObject Scorbot_obj;
    public Scorboy_Arm Arm;
    public Scorboy_Claw Claw;

    [HideInInspector] public Button claw_toggle;
    [HideInInspector] public Button claw_minus, claw_stop, claw_plus;
    [HideInInspector] public Button joint2_minus, joint2_stop, joint2_plus;
    [HideInInspector] public Button joint1_minus, joint1_stop, joint1_plus;

    private void Start()
    {
        Scorbot_obj = GameObject.FindGameObjectWithTag("Controlable");

        Claw = Scorbot_obj.transform.GetChild(0).GetComponent<Scorboy_Claw>();
        Arm = Scorbot_obj.gameObject.GetComponent<Scorboy_Arm>();

        claw_toggle.onClick.AddListener(delegate { Claw.Toggle(); });
        claw_minus.onClick.AddListener(delegate { Claw.Set(-20); });
        claw_stop.onClick.AddListener(delegate { Claw.Set(0); });
        claw_plus.onClick.AddListener(delegate { Claw.Set(20); });
        joint2_minus.onClick.AddListener(delegate { Arm.Joint_2_Set(-20); });
        joint2_stop.onClick.AddListener(delegate { Arm.Joint_2_Set(0); });
        joint2_plus.onClick.AddListener(delegate { Arm.Joint_2_Set(20); });
        joint1_minus.onClick.AddListener(delegate { Arm.Joint_1_Set(-20); });
        joint1_stop.onClick.AddListener(delegate { Arm.Joint_1_Set(0); });
        joint1_plus.onClick.AddListener(delegate { Arm.Joint_1_Set(20); });
    }

    private void OnDestroy()
    {
        claw_toggle.onClick.RemoveListener(delegate { Claw.Toggle(); });
        claw_minus.onClick.RemoveListener(delegate { Claw.Set(-20); });
        claw_stop.onClick.RemoveListener(delegate { Claw.Set(0); });
        claw_plus.onClick.RemoveListener(delegate { Claw.Set(20); });
        joint2_minus.onClick.RemoveListener(delegate { Arm.Joint_2_Set(-20); });
        joint2_stop.onClick.RemoveListener(delegate { Arm.Joint_2_Set(0); });
        joint2_plus.onClick.RemoveListener(delegate { Arm.Joint_2_Set(20); });
        joint1_minus.onClick.RemoveListener(delegate { Arm.Joint_1_Set(-20); });
        joint1_stop.onClick.RemoveListener(delegate { Arm.Joint_1_Set(0); });
        joint1_plus.onClick.RemoveListener(delegate { Arm.Joint_1_Set(20); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
