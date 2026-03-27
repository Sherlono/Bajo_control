using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class DroneGameManager : MonoBehaviour
{
    public static DroneGameManager instance;

    public enum State
    {
        Tuning,
        Routing,
        Flying,
        Over
    }

    public static event Action<DroneCameraManager.State> onCamEvent;

    [SerializeField] private State state = 0;

    public List<GameObject> pointsList = new List<GameObject>();

    public hdrone drone;
    [SerializeField, HideInInspector] private GameObject _point_creator;
    [SerializeField, HideInInspector] private EndFlag _finishFlag;
    [SerializeField, HideInInspector] private SpriteRenderer panelLogo;
    [SerializeField, HideInInspector] private GameObject WinObject, LoseObject;

    [SerializeField] private int maxPoints;
    public static int currentPoint = 0;

    private void On_Point_Reached()
    {
        currentPoint++;
        if (currentPoint != maxPoints)
        {
            drone.targetpoint = new Vector2(pointsList[currentPoint].transform.position.x, pointsList[currentPoint].transform.position.y);
        }
    }

    private void Pass_Gains_2_Drone(int gains_set)
    {
        if(gains_set == 0)  // First set of PD gains passed
        {
            drone.Kp_x = PIDPanel.Kp;
            drone.Kp_y = PIDPanel.Kp;
            drone.Kv_x = PIDPanel.Td;
            drone.Kv_y = PIDPanel.Td;
        }
        else if (gains_set == 1)  // Second set of PD gains passed
        {
            drone.Kp_phi = PIDPanel.Kp;
            drone.Kv_phi = PIDPanel.Td;

            onCamEvent?.Invoke(DroneCameraManager.State.PanOut);
            _point_creator.SetActive(true);

            state++;
        }
    }

    private void Modify_Panel_Logo(int panelExiScreenCount)
    {
        if (panelExiScreenCount == 0)
        {
            panelLogo.sprite = Resources.Load("Graphics/drone_rotate", typeof(Sprite)) as Sprite;
            PIDPanel.Restart();
        }
    }

    private void Awake()
    {
        instance = this;
        if (instance != null && instance != this) Destroy(this);

        PIDPanel.onReady += Pass_Gains_2_Drone;
        PIDPanel.onExitScreen += Modify_Panel_Logo;
        refpoint.onReached += On_Point_Reached;

        float maxwind = 0.4f;
        drone.x_wind = UnityEngine.Random.Range(-maxwind, maxwind);
        drone.y_wind = UnityEngine.Random.Range(-0.1f, 0.1f);
    }

    private void OnDestroy()
    {
        state = 0;
        currentPoint = 0;

        PIDPanel.onReady -= Pass_Gains_2_Drone;
        PIDPanel.onExitScreen -= Modify_Panel_Logo;
        refpoint.onReached -= On_Point_Reached;
    }

    private void Start()
    {
        LoseObject.gameObject.SetActive(false);

        drone.Efficiency = hdrone.NORMALEFFICIENCY;
    }

    void Update()
    {
        switch (state)
        {
            case State.Routing:
                if (pointsList.Count == maxPoints)
                {
                    _point_creator.SetActive(false);
                    drone.targetpoint = new Vector2(pointsList[0].transform.position.x, pointsList[0].transform.position.y);
                    drone.Power = true;
                    state++;
                }
                break;
            case State.Flying:
                if (!_finishFlag.win)
                {
                    if (currentPoint >= maxPoints)
                    {
                        drone.targetpoint = new Vector2(_finishFlag.transform.position.x - 20, _finishFlag.transform.position.y);
                    }

                    if (drone.Power == true)
                    {
                        if (DroneCameraManager.GetState != DroneCameraManager.State.Follow_Drone) onCamEvent?.Invoke(DroneCameraManager.State.Follow_Drone);
                    }
                    else
                    {
                        LoseObject.SetActive(true);
                        LoseObject.GetComponent<Image>().enabled = true;
                        onCamEvent?.Invoke(DroneCameraManager.State.None);
                        state++;
                    }
                }
                else
                {
                    WinObject.SetActive(true);
                    WinObject.GetComponent<Image>().enabled = true;
                    onCamEvent?.Invoke(DroneCameraManager.State.None);
                    drone.Efficiency = 1f;
                    state++;
                }
                break;
        }
    }
}
