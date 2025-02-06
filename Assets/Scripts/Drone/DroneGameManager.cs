using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class DroneGameManager : MonoBehaviour
{
    public int state = 0;
    public List<GameObject> pointsList = new List<GameObject>();

    [HideInInspector]
    public Camera mainCam;
    [HideInInspector]
    public hdrone drone;
    private PIDPanel panel;
    private GameObject creator;
    private GameObject finishFlag;
    private SpriteRenderer panelLogo;
    public Slider KpSlider, TdSlider;
    [SerializeField]
    private int maxPoints, currentPoint = 0;

    private void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        drone = GameObject.Find("Drone").GetComponent<hdrone>();
        panel = GameObject.Find("Control Panel").GetComponent<PIDPanel>();
        finishFlag = GameObject.Find("Flag");
        panelLogo = GameObject.Find("DroneLogo").GetComponent<SpriteRenderer>();

        creator = Instantiate(Resources.Load<GameObject>("Prefabs/PointCreator"), GameObject.Find("Controls").transform);
        creator.SetActive(false);
        mainCam.transform.position = new Vector3(drone.transform.position.x, drone.transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state) {
            case 0: // Entering first parameters
                if (panel.state == 1)
                {
                    drone.Kp_x = KpSlider.value;
                    drone.Kp_y = KpSlider.value;
                    drone.Kv_x = TdSlider.value;
                    drone.Kv_y = TdSlider.value;

                    state++;
                }
                break;
            case 1: // First parameters entered
                if (panel.state == 2)
                {
                    panelLogo.sprite = Resources.Load("drone_rotate", typeof(Sprite)) as Sprite;
                    panel.Restart();
                    KpSlider.Reset();
                    TdSlider.Reset();
                    state++;
                }
                break;
            case 2: // Entering second parameters
                if (panel.state == 1)
                {
                    drone.Kp_phi = KpSlider.value;
                    drone.Kv_phi = KpSlider.value;
                        
                    state++;
                }
                break;
            case 3: // Point creating start
                creator.SetActive(true);
                state++;
                break;
            case 4: // Point creating end, then drone start
                if(pointsList.Count == maxPoints)
                {
                    creator.SetActive(false);
                    drone.targetpoint = new Vector2(pointsList[0].transform.position.x, pointsList[0].transform.position.y);
                    drone.Power(true);
                    state++;
                }
                break;
            case 5: // Drone is active
                mainCam.transform.position = new Vector3(drone.transform.position.x, drone.transform.position.y, transform.position.z);

                if (currentPoint < maxPoints)
                {
                    if (pointsList[currentPoint].GetComponent<refpoint>().done == true)
                    {
                        currentPoint++;
                        if (currentPoint != maxPoints)
                        {
                            drone.targetpoint = new Vector2(pointsList[currentPoint].transform.position.x, pointsList[currentPoint].transform.position.y);
                        }
                    }
                }
                else
                {
                    drone.targetpoint = new Vector2(finishFlag.transform.position.x - 20, finishFlag.transform.position.y);
                }
                break;
        }
    }
}
