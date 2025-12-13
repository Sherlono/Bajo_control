using UnityEngine;
using UnityEngine.UI;

public class ImageEnableOnTrigger : MonoBehaviour
{
    [SerializeField] private int objectID;

    public void EnableObject(int id)
    {
        if (objectID == id) gameObject.SetActive(true);
    }

    private void Awake()
    {
        ObjectTrigger2D.onEnter += EnableObject;
        GetComponent<Image>().enabled = true;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        ObjectTrigger2D.onEnter -= EnableObject;
    }

}
