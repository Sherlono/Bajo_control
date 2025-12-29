using TMPro;
using UnityEngine;

public class AnimatableText : MonoBehaviour
{
    public int value = 0;
    [SerializeField] private TextMeshProUGUI Text;

    void Update()
    {
        Text.text = value.ToString("F0");
    }
}
