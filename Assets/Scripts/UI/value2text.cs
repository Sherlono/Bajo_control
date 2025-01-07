using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class value2text : MonoBehaviour
{
    public Slider slider;
    public string text_name;
    private TextMeshPro Text;

    // Start is called before the first frame update
    void Start()
    {
        Text = GameObject.Find(text_name).GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        Text.text = slider.value.ToString("F2");
    }
}
