using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatePrefab : MonoBehaviour
{
    public void AddObject(string option)
    {
        switch (option)
        {
            case "Proportional":
                Instantiate(Resources.Load<GameObject>("Prefabs/P_InfoCard"), GameObject.Find("centerPoint").transform.position, Quaternion.identity, GameObject.FindGameObjectWithTag("UI").transform);
                break;
            case "Integrative":
                Instantiate(Resources.Load<GameObject>("Prefabs/I_InfoCard"), GameObject.Find("centerPoint").transform.position, Quaternion.identity, GameObject.FindGameObjectWithTag("UI").transform);
                break;
            case "Derivative":
                Instantiate(Resources.Load<GameObject>("Prefabs/D_InfoCard"), GameObject.Find("centerPoint").transform.position, Quaternion.identity, GameObject.FindGameObjectWithTag("UI").transform);
                break;
        }
        
    }
}
