using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MouseHoldable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent MouseHold;
    public UnityEvent MouseRelease;

    public void OnPointerDown(PointerEventData eventData)
    {
        MouseHold?.Invoke();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        MouseRelease?.Invoke();
    }
}
