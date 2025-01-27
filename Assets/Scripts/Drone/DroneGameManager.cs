using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class DroneGameManager : MonoBehaviour
{
    public Camera mainCam;
    public PIDPanel panel;
    public GameObject panelLogo;
    public GameObject creator;
    public GameObject finishFlag;
    public Slider KpSlider, TdSlider;
    public hdrone drone;
    public List<GameObject> pointsList = new List<GameObject>();
    public int state = 0;
    public int maxPoints, currentPoint = 0;

    private void Start()
    {
        mainCam.transform.position = new Vector3(drone.transform.position.x, drone.transform.position.y, transform.position.z);
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
            case 1: // Entering first parameters
                if (panel.state == 2)
                {
                    panelLogo.GetComponent<SpriteRenderer>().sprite = Resources.Load("drone_rotate", typeof(Sprite)) as Sprite;
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
                    drone.refpoint = new Vector2(pointsList[0].transform.position.x, pointsList[0].transform.position.y);
                    drone.power = true;
                    state++;
                }
                break;
            case 5: // Drone is active
                mainCam.transform.position = new Vector3(drone.transform.position.x, drone.transform.position.y, transform.position.z);

                if (currentPoint < maxPoints)
                {
                    if (pointsList[currentPoint].GetComponent<refpoint>().done == true)
                    {
                        currentPoint++;
                        if (currentPoint != maxPoints)
                        {
                            drone.refpoint = new Vector2(pointsList[currentPoint].transform.position.x, pointsList[currentPoint].transform.position.y);
                        }
                    }
                }
                else
                {
                    drone.refpoint = new Vector2(finishFlag.transform.position.x - 20, finishFlag.transform.position.y);
                }
                break;
        }
    }
}
