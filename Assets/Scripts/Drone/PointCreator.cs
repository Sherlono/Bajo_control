using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class PointCreator : MonoBehaviour
{
    [SerializeField] private DroneGameManager manager;
    [SerializeField] private Camera mainCam;

    void Start()
    {
        float height = 2f * mainCam.orthographicSize;
        float width = height * mainCam.aspect;

        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.size = new Vector2(width, height);
        collider.offset = Vector2.zero;
    }

    void OnMouseUp()
    {
        float x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        float y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        GameObject newpoint = Instantiate(Resources.Load<GameObject>("Prefabs/Drone/Refpoint"), new Vector3(x, y, 85), Quaternion.identity);
        manager.pointsList.Add(newpoint);
    }
}
