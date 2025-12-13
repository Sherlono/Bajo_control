using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transition_time = 1f;

    public void LoadScene(string Scene_name)
    {
        if (Scene_name == "_Restart_")
        {
            StartCoroutine(TransitionToLevel(SceneManager.GetActiveScene().name));
        }
        else
        {
            StartCoroutine(TransitionToLevel(Scene_name));
        }
    }

    IEnumerator TransitionToLevel(string Scene_name)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transition_time);

        SceneManager.LoadScene(Scene_name);
    }
}
