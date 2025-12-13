using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DroneGameManager : MonoBehaviour
{
    public UnityEvent Cam_Zoom_Out;
    public UnityEvent Cam_Follow_Drone;
    public UnityEvent Cam_Final;

    public int state = 0;
    public int level;
    public List<GameObject> pointsList = new List<GameObject>();
    public List<Obstacle> wallList = new List<Obstacle>();

    public hdrone drone;
    [SerializeField] private PIDPanel panel;
    [SerializeField] private GameObject _point_creator;
    [SerializeField] private EndFlag _finishFlag;
    [SerializeField] private SpriteRenderer panelLogo;
    [SerializeField] private Image windArrow;
    [SerializeField] private GameObject WinObject, LoseObject;
    [SerializeField] private Slider KpSlider, TdSlider;

    [SerializeField] private int maxPoints;
    private int currentPoint = 0;

    private void Start()
    {
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
        switch (state)
        {
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
                    drone.Kv_phi = TdSlider.value;

                    state++;
                }
                break;
            case 3: // Point creating start
                Cam_Zoom_Out?.Invoke();
                _point_creator.SetActive(true);
                state++;
                break;
            case 4: // Point creating end, then drone start
                if (pointsList.Count == maxPoints)
                {
                    _point_creator.SetActive(false);
                    drone.targetpoint = new Vector2(pointsList[0].transform.position.x, pointsList[0].transform.position.y);
                    drone.Power(true);
                    state++;
                }
                break;
            case 5: // Drone is active
                if (!_finishFlag.win)
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
                        drone.targetpoint = new Vector2(_finishFlag.transform.position.x - 20, _finishFlag.transform.position.y);
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
                        Cam_Follow_Drone?.Invoke();
                    }
                    else
                    {
                        LoseObject.SetActive(true);
                        state++;
                    }
                }
                else
                {
                    WinObject.SetActive(true);
                    Cam_Final?.Invoke();
                    state++;
                }
                break;
        }
    }
}
