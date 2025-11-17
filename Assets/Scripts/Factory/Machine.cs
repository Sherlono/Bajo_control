using UnityEngine;

public class Machine : MonoBehaviour
{
    private const float _curtain_speed = 0.01f;
    public GameObject Block;
    public Vector3 transformed_scale;
    public Color transformed_color;
    public GameObject curtain;

    private Collider2D trigger;
    public SpriteRenderer LED_sr;

    private float _initial_y;
    public bool enable, done = false, is_goal;
    public bool is_open = true;
    [SerializeField] private bool _opening;
    [SerializeField] private bool _closing;

    private void Open()
    {
        if (curtain.transform.position.y < _initial_y + 1.5f) curtain.transform.position = new Vector3(curtain.transform.position.x, curtain.transform.position.y + _curtain_speed, curtain.transform.position.z);
        else
        {
            is_open = true;
            _opening = false;
        }
    }

    private void Close()
    {
        if (curtain.transform.position.y > _initial_y - 0.1f) curtain.transform.position = new Vector3(curtain.transform.position.x, curtain.transform.position.y - _curtain_speed, curtain.transform.position.z);
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
        if (trigger.bounds.Contains(new Vector3(Block.transform.position.x, Block.transform.position.y, transform.position.z)))
        {
            Block.transform.localScale = transformed_scale;
            Block.GetComponent<SpriteRenderer>().color = transformed_color;
            Block.transform.position = new Vector3(transform.position.x, Block.transform.position.y, Block.transform.position.z);
        }
    }

    private void Finish_Process()
    {
        Toggle_Curtain();
        LED_OFF();
        done = true;
    }

    private void Process_Block()
    {
        Toggle_Curtain();
        Invoke("Transform_Block", 2.0f);
        Invoke("Finish_Process", 4.0f);
    }

    public void LED_ON()
    {
        LED_sr.color = Color.green;
    }
    public void LED_OFF()
    {
        LED_sr.color = Color.red;
    }

    void Start()
    {
        trigger = GetComponent<BoxCollider2D>();

        enable = false;
        _initial_y = transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enable)
        {
            if (trigger.bounds.Contains(new Vector3(Block.transform.position.x, Block.transform.position.y, transform.position.z)))
            {
                enable = false;
                Invoke("Process_Block", 1.0f);
            }
        }

        if (_closing) Close();
        if (_opening) Open();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(enable) LED_ON();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        LED_OFF();
    }
}
