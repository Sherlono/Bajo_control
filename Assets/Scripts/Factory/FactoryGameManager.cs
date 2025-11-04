using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class FactoryGameManager : MonoBehaviour
{
    [HideInInspector] public Scorboy_Controller Controller;
    private List<GameObject> Scorboy_Objects = new();
    [SerializeField] private int scorboy_index = 0;

    [HideInInspector] public Button next_arm_btn;
    [HideInInspector] public Button save_state_btn;

    private void Next_Arm()
    {
        Scorboy_Objects[scorboy_index].GetComponent<Scorboy_Arm>().LED_OFF();
        scorboy_index++;
        Controller.Scorbot_obj = Scorboy_Objects[scorboy_index];
        Controller.Arm = Controller.Scorbot_obj.GetComponent<Scorboy_Arm>();
        Controller.Claw = Controller.Scorbot_obj.transform.GetChild(0).GetComponent<Scorboy_Claw>();

        Controller.Arm.LED_ON();
    }

    private void Awake()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject newScorboy = Instantiate(Resources.Load<GameObject>("Prefabs/Scorboy"), new Vector3(-5.8416f + i*5.0f, -3.178f, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
            newScorboy.tag = "Controlable";
            if(i == 0) newScorboy.GetComponent<Scorboy_Arm>().LED_ON();
            Scorboy_Objects.Add(newScorboy);
        }

    }

    private void Start()
    {
        next_arm_btn.onClick.AddListener(Next_Arm);
        save_state_btn.onClick.AddListener(delegate { Scorboy_Objects[scorboy_index].GetComponent<Scorboy_Arm>().Create_State(); });
    }

    private void OnDestroy()
    {
        next_arm_btn.onClick.RemoveListener(Next_Arm);
        save_state_btn.onClick.RemoveListener(delegate { Scorboy_Objects[scorboy_index].GetComponent<Scorboy_Arm>().Create_State(); });
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
