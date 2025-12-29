using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DroneCameraManager : MonoBehaviour
{
    public static DroneCameraManager instance;

    public enum State
    {
        None,
        PanOut,
        Follow_Drone
    }

    [SerializeField] private GameObject drone;
    private Camera _camera;

    [SerializeField] private State state = State.None;
    public static State GetState { get { return instance.state; } }

    //[SerializeField] private float zoom;
    private float posX, posY;
    private float vel = 0f, smoothTime = 0.25f;
    private Vector2 focusPoint;
    
    private void Set_State(State input)
    {
        state = input;
    }

    private void Awake()
    {
        instance = this;
        DroneGameManager.onCamEvent += Set_State;
        Hazard.onEnter += delegate { Set_State(State.None); };
    }

    private void OnDestroy()
    {
        DroneGameManager.onCamEvent -= Set_State;
        Hazard.onEnter -= delegate { Set_State(State.None); };
        instance = null;
    }

    void Start()
    {
        posX = transform.position.x;
        posY = transform.position.y;

        Transform Focus = GameObject.Find("FocusPoint").transform;
        focusPoint = new Vector2(Focus.position.x, Focus.position.y);
        _camera = GetComponent<Camera>();

        _camera.transform.position = new Vector3(drone.transform.position.x, drone.transform.position.y, transform.position.z);
    }

    void FixedUpdate()
    {
        switch(state)
        {
            case State.PanOut:
                _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, 300f, ref vel, smoothTime);

                float x, y;

                if (_camera.transform.position.x < focusPoint.x)
                {
                    x = Mathf.SmoothDamp(_camera.transform.position.x, posX, ref vel, smoothTime);
                }
                else
                {
                    x = focusPoint.x;
                }

                if (_camera.transform.position.y < focusPoint.y)
                {
                    y = Mathf.SmoothDamp(_camera.transform.position.y, posY, ref vel, smoothTime);
                }
                else
                {
                    y = focusPoint.y;
                }

                posX = focusPoint.x;
                posY = focusPoint.y;
                _camera.transform.position = new Vector3(x, y, _camera.transform.position.z);
                break;
            case State.Follow_Drone:
                transform.position = new Vector3(drone.transform.position.x, drone.transform.position.y, transform.position.z);
                _camera.orthographicSize = 240;
                break;
            default:
                _camera.orthographicSize = 240;
                break;

        }
    }
}
