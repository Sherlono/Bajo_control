using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatePrefab : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    public Vector3 position;
    [SerializeField] private Transform _optionalTransform;

    public void AddObject(string parent)
    {
        Instantiate(prefab, _optionalTransform ? _optionalTransform.position : position, Quaternion.identity, GameObject.FindGameObjectWithTag(parent).transform);
    }
}
