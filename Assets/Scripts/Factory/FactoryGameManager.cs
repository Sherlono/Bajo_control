using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactoryGameManager : MonoBehaviour
{
    [Header("Objects")]
    [HideInInspector] public Scorboy_Controller Controller;
    private List<GameObject> Scorboy_Objects = new();

    [Header("Camera Stuff")]
    public Camera Main_Camera;
    //private float posX, posY;

    [Header("Botones")]
    [HideInInspector] public Button next_arm_btn;
    [HideInInspector] public Button save_state_btn;

    [Header("Data")]
    public int state = 0;
    public int scorboy_max = 3;
    private int scorboy_index = 0;

    private void Next_Arm()
    {
        if (state == 0)
        {
            if (scorboy_index < scorboy_max - 1)
            {
                Scorboy_Arm Arm = Scorboy_Objects[scorboy_index].GetComponent<Scorboy_Arm>();
                Arm.LED_OFF();
                Arm.Joint_1_Set(0);
                Arm.Joint_2_Set(0);
                Scorboy_Objects[scorboy_index].transform.GetChild(0).GetComponent<Scorboy_Claw>().Set(0);

                scorboy_index++;
                Controller.Set_Scorboy(Scorboy_Objects[scorboy_index]);

                Controller.Arm.LED_ON();
            }
            else
            {
                state++;
            }
        }
        /*else
        {

        }*/
    }

    private void Awake()
    {
        for (int i = 0; i < scorboy_max; i++)
        {
            GameObject newScorboy = Instantiate(Resources.Load<GameObject>("Prefabs/Scorboy"),
                                                new Vector3((-5.8416f + i*5.0f), -3.178f, 0),
                                                Quaternion.identity,
                                                GameObject.FindGameObjectWithTag("Canvas").transform);
            newScorboy.tag = "Controlable";
            if(i == 0) newScorboy.GetComponent<Scorboy_Arm>().LED_ON();
            Scorboy_Objects.Add(newScorboy);
        }

    }

    private void Start()
    {
        /*posX = Scorboy_Objects[scorboy_index].transform.position.x;
        posY = Scorboy_Objects[scorboy_index].transform.position.y;*/

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
        /*posX = Scorboy_Objects[scorboy_index].transform.position.x;
        posY = Scorboy_Objects[scorboy_index].transform.position.y;
        Vector3 target = Vector3.Lerp(Main_Camera.transform.position,
                                      Scorboy_Objects[scorboy_index].transform.position,
                                      0.2f);*/

        //Main_Camera.transform.position = new Vector3(target.x, target.y, Main_Camera.transform.position.z);
    }
}
