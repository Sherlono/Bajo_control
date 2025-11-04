using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DroneTitleManager : MonoBehaviour
{
    public int state = 0;

    [HideInInspector]
    public hdrone drone;
    public Vector2 target;

    private void Start()
    {
        drone = GameObject.Find("Title").GetComponent<hdrone>();
        target = GameObject.Find("TargetObj").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case 0: // Entering first parameters
                drone.targetpoint = new Vector2(target.x, target.y);
                drone.Power(true);
                state++;
                break;
            case 1:

                break;
        }
    }
}
