using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneCameraManager : MonoBehaviour
{
    public GameObject manager;
    public GameObject drone;
    public GameObject Focus;
    private Camera _camera;

    private float zoom;
    private float posX, posY;
    private float vel = 0f, smoothTime = 0.25f;
    private Vector2 focusPoint;

    // Start is called before the first frame update
    void Start()
    {
        posX = transform.position.x;
        posY = transform.position.y;
        focusPoint = new Vector2(Focus.GetComponent<Transform>().position.x, Focus.GetComponent<Transform>().position.y);
        _camera = GetComponent<Camera>();
        zoom = _camera.orthographicSize;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (manager.GetComponent<DroneGameManager>().state == 4)
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
        }
        else
        {
            _camera.orthographicSize = 240;
        }
    }
}
