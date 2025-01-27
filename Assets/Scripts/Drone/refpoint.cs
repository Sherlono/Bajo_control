using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class refpoint : MonoBehaviour
{
    private Collider2D col;
    public bool done;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
        done = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        GetComponent<SpriteRenderer>().color = new Color(0, 0.8f, 0, 1f);
        done = true;
    }
}
