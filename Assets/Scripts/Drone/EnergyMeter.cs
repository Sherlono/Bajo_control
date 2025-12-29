using UnityEngine;

public class EnergyMeter : MonoBehaviour
{
    private Transform bar_transform;
    private hdrone drone;

    private float _max_energy, _ratio;

    void Start()
    {
        drone = GameObject.FindGameObjectWithTag("Player").GetComponent<hdrone>();

        bar_transform = transform.GetChild(0).transform;
        Transform ceiling_pos = transform.GetChild(1).transform;
        Transform floor_pos = transform.GetChild(2).transform;

        _max_energy = drone.battery;
        _ratio = (floor_pos.localPosition.y - ceiling_pos.localPosition.y) / _max_energy;
    }

    void Update()
    {
        float y_pos = (_max_energy - drone.battery) *_ratio;
        bar_transform.localPosition = new Vector3(0, y_pos, bar_transform.localPosition.z);
    }
}
