using UnityEngine;

public class Factory_Machine : MonoBehaviour
{
    private const float _curtain_speed = 0.01f;

    [Header("The Block")]
    [SerializeField] private GameObject Block;
    private Factory_Block _f_block;
    private SpriteRenderer _spriteRenderer;

    [Header("Block Transformation")]
    public Vector3 transformed_scale;
    private Vector3 _target_position;
    public Color transformed_color;

    [SerializeField] private SpriteRenderer _led_sr;
    [SerializeField] private GameObject _curtain;
    [SerializeField] private GameObject _arrow;
    private Collider2D _trigger;

    [Header("Data")]
    public bool done = false;
    [HideInInspector] public bool is_open = true;
    [HideInInspector] public bool highlight;
    private bool _opening, _closing;
    private float _init_curtain_y;

    [SerializeField] private TaskAction _taskData;

    private void Open()
    {
        if (_curtain.transform.position.y < _init_curtain_y + 1.5f) _curtain.transform.position = new Vector3(_curtain.transform.position.x, _curtain.transform.position.y + _curtain_speed, _curtain.transform.position.z);
        else
        {
            is_open = true;
            _opening = false;
        }
    }

    private void Close()
    {
        if (_curtain.transform.position.y > _init_curtain_y - 0.1f) _curtain.transform.position = new Vector3(_curtain.transform.position.x, _curtain.transform.position.y - _curtain_speed, _curtain.transform.position.z);
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
    
    private void iTransform_Block()
    {
        Block.transform.localScale = transformed_scale;
        _spriteRenderer.color = transformed_color;

        _target_position.x = transform.position.x;
        _target_position.y = Block.transform.position.y;
        _target_position.z = Block.transform.position.z;

        Block.transform.position = _target_position;
        Block.transform.rotation = Quaternion.identity;

        _f_block.saved_pos = Block.transform.position;
        _f_block.saved_color = transformed_color;
        _f_block.saved_scale = Block.transform.localScale;
    }

    public void Transform_Block(int id)
    {
        if(id == _taskData.ID) iTransform_Block();
    }

    public bool Block_is_Inside()
    {
        return _trigger.bounds.Contains(new Vector3(Block.transform.position.x, Block.transform.position.y, transform.position.z));
    }

    private void Finish_Process()
    {
        Toggle_Curtain();
        LED_OFF();
        _taskData.done = true;
    }

    private void Process_Block()
    {
        if (Block_is_Inside())
        {
            Toggle_Curtain();
            Invoke("iTransform_Block", 2.0f);
            Invoke("Finish_Process", 4.0f);
        }
    }

    public void LED_ON()
    {
        _led_sr.color = Color.green;
    }
    public void LED_OFF()
    {
        _led_sr.color = Color.red;
    }

    public void Set_Highlight(int id, bool option)
    {
        if (id == _taskData.ID)
        {
            highlight = option;
            _arrow.SetActive(option);
        }
    }

    private void Begin_Task(int id)
    {
        if (_taskData.ID == id)
        {
            if (Block_is_Inside())
            {
                LED_ON();
                Invoke("Process_Block", 1.0f);
            }
            else
            {
                _taskData.fail = true;
            }
        }
    }

    private void Awake()
    {
        FactoryGameManager.onStartTask += Begin_Task;
        FactoryGameManager.onHighlight += Set_Highlight;
        FactoryGameManager.onTaskTrigger += Transform_Block;
    }

    private void OnDestroy()
    {
        FactoryGameManager.onStartTask -= Begin_Task;
        FactoryGameManager.onHighlight -= Set_Highlight;
        FactoryGameManager.onTaskTrigger -= Transform_Block;
    }

    void Start()
    {
        _trigger = GetComponent<BoxCollider2D>();
        _f_block = Block.GetComponent<Factory_Block>();
        _spriteRenderer = Block.GetComponent<SpriteRenderer>();

        _init_curtain_y = transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_closing) Close();
        if (_opening) Open();
    }

}
