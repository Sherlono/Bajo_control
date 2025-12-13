using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolGameManager : MonoBehaviour
{
    private Pool pool;
    [Header ("Game Objects")]
    [SerializeField] private Spawner kidSpawner;
    [SerializeField] private Setpointer setpointer;

    [Header("Splashes")]
    [SerializeField] private GameObject WinObject;
    [SerializeField] private GameObject LoseObject;
    [Header("Pool Points")]
    [SerializeField] private GameObject pnt1;
    [SerializeField] private GameObject pnt2;

    private Vector3 pnt1_pos, pnt2_pos;
    private float a, b;

    private bool Below_Pool(Vector3 kid_position)
    {
        float kid_y_minus_offset = kid_position.y - 55;
        bool below_area1 = kid_position.x <= pnt1_pos.x && kid_y_minus_offset < pnt1_pos.y;
        bool below_area2 = kid_position.x <= pnt2_pos.x && kid_y_minus_offset < a * kid_position.x + b;
        bool below_area3 = kid_position.x > pnt2_pos.x && kid_y_minus_offset < pnt2_pos.y;

        return below_area1 || below_area2 || below_area3;
    }


    private void Start()
    {
        pool = GameObject.Find("Water").GetComponent<Pool>();
        pnt1_pos = pnt1.GetComponent<Transform>().position;
        pnt2_pos = pnt2.GetComponent<Transform>().position;

        a = (pnt2_pos.y - pnt1_pos.y) / (pnt2_pos.x - pnt1_pos.x);
        b = pnt1_pos.y - a * pnt1_pos.x;
    }

    private void FixedUpdate()
    {
        if (kidSpawner.kidCount == kidSpawner.maxKids && kidSpawner.kidList.Count == 0) { // Win
            WinObject.SetActive(true);
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
                LoseObject.SetActive(true);
                kidSpawner.Activate(false);
                foreach(GameObject kid in kidSpawner.kidList) kid.GetComponent<Kid>().Pause(true);
                gameObject.SetActive(false);
            }
        }
    }
}
