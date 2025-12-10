using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    private Pool pool;
    private Spawner kidSpawner;
    private Setpointer setpointer;
    private GameObject WinObject, LoseObject;
    private Vector3 pnt1, pnt2;
    private Vector3 centerPoint;

    private float a, b;

    private bool Below_Pool(Vector3 kid_position)
    {
        float kid_y_minus_offset = kid_position.y - 55;
        bool below_area1 = kid_position.x <= pnt1.x && kid_y_minus_offset < pnt1.y;
        bool below_area2 = kid_position.x <= pnt2.x && kid_y_minus_offset < a * kid_position.x + b;
        bool below_area3 = kid_position.x > pnt2.x && kid_y_minus_offset < pnt2.y;

        return below_area1 || below_area2 || below_area3;
    }


    private void Start()
    {
        pool = GameObject.Find("Water_Surface").GetComponent<Pool>();
        kidSpawner = GameObject.Find("KidSpawner").GetComponent<Spawner>();
        setpointer = GameObject.Find("Setpointer").GetComponent<Setpointer>();
        pnt1 = GameObject.Find("p1 mark").GetComponent<Transform>().position;
        pnt2 = GameObject.Find("p2 mark").GetComponent<Transform>().position;

        a = (pnt2.y - pnt1.y) / (pnt2.x - pnt1.x);
        b = pnt1.y - a * pnt1.x;

        WinObject = GameObject.Find("WinSplash");
        LoseObject = GameObject.Find("FailSplash");

        centerPoint = GameObject.Find("centerPoint").transform.localPosition;
    }

    private void FixedUpdate()
    {
        if (kidSpawner.kidCount == kidSpawner.maxKids && kidSpawner.kidList.Count == 0) { // Win
            WinObject.transform.localPosition = new Vector3(0, centerPoint.y, transform.localPosition.z);
            gameObject.SetActive(false);
        } else {
            if (setpointer.state == 1) {  // Simulation start
                if (pool.controller.h > setpointer.value - 0.1f) {   // Setpoint "reached"
                    kidSpawner.Activate(true);
                }

                foreach(GameObject kid in kidSpawner.kidList)
                {
                    Vector3 kid_position = kid.GetComponent<Transform>().position;

                    if (Below_Pool(kid_position)) {
                        pool.Pause(true);
                        GameObject Water_splash = Instantiate(Resources.Load<GameObject>("Prefabs/Pool/hurt"),
                                                              new Vector3(kid_position.x, kid_position.y - 55, kid_position.z),
                                                              Quaternion.identity,
                                                              GameObject.FindGameObjectWithTag("Canvas").transform);
                    }
                }
            }
            if (pool.IsPaused() && setpointer.state == 1) {    // Fail
                LoseObject.transform.localPosition = new Vector3(0, centerPoint.y, transform.localPosition.z);
                kidSpawner.Activate(false);
                foreach(GameObject kid in kidSpawner.kidList) kid.GetComponent<Kid>().Pause(true);
                gameObject.SetActive(false);
            }
        }
    }
}
