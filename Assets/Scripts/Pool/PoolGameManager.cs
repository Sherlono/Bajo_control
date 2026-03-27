using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PoolGameManager : MonoBehaviour
{
    public static PoolGameManager instance;

    public static event Action onLose;

    [SerializeField] private AudioClip splashSFX;

    [HideInInspector] public Pool pool;
    [Header ("Game Objects")]
    [SerializeField] private Spawner kidSpawner;
    [SerializeField] private Setpointer setpointer;

    [Header("Splashes")]
    [SerializeField] private GameObject WinObject;
    [SerializeField] private GameObject LoseObject;
    [Header("Pool Points")]
    [SerializeField] private GameObject pnt1;
    [SerializeField] private GameObject pnt2;

    private Vector3 pnt1_pos, pnt2_pos;
    private float _a, _b;

    public bool Below_Pool(Vector3 kid_position, float feet_y)
    {
        float kid_y_minus_offset = kid_position.y - 55;
        bool below_area1 = kid_position.x <= pnt1_pos.x && feet_y < pnt1_pos.y;
        bool below_area2 = kid_position.x <= pnt2_pos.x && feet_y < _a * kid_position.x + _b;
        bool below_area3 = kid_position.x > pnt2_pos.x && feet_y < pnt2_pos.y;

        return below_area1 || below_area2 || below_area3;
    }

    private void Awake()
    {
        instance = this;

        if (instance != null && instance != this)
        {
            Destroy(this);
        }

    }

    private void Start()
    {
        SoundFXManager.instance.PlaySoundEffectClip(splashSFX, transform, 1f, 1f, true);

        pool = GameObject.Find("Water").GetComponent<Pool>();
        pnt1_pos = pnt1.GetComponent<Transform>().position;
        pnt2_pos = pnt2.GetComponent<Transform>().position;

        _a = (pnt2_pos.y - pnt1_pos.y) / (pnt2_pos.x - pnt1_pos.x);
        _b = pnt1_pos.y - _a * pnt1_pos.x;
    }

    private void FixedUpdate()
    {
        if (kidSpawner.Is_Done()) // Win
        {
            WinObject.SetActive(true);
            WinObject.GetComponent<Image>().enabled = true;
            gameObject.SetActive(false);
        }
        else
        {
            if (setpointer.state == 1)  // Simulation start
            {
                if (pool.controller.h > setpointer.value - 0.08f)   // Setpoint "reached"
                {
                    kidSpawner.gameObject.SetActive(true);
                }

                if (pool.IsPaused())    // Fail
                {
                    LoseObject.SetActive(true);
                    LoseObject.GetComponent<Image>().enabled = true;
                    kidSpawner.gameObject.SetActive(false);
                    onLose?.Invoke();
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
