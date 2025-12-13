using System;
using UnityEngine;

public class TaskAction : MonoBehaviour
{
    public static event Action<int> onCompletion;
    [SerializeField] private int _taskID;

    private bool _prevDone = false;
    public bool done = false;

    void Update()
    {
        if (_prevDone != done && done) onCompletion?.Invoke(_taskID);
        _prevDone = done;
    }
}
