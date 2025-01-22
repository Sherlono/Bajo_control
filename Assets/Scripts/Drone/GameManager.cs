using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PIDPanel panel;
    public GameObject panelLogo;
    public Slider KpSlider, TdSlider;
    public hdrone drone;
    public List<Vector2> pointsList = new List<Vector2>();
    public int state = 0;
    private int maxPoints, pointCount = 0;

    // Update is called once per frame
    void Update()
    {
        if (state == 0) // Entering first parameters
        {
            if (panel.state == 1)
            {
                drone.Kp_x = KpSlider.value;
                drone.Kp_y = KpSlider.value;
                drone.Kv_x = TdSlider.value;
                drone.Kv_y = TdSlider.value;

                state++;
            }
        }
        else if (state == 1)    // Panel moving out of frame
        {
            if(panel.state == 2)
            {
                panelLogo.GetComponent<SpriteRenderer>().sprite = Resources.Load("drone_rotate", typeof(Sprite)) as Sprite;
                panel.Restart();
                KpSlider.Reset();
                TdSlider.Reset();
                state++;
            }
        }
        else if (state == 2)    // Entering second parameters
        {
            if (panel.state == 1)
            {
                drone.Kp_phi = KpSlider.value;
                drone.Kv_phi = KpSlider.value;

                drone.power = true;
                state++;
            }
        }
    }
}
