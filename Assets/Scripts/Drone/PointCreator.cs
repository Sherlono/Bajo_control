using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class PointCreator : MonoBehaviour
{
    [SerializeField]
    private DroneGameManager manager;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<DroneGameManager>();
        float height = 2f * manager.mainCam.orthographicSize;
        float width = height * manager.mainCam.aspect;

        GetComponent<BoxCollider>().size = new Vector3(width, height, 1f);
        GetComponent<BoxCollider>().center = Vector3.zero;
    }

    void OnMouseUp()
    {
        float x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        float y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        GameObject newpoint = Instantiate(Resources.Load<GameObject>("Prefabs/Drone/Refpoint"), new Vector3(x, y, 85), Quaternion.identity);
        manager.pointsList.Add(newpoint);
    }
}
