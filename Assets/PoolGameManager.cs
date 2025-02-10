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
    private Vector3 centerPoint;

    private void Start()
    {
        pool = GameObject.Find("Water_Surface").GetComponent<PID>();
        kidSpawner = GameObject.Find("KidSpawner").GetComponent<Spawner>();
        setpointer = GameObject.Find("Setpointer").GetComponent<Setpointer>();

        WinObject = GameObject.Find("WinSplash");
        LoseObject = GameObject.Find("FailSplash");

        centerPoint = GameObject.Find("centerPoint").transform.localPosition;
    }

    private void Update()
    {
        if (setpointer.state == 1)
        {
            if (pool.h > setpointer.value - 0.1f)
            {
                kidSpawner.Activate(true);
            }
        }
        if (pool.IsPaused())
        {
            LoseObject.transform.localPosition = new Vector3(0, centerPoint.y, transform.localPosition.z);
            kidSpawner.Activate(false);
        }
    }
}
