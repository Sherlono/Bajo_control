using TMPro;
using UnityEngine;

public class TargetCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text;
    [SerializeField] private int max_points;
    void Modify_Count(int value)
    {
        int num = max_points - value;
        if (num == 0) gameObject.SetActive(false);
        Text.text = num.ToString();
    }

    private void Awake()
    {
        PointCreator.onCreatePoint += Modify_Count;
    }
    private void OnDestroy()
    {
        PointCreator.onCreatePoint -= Modify_Count;
    }
    
    void Start()
    {
        Text.text = max_points.ToString();
    }
}
