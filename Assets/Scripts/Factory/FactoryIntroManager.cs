using jv;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class FactoryIntroManager : MonoBehaviour
{
    public GameObject _scorboy_object;
    [SerializeField] private Scorboy_Controller _scorboy_controller;
    [SerializeField] private Factory_Machine _machine;
    [SerializeField] private Factory_Block _block;

    [SerializeField] private GameObject[] _info_cards;
    [SerializeField] private GameObject[] _extra_info;

    [SerializeField] private Button _next_arm_btn;

    public Transform[] cam_targets;
    public int card_index;

    public Camera ui_camera;
    private Vector2 _cam_target;
    public float cam_offset;
    const float lerp_t = 0.01f;

    private void Move_Camera_to_Target(Vector2 target)
    {
        Vector2 target_lerp = Vector2.Lerp(ui_camera.transform.position, target, lerp_t);
        ui_camera.transform.position = new Vector3(target_lerp.x, target_lerp.y, ui_camera.transform.position.z);
    }

    private void Next_Arm_btn()
    {
        card_index++;
        _info_cards[card_index].SetActive(true);
        _scorboy_controller.Freeze_Scorboy();
        _cam_target = new Vector2(cam_targets[card_index].position.x + cam_offset, ui_camera.transform.position.y);
    }

    void Start()
    {
        _next_arm_btn.onClick.AddListener(Next_Arm_btn);

        _extra_info[0].SetActive(false);
        for (int i = 0; i < _info_cards.Length; i++) _info_cards[i].SetActive(i == 0);
        _scorboy_controller.Set_Scorboy(_scorboy_object);

        _cam_target = new Vector2(cam_targets[0].position.x + cam_offset, ui_camera.transform.position.y);
    }

    private void OnDestroy()
    {
        _next_arm_btn.onClick.RemoveListener(Next_Arm_btn);
    }

    void FixedUpdate()
    {
        if (card_index < _info_cards.Length - 1 && _info_cards[card_index].activeSelf == false)
        {
            if (card_index == 0)
            {
                _extra_info[0].SetActive(true);
                _info_cards[card_index].SetActive(false);
                card_index++;
                _info_cards[card_index].SetActive(true);
            }
            else if (card_index == 1)
            {
                _extra_info[0].SetActive(false);
            }
            else
            {
                _machine.Set_Highlight(card_index == 2);
                _info_cards[card_index].SetActive(false);
                card_index++;
                _info_cards[card_index].SetActive(true);
                _cam_target = new Vector2(cam_targets[card_index].position.x + cam_offset, ui_camera.transform.position.y);
            }

        }

        if (card_index == 1)
        {
            _scorboy_controller.transform.localPosition = Vector2.Lerp(_scorboy_controller.transform.localPosition, new Vector2(_scorboy_controller.transform.localPosition.x, -23.7f), lerp_t);
            _cam_target = new Vector2(_scorboy_object.transform.GetChild(0).transform.position.x + cam_offset, ui_camera.transform.position.y);
        }
        else _scorboy_controller.transform.localPosition = Vector2.Lerp(_scorboy_controller.transform.localPosition, new Vector2(_scorboy_controller.transform.localPosition.x, -630.0f), lerp_t);

        Move_Camera_to_Target(_cam_target);
    }
}
