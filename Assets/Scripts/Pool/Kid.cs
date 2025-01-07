using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kid : MonoBehaviour
{
    [SerializeField]
    private GameObject Water_splash;
    [SerializeField]
    private Animator kidanim;
    private PID pool;
    private bool splashed = false;

    private float target, _y;
    private float start = 0.0f, swimLinger_t, targetIsBehind;
    public int state = 0;

    // Start is called before the first frame update
    void Start()
    {
        pool = GameObject.Find("Water_Surface").GetComponent<PID>();
        kidanim = GetComponent<Animator>();
        target = 190 + Random.value * 380;
        _y = transform.position.y;
        swimLinger_t = 5 + Random.value * 10;

        if(target - transform.position.x < 0)
        {
            targetIsBehind = 1;
            transform.Rotate(0, 180, 0);
        }else{
            targetIsBehind = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch(state){
            case 0: // Aproaching target
                if (target - transform.position.x > 1 || target - transform.position.x < - 1){   // Running to target
                    transform.position = new Vector3(transform.position.x + 0.8f - 1.6f * targetIsBehind, transform.position.y, transform.position.z);
                }else{
                    state++;
                }
                break;
            case 1: // Target reached, starting jump
                if (start == 0){
                    start = Time.time;
                    kidanim.Play("boy_jump_0");
                }else if (Time.time - start > 0.6f){    // Jump windup time
                    start = 0;
                    state++;
                }
                break;
            case 2: // Target reached, in the air jumping
                if (transform.position.y > pool.transform.position.y)
                {     
                    if (start == 0){
                        start = Time.time;
                    }
                    float _temp = (Time.time - start) * 5.5f;
                    transform.position = new Vector3(transform.position.x, -(9.8f * _temp * _temp) + (50.0f * _temp) + _y, 0);

                    if (transform.position.y < pool.transform.position.y + 33 && !splashed) // Feet enter water
                    {
                        Instantiate(Water_splash, new Vector3(transform.position.x, transform.position.y + 15, transform.position.z), Quaternion.identity);
                        splashed = true;
                    }
                }else{
                    start = 0;
                    state++;
                }
                break;
            case 3: // Kid is floating
                if (start == 0){
                    start = Time.time;
                    kidanim.Play("boy_float_0");
                    pool.splashlist.Add(10);    // The splash "pulse" lasts n updates. Add(n)
                }
                transform.position = new Vector3(transform.position.x, pool.transform.position.y - 15, 0);
                if(Time.time > start + swimLinger_t)
                {
                    target = GameObject.Find("Stairs").transform.position.x;
                    if(target - transform.position.x < 0){
                        targetIsBehind = 1;
                        transform.Rotate(0, 180,0);
                    }
                    else
                    {
                        targetIsBehind = 0;
                    }
                    start = 0;
                    state++;
                }
                break;
            case 4: // Kid swims to the stairs
                if (start == 0)
                {
                    start = Time.time;
                    kidanim.Play("boy_swim_0");
                }
                if (target - transform.position.x > 25 || target - transform.position.x < -25)
                {
                    transform.position = new Vector3(transform.position.x + 0.3f - 0.6f * targetIsBehind, pool.transform.position.y, transform.position.z);
                }else{
                    Destroy(this.gameObject);
                }
                break;
                //case 1:
        }
    }
}
