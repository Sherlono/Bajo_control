using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject kid;
    public bool activated = false;
    private int t = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (t < 1000)
        {
            if (activated)
            {
                t++;
            }
        }
        else
        {
            Instantiate(kid, new Vector3(-120, 310 + (Random.value * 20), -1), Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
            t = 0;
        }

    }
}
