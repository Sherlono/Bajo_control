using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneCameraManager : MonoBehaviour
{
    [SerializeField] private GameObject drone;
    private Camera _camera;

    private int state = 0;
    [SerializeField] private float zoom;
    private float posX, posY;
    private float vel = 0f, smoothTime = 0.25f;
    private Vector2 focusPoint;

    public void Set_State(int s)
    {
        state = s;
    }

    void Start()
    {
        posX = transform.position.x;
        posY = transform.position.y;

        Transform Focus = GameObject.Find("FocusPoint").transform;
        focusPoint = new Vector2(Focus.position.x, Focus.position.y);
        _camera = GetComponent<Camera>();
        zoom = _camera.orthographicSize;

        _camera.transform.position = new Vector3(drone.transform.position.x, drone.transform.position.y, transform.position.z);
    }

    void FixedUpdate()
    {
        if (state == 1)
        {
            if (zoom <= 300f)
            {
                _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, 300f, ref vel, smoothTime);
            }
            else
            {
                _camera.orthographicSize = 300f;
            }

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

        }else if (state == 2)
        {
            transform.position = new Vector3(drone.transform.position.x, drone.transform.position.y, transform.position.z);
            _camera.orthographicSize = 240;
        }
        else
        {
            _camera.orthographicSize = 240;
        }
    }
}
