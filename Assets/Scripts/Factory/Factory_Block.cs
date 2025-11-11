using UnityEngine;

public class Factory_Block : MonoBehaviour
{
    private Vector3 init_pos;

    void Start()
    {
        init_pos = transform.position;
    }

    public void Reset_Pos()
    {
        GetComponent<Rigidbody2D>().angularVelocity = 0;
        GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
        transform.position = init_pos;
        transform.rotation = Quaternion.identity;
    }

    /*/ Update is called once per frame
    void Update()
    {
        
    }*/
}
