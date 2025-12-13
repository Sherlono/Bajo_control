using UnityEngine;

public class Conveyor_Belt : MonoBehaviour
{
    public Rigidbody2D Block_rb2d;
    private Vector2 Block_rb2d_position;
    [SerializeField] private Animator belt_animation;

    [SerializeField] private GameObject arrow;

    [HideInInspector] public bool _activate_arrow;
    public float speed;
    private bool pushing = false;
    private bool enable = false;

    public void Set_Enable(bool option)
    {
        enable = option;
        if (belt_animation.enabled != option)
        {
            if (option) belt_animation.enabled = true;
            else belt_animation.enabled = false;
        }
    }
    public void Set_Highlight(bool option)
    {
        _activate_arrow = option;
        arrow.SetActive(option);
    }

    private void FixedUpdate()
    {
        if (enable && pushing)
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
