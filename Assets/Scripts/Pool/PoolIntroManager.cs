using UnityEngine;

public class PoolIntroManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _info_cards;
    private int card_index;

    [SerializeField] private GameObject _controlPanel;

    [SerializeField] private Setpointer setpointer;

    const float lerp_t = 0.005f;
    bool flag = false;

    private void Next_Card()
    {
        _info_cards[card_index].SetActive(false);
        card_index++;
        _info_cards[card_index].SetActive(true);
    }

    private void Check_Pressed(int gains_set)
    {
        if (gains_set == 0)
        {
            flag = true;
            Next_Card();
        }
    }

    private void Awake()
    {
        PIDPanel.onReady += Check_Pressed;
    }

    private void OnDestroy()
    {
        PIDPanel.onReady -= Check_Pressed;
    }

    void Start()
    {
        for (int i = 0; i < _info_cards.Length; i++) _info_cards[i].SetActive(i == 0);
    }

    void FixedUpdate()
    {
        if (card_index < _info_cards.Length - 1 && _info_cards[card_index].activeSelf == false)
        {
            Next_Card();
        }

        if (card_index == 2 && !flag) _controlPanel.transform.localPosition = Vector3.Lerp(_controlPanel.transform.localPosition, new Vector3(0.0f, 0.0f, _controlPanel.transform.localPosition.z), lerp_t);

    }
}
