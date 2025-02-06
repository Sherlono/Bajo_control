using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    private GameObject CenterPoint;
    private float _y;
    private bool isIn = false;

    // Start is called before the first frame update
    void Start()
    {
        CenterPoint = GameObject.Find("centerPoint");
        _y = transform.position.y;
    }

    public void Toggle() {
        if (isIn)
        {
            MoveOut();
            isIn = false;
        }
        else
        {
            MoveIn();
            isIn = true;
        }
    }

    private void MoveIn()
    {
        transform.position = new Vector3(transform.position.x, CenterPoint.transform.position.y, transform.position.z);
    }
    private void MoveOut()
    {
        transform.position = new Vector3(transform.position.x, _y, transform.position.z);
    }
}
