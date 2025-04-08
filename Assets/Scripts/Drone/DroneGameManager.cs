using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DroneGameManager : MonoBehaviour
{
    public int state = 0;
    public int level;
    public List<GameObject> pointsList = new List<GameObject>();
    public List<Obstacle> wallList = new List<Obstacle>();

    [HideInInspector]
    public Camera mainCam;
    [HideInInspector]
    public hdrone drone;
    private PIDPanel panel;
    private GameObject creator;
    private EndFlag finishFlag;
    private Vector3 canvasCenter;
    private SpriteRenderer panelLogo;
    private Image windArrow;
    public GameObject WinObject, LoseObject;
    public Slider KpSlider, TdSlider;
    [SerializeField]
    private int maxPoints, currentPoint = 0;

    private void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        drone = GameObject.Find("Drone").GetComponent<hdrone>();
        panel = GameObject.Find("Control Panel").GetComponent<PIDPanel>();
        finishFlag = GameObject.Find("Flag").GetComponent<EndFlag>();
        canvasCenter = GameObject.Find("DroneLogo").transform.localPosition;
        panelLogo = GameObject.Find("DroneLogo").GetComponent<SpriteRenderer>();
        windArrow = GameObject.Find("Arrow").GetComponent<Image>();

        creator = Instantiate(Resources.Load<GameObject>("Prefabs/PointCreator"), GameObject.Find("centerPoint").transform);
        creator.SetActive(false);
        mainCam.transform.position = new Vector3(drone.transform.position.x, drone.transform.position.y, mainCam.transform.position.z);

        if (level == 0) // Wind force
        {
            float maxwind = 0.4f;
            drone.x_wind = Random.Range(-maxwind, maxwind);
            drone.y_wind = Random.Range(-0.1f, 0.1f);
            windArrow.transform.localScale = new Vector3(-drone.x_wind / maxwind, 1, 1);
        }
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
                if (!finishFlag.win)
                {
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

                    foreach (Obstacle obs in wallList)
                    {
                        if (obs.colided)
                        {
                            drone.Power(false);
                        }
                    }
                    if (drone.IsPowered())
                    {
                        mainCam.transform.position = new Vector3(drone.transform.position.x, drone.transform.position.y, transform.position.z);
                    }
                    else
                    {
                        LoseObject.transform.localPosition = new Vector3(0, canvasCenter.y, WinObject.transform.localPosition.z);
                    }
                }
                else
                {
                    WinObject.transform.localPosition = new Vector3(0, canvasCenter.y, WinObject.transform.localPosition.z);
                }
                break;
        }
    }
}
