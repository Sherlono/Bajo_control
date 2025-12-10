using UnityEngine;

public class Fuel_Meter : MonoBehaviour
{
    private Transform bar_transform;
    private hdrone drone;

    private float _max_fuel, _ratio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bar_transform = transform.GetChild(0).transform;
        Transform ceiling_pos = transform.GetChild(1).transform;
        Transform floor_pos = transform.GetChild(2).transform;

        drone = GameObject.FindGameObjectWithTag("Player").GetComponent<hdrone>();

        _max_fuel = drone.fuel;
        _ratio = (floor_pos.localPosition.y - ceiling_pos.localPosition.y) / _max_fuel;
    }

    // Update is called once per frame
    void Update()
    {
        float y_pos = (_max_fuel - drone.fuel)*_ratio;
        bar_transform.localPosition = new Vector3(0, y_pos, bar_transform.localPosition.z);
    }
}
