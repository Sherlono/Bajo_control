using UnityEngine;

public class Factory_Machine : MonoBehaviour
{
    private const float _curtain_speed = 0.01f;

    [Header("The Block")]
    public GameObject Block;

    [Header("Block Transformation")]
    public Vector3 transformed_scale;
    public Color transformed_color;

    [SerializeField] private GameObject curtain;
    [SerializeField] private GameObject arrow;
    [SerializeField] private SpriteRenderer LED_sr;
    private Collider2D trigger;

    [Header("Data")]
    public bool enable;
    public bool done = false;
    [HideInInspector] public bool is_open = true;
    [HideInInspector] public bool highlight;
    private bool _opening, _closing;
    private float _init_curtain_y;

    private void Open()
    {
        if (curtain.transform.position.y < _init_curtain_y + 1.5f) curtain.transform.position = new Vector3(curtain.transform.position.x, curtain.transform.position.y + _curtain_speed, curtain.transform.position.z);
        else
        {
            is_open = true;
            _opening = false;
        }
    }

    private void Close()
    {
        if (curtain.transform.position.y > _init_curtain_y - 0.1f) curtain.transform.position = new Vector3(curtain.transform.position.x, curtain.transform.position.y - _curtain_speed, curtain.transform.position.z);
        else
        {
            is_open = false;
            _closing = false;
        }
    }

    private void Toggle_Curtain()
    {
        if (is_open) _closing = true;
        else _opening = true;
    }
    
    public void Transform_Block()
    {
        Factory_Block f_block = Block.GetComponent<Factory_Block>();

        Block.transform.localScale = transformed_scale;
        Block.GetComponent<SpriteRenderer>().color = transformed_color;
        Block.transform.position = new Vector3(transform.position.x, Block.transform.position.y, Block.transform.position.z);

        f_block.saved_pos = Block.transform.position;
        f_block.saved_color = transformed_color;
        f_block.saved_scale = Block.transform.localScale;
    }

    public bool Block_is_Inside()
    {
        return trigger.bounds.Contains(new Vector3(Block.transform.position.x, Block.transform.position.y, transform.position.z));
    }

    private void Finish_Process()
    {
        Toggle_Curtain();
        LED_OFF();
        done = true;
    }

    private void Process_Block()
    {
        if (Block_is_Inside())
        {
            Toggle_Curtain();
            Invoke("Transform_Block", 2.0f);
            Invoke("Finish_Process", 4.0f);
        }
    }

    public void LED_ON()
    {
        LED_sr.color = Color.green;
    }
    public void LED_OFF()
    {
        LED_sr.color = Color.red;
    }

    public void Set_Highlight(bool option)
    {
        highlight = option;
        arrow.SetActive(option);
    }

    void Start()
    {
        trigger = GetComponent<BoxCollider2D>();

        enable = false;
        _init_curtain_y = transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enable)
        {
            if (Block_is_Inside())
            {
                enable = false;
                LED_ON();
                Invoke("Process_Block", 1.0f);
            }
        }

        if (_closing) Close();
        if (_opening) Open();
    }

}
