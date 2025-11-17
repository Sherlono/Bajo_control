using UnityEngine;

public class Factory_Block : MonoBehaviour
{
    private Vector3 _init_pos, _init_scale;
    private Color _init_color;

    void Start()
    {
        _init_pos = transform.position;
        _init_scale = transform.localScale;
        _init_color = GetComponent<SpriteRenderer>().color;
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

    /*/ Update is called once per frame
    void Update()
    {
        
    }*/
}
