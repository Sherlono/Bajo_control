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
        FactoryGameManager.LoseAction += EnableObject;
        GetComponent<Image>().enabled = true;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        FactoryGameManager.LoseAction -= EnableObject;
    }

}
