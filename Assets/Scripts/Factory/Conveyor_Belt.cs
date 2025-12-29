using UnityEngine;

public class Conveyor_Belt : MonoBehaviour
{
    public Rigidbody2D Block_rb2d;
    private Vector2 Block_rb2d_position;
    [SerializeField] private Animator belt_animation;

    [SerializeField] private GameObject arrow;

    //[HideInInspector] public bool _activate_arrow;
    public float speed;
    private bool pushing = false;

    [SerializeField] private TaskAction _taskData;

    private void Turn_On(int id)
    {
        if (id == _taskData.ID)
        {
            belt_animation.enabled = true;
        }
    }

    public void Set_Highlight(int id, bool option)
    {
        if (id == _taskData.ID) arrow.SetActive(option);
    }

    private void Awake()
    {
        FactoryGameManager.onStartTask += Turn_On;
        FactoryGameManager.onHighlight += Set_Highlight;
    }

    private void OnDestroy()
    {
        FactoryGameManager.onStartTask -= Turn_On;
        FactoryGameManager.onHighlight -= Set_Highlight;
    }

    private void FixedUpdate()
    {
        if (belt_animation.enabled && pushing)
        {
            Block_rb2d_position = Block_rb2d.position;
            Block_rb2d_position += Vector2.right * speed * Time.fixedDeltaTime;
            Block_rb2d.MovePosition(Block_rb2d_position);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        pushing = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        pushing = false;
    }

}
