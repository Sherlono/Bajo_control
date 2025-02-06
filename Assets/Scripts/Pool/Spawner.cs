using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private bool activated = false;
    [SerializeField]
    private List<GameObject> kidList = new List<GameObject>();
    private int t = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (t < 5000)
        {
            if (activated)
            {
                t++;
            }
        }
        else
        {
            GameObject newkid = Instantiate(Resources.Load<GameObject>("Prefabs/Kid"), new Vector3(-120, 310 + (Random.value * 20), -1), Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
            kidList.Add(newkid);
            t = 0;
        }
        for (int i = 0; i < kidList.Count; i++)
        {
            if (kidList[i].GetComponent<Kid>().state == 5)
            {
                Destroy(kidList[i]);
                kidList.Remove(kidList[i]);
            }
        }
    }
}
