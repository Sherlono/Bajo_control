using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    private PID pool;
    private Spawner kidSpawner;
    private Setpointer setpointer;
    private GameObject WinObject, LoseObject;
    private Transform pnt1, pnt2;
    private Vector3 centerPoint;

    private float a, b;

    private void Start()
    {
        pool = GameObject.Find("Water_Surface").GetComponent<PID>();
        kidSpawner = GameObject.Find("KidSpawner").GetComponent<Spawner>();
        setpointer = GameObject.Find("Setpointer").GetComponent<Setpointer>();
        pnt1 = GameObject.Find("p1 mark").GetComponent<Transform>();
        pnt2 = GameObject.Find("p2 mark").GetComponent<Transform>();

        a = (pnt2.position.y - pnt1.position.y) / (pnt2.position.x - pnt1.position.x);
        b = pnt1.position.y - a * pnt1.position.x;

        WinObject = GameObject.Find("WinSplash");
        LoseObject = GameObject.Find("FailSplash");

        centerPoint = GameObject.Find("centerPoint").transform.localPosition;
    }

    private void Update()
    {
        if (kidSpawner.kidCount == kidSpawner.maxKids && kidSpawner.kidList.Count == 0) { // Win
            WinObject.transform.localPosition = new Vector3(0, centerPoint.y, transform.localPosition.z);
        } else {
            if (setpointer.state == 1) {  // Simulation start
                if (pool.h > setpointer.value - 0.1f) {   // Setpoint "reached"
                    kidSpawner.Activate(true);
                }

                foreach(GameObject kid in kidSpawner.kidList) {
                    if (kid.transform.position.x <= pnt1.position.x) {
                        if (kid.transform.position.y - 55 < pnt1.position.y) {
                            pool.Pause(true);
                            Vector3 kid_position = kid.GetComponent<Transform>().position;
                            GameObject Water_splash = Instantiate(Resources.Load<GameObject>("Prefabs/hurt"), new Vector3(kid_position.x, kid_position.y - 55, kid_position.z), Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
                        }
                    }else if (kid.transform.position.x <= pnt2.position.x)
                    {
                        if (kid.transform.position.y - 55 < a * kid.transform.position.x + b) {
                            pool.Pause(true);
                            Vector3 kid_position = kid.GetComponent<Transform>().position;
                            GameObject Water_splash = Instantiate(Resources.Load<GameObject>("Prefabs/hurt"), new Vector3(kid_position.x, kid_position.y - 55, kid_position.z), Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
                        }
                    } else {
                        if (kid.transform.position.y - 55 < pnt2.position.y) {
                            pool.Pause(true);
                            Vector3 kid_position = kid.GetComponent<Transform>().position;
                            GameObject Water_splash = Instantiate(Resources.Load<GameObject>("Prefabs/hurt"), new Vector3(kid_position.x, kid_position.y - 55, kid_position.z), Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
                        }
                    }
                }
            }
            if (pool.IsPaused()) {    // Fail
                LoseObject.transform.localPosition = new Vector3(0, centerPoint.y, transform.localPosition.z);
                kidSpawner.Activate(false);
                foreach(GameObject kid in kidSpawner.kidList) {
                    kid.GetComponent<Kid>().Pause(true);
                }
                gameObject.SetActive(false);
            }
        }
    }
}
