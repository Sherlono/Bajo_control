using System;
using UnityEngine;

public class DroneIntroManager : MonoBehaviour
{
    public hdrone drone;
    [SerializeField] private Transform _dronetarget;

    [SerializeField] private GameObject[] _info_cards;
    public Transform[] cam_targets;

    [SerializeField] private Animator _targetsAnimator;

    [SerializeField] private GameObject _controlPanel;
    [SerializeField] private SpriteRenderer panelLogo;
    private int card_index;

    public Camera ui_camera;
    private Vector2 _cam_target;
    public float cam_offset;
    public float wind_amplitud = 1, time_speed_factor = 1;
    const float lerp_t = 0.005f;
    bool flag = false;

    private void Move_Camera_to_Target(Vector2 target)
    {
        Vector2 target_lerp = Vector2.Lerp(ui_camera.transform.position, target, lerp_t);
        ui_camera.transform.position = new Vector3(target_lerp.x, target_lerp.y, ui_camera.transform.position.z);
    }
    public void Modify_Panel_Logo(int panelExiScreenCount)
    {
        if (panelExiScreenCount == 0)
        {
            panelLogo.sprite = Resources.Load("Graphics/drone_rotate", typeof(Sprite)) as Sprite;
            PIDPanel.Restart();
        }
    }
    private void Check_Pressed(int gains_set)
    {
        if (gains_set == 0) flag = true;
        else if (gains_set == 1) Next_Card();
    }

    private void Update_Wind()
    {
        drone.x_wind = Mathf.Cos(Time.time / (Time.fixedDeltaTime * time_speed_factor)) * wind_amplitud;
        drone.y_wind = 1.5f * Mathf.Sin(33.3f + Time.time / (Time.fixedDeltaTime * time_speed_factor * 2 / 3)) * wind_amplitud;
    }

    private void Next_Card()
    {
        _info_cards[card_index].SetActive(false);
        card_index++;
        _info_cards[card_index].SetActive(true);
    }

    private void Awake()
    {
        PIDPanel.onReady += Check_Pressed;
        PIDPanel.onExitScreen += Modify_Panel_Logo;
    }

    private void OnDestroy()
    {
        PIDPanel.onReady -= Check_Pressed;
        PIDPanel.onExitScreen -= Modify_Panel_Logo;
    }

    void Start()
    {
        for (int i = 0; i < _info_cards.Length; i++) _info_cards[i].SetActive(i == 0);

        drone.targetpoint = _dronetarget.position;

        drone.Power = true;
        drone.Efficiency = 1f;
    }

    void FixedUpdate()
    {
        Update_Wind();

        if (card_index < _info_cards.Length - 1 && _info_cards[card_index].activeSelf == false)
        {
            if (card_index == 0)
            {
                Next_Card();
            }
            else
            {
                Next_Card();
                _cam_target = new Vector2(cam_targets[card_index].position.x + cam_offset, ui_camera.transform.position.y);
            }
        }

        if (card_index == 3 && !flag) _controlPanel.transform.localPosition = Vector3.Lerp(_controlPanel.transform.localPosition, new Vector3(0.0f, 0.0f, _controlPanel.transform.localPosition.z), lerp_t);

        if (card_index == 4) _targetsAnimator.enabled = true;

        _cam_target = new Vector2(cam_targets[card_index].position.x + cam_offset, ui_camera.transform.position.y);

        Move_Camera_to_Target(_cam_target);
    }

}
