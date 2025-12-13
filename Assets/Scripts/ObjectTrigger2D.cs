using UnityEngine;
using System;

public class ObjectTrigger2D : MonoBehaviour
{
    private BoxCollider2D trigger;
    public GameObject referencedObject;
    private Vector3 position;

    public static event Action<int> onEnter, onExit;
    [SerializeField] private int _id;

    private bool _isInside = false;

    public bool Object_Contains()
    {
        position.x = referencedObject.transform.position.x;
        position.y = referencedObject.transform.position.y;
        position.z = transform.position.z;

        return trigger.bounds.Contains(position);
    }

    void Start()
    {
        trigger = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        bool contains = Object_Contains();
        if (!_isInside && contains)
        {
            onEnter?.Invoke(_id);
            _isInside = true;
        }
        else if (_isInside && !contains)
        {
            onExit?.Invoke(_id);
            _isInside = false;
        }
    }
}
