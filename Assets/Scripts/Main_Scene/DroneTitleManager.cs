using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DroneTitleManager : MonoBehaviour
{
    [SerializeField] private hdrone drone;
    [SerializeField] private GameObject targetObject;
    private Vector2 target;

    private void Start()
    {
        target = targetObject.transform.position;

        drone.targetpoint = new Vector2(target.x, target.y);
        drone.Power = true;
        drone.Efficiency = 1f;
    }
}
