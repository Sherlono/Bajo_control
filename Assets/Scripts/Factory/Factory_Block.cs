using UnityEngine;
using UnityEngine.UI;

public class Factory_Block : MonoBehaviour
{
    private Vector3 _init_pos, _init_scale;
    private Color _init_color;

    [HideInInspector] public Vector3 saved_pos;
    [HideInInspector] public Vector3 saved_scale;
    [HideInInspector] public Color saved_color;

    public Button reset_btn;

    private void Awake()
    {
        reset_btn.onClick.AddListener(Load_Saved_Block);
    }

    private void OnDestroy()
    {
        reset_btn.onClick.RemoveListener(Load_Saved_Block);
    }

    void Start()
    {
        _init_pos = transform.position;
        _init_scale = transform.localScale;
        _init_color = GetComponent<SpriteRenderer>().color;

        saved_pos = _init_pos;
        saved_scale = _init_scale;
        saved_color = _init_color;
    }

    public void Reset_Block()
    {
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        rb2d.angularVelocity = 0;
        rb2d.linearVelocity = Vector3.zero;
        transform.position = _init_pos;
        transform.rotation = Quaternion.identity;

        transform.localScale = _init_scale;
        GetComponent<SpriteRenderer>().color = _init_color;
    }

    public void Load_Saved_Block()
    {
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        rb2d.angularVelocity = 0;
        rb2d.linearVelocity = Vector3.zero;
        transform.position = saved_pos;
        transform.rotation = Quaternion.identity;

        transform.localScale = saved_scale;
        GetComponent<SpriteRenderer>().color = saved_color;
    }
}
