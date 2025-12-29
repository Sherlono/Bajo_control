using System;
using UnityEngine;

[System.Serializable]
public class TaskAction : MonoBehaviour
{
    public static event Action<int> onCompletion;
    public static event Action onFail;

    [SerializeField] private int _taskId;

    public int ID { get { return _taskId; } }

    [HideInInspector] public bool done = false;
    [HideInInspector] public bool fail = false;
    private bool _prevDone = false;
    private bool _prevFail = false;

    void Update()
    {
        if (done && _prevDone != done) onCompletion?.Invoke(_taskId);
        if (fail && _prevFail != fail) onFail?.Invoke();
        _prevDone = done;
        _prevFail = fail;
    }
}
