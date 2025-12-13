using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterOrganizer : MonoBehaviour
{
    public List<GameObject> waters = new List<GameObject>();
    [Range(70.0f, 80.0f)]
    public float _spacing;

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < waters.Count; i++)
        {
            waters[i].transform.localPosition = new Vector3(waters[0].transform.localPosition.x + (_spacing * i), waters[0].transform.localPosition.y, waters[0].transform.localPosition.z);
        }
    }
}
