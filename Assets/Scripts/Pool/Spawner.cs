using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> kidList = new();
    private int t = 0;

    public int kidCount = 0, maxKids = 6;
    public int level;

    public bool Is_Done()
    {
        return kidCount == maxKids && kidList.Count == 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (kidCount < maxKids)
        {
            switch (level)
            {
                case 1:
                    if (t < 4000) t++;
                    else
                    {
                        GameObject newkid = Instantiate(Resources.Load<GameObject>("Prefabs/Pool/Kid"),
                                                        new Vector3(-120, 310 + (Random.value * 20), 0),
                                                        Quaternion.identity,
                                                        GameObject.FindGameObjectWithTag("Canvas").transform);
                        kidList.Add(newkid);
                        kidCount++;
                        t = 0;
                    }
                    break;
                case 2:
                    if (t < 5000) t++;
                    else
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            GameObject newkid = Instantiate(Resources.Load<GameObject>("Prefabs/Pool/Kid"),
                                                            new Vector3(-120 - 50*i, 310 + (Random.value * 20), 0),
                                                            Quaternion.identity,
                                                            GameObject.FindGameObjectWithTag("Canvas").transform);
                            kidList.Add(newkid);
                            kidCount++;
                        }
                        t = 0;
                    }
                    break;
            }
        }

        for (int i = 0; i < kidList.Count; i++)
        {
            if (kidList[i].GetComponent<Kid>().state == 7)
            {
                Destroy(kidList[i]);
                kidList.Remove(kidList[i]);
            }
        }
    }
}
