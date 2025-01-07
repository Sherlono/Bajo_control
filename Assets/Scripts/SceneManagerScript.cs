using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public void LoadScene(string Scene_name)
    {
        SceneManager.LoadScene(Scene_name);
    }
}
