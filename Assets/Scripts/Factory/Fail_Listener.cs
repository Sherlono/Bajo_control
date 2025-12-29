using UnityEngine;
using UnityEngine.UI;

public class Fail_Listener : MonoBehaviour
{
    public void EnableObject()
    {
        gameObject.SetActive(true);
    }

    private void Awake()
    {
        TaskAction.onFail += EnableObject;
        GetComponent<Image>().enabled = true;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        TaskAction.onFail -= EnableObject;
    }

}
