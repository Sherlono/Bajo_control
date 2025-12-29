using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class value2text : MonoBehaviour
{
    public Slider slider;
    [SerializeField] private string text_name;
    private TextMeshProUGUI Text;
    
    // Start is called before the first frame update
    void Start()
    {
        Text = GameObject.Find(text_name).GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        Text.text = slider.value.ToString("F2");
    }
}
