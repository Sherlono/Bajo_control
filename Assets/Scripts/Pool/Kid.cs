using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kid : MonoBehaviour
{
    private Animator kidanim;
    private Pool pool;
    private bool splashed = false;

    private float target, _y;
    private float start = 0.0f, swimLinger_t, targetIsBehind;
    private int gender;
    public int state = 0;
    public bool paused = false;
    // Start is called before the first frame update
    void Start()
    {
        pool = GameObject.Find("Water").GetComponent<Pool>();
        kidanim = GetComponent<Animator>();
        target = 190 + Random.value * 380;
        _y = transform.position.y;
        swimLinger_t = 5 + Random.value * 10;

        if(target - transform.position.x < 0){
            targetIsBehind = 1;
            transform.Rotate(0, 180, 0);
        } else {
            targetIsBehind = 0;
        }
        gender = Random.Range(0,2);
        if(gender == 0){
            kidanim.runtimeAnimatorController = Resources.Load("Animations/Boy/boy_run_0", typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        }else{
            kidanim.runtimeAnimatorController = Resources.Load("Animations/Girl/girl_run_0", typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch(state){
            case 0: // Aproaching target
                Run();
                break;
            case 1: // Target reached, starting jump, is grounded
                Jump(true);
                break;
            case 2: // During jump the air, is not grounded
                Jump(false);
                break;
            case 3: // Kid is floating
                Float();
                break;
            case 4: // Kid swims to the stairs
                Swim();
                break;
            case 5: // Kid climbs out of pool
                Climb();
                break;
            case 6: // Kid leaves
                Run();
                break;
        }
    }

    void Run()
    {
        if(start == 0) {
            start = Time.time;
            kidanim.Play("run_0");
        }

        //float error = target - transform.position.x;
        if (target - transform.position.x > 1 || target - transform.position.x < -1) {   // Running to target
            transform.position = new Vector3(transform.position.x + 0.4f - 0.8f * targetIsBehind, transform.position.y, 88);
        } else {
            start = 0;
            state++;
        }
    }

    void Jump(bool grounded)
    {
        if (grounded) {
            if (start == 0) {
                start = Time.time;
                kidanim.Play("jump_0");
            } 
            else if (Time.time - start > 0.6f) {    // Jump windup time
                start = 0;
                state++;
            }
        } else {
            if (!paused) {
                if (transform.position.y > pool.transform.position.y) {
                    if (start == 0) {
                        start = Time.time;
                    }
                    float _temp = (Time.time - start) * 5.5f;
                    transform.position = new Vector3(transform.position.x, -(9.8f * _temp * _temp) + (50.0f * _temp) + _y, 88);

                    // Feet enter water
                    if (transform.position.y < pool.transform.position.y + 33 && !splashed) {
                        GameObject Water_splash = Instantiate(Resources.Load<GameObject>("Prefabs/Pool/Water_Splash"), new Vector3(transform.position.x, transform.position.y + 15, 88), Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
                        splashed = true;
                    }
                } else {
                    start = 0;
                    state++;
                }
            }
        }
    }
    void Float()
    {
        if (start == 0) {
            start = Time.time;
            kidanim.Play("float_0");
            pool.splashlist.Add(10);    // The splash "pulse" lasts |n| updates. Add(|n|)
        }

        if (!paused) {
            transform.position = new Vector3(transform.position.x, pool.transform.position.y - 15, 88);

            if (Time.time > start + swimLinger_t) {
                target = GameObject.Find("Stairs").transform.position.x;
                if (target - transform.position.x < 0) {
                    targetIsBehind = 1;
                    transform.Rotate(0, 180, 0);
                } else {
                    targetIsBehind = 0;
                }
                start = 0;
                state++;
            }
        }
    }

    void Swim()
    {
        if (start == 0) {
            start = Time.time;
            kidanim.Play("swim_0");
        }
        if (!paused) {
            if (target - transform.position.x > 25 || target - transform.position.x < -25) {
                transform.position = new Vector3(transform.position.x + 0.15f - 0.3f * targetIsBehind, pool.transform.position.y, 88);
            } else {
                start = 0;
                state++;
            }
        }
        
    }

    void Climb()
    {
        if (start == 0){
            start = Time.time;
            kidanim.Play("climb_0");
            target = GameObject.Find("Stairs").transform.position.y + 20.0f;
            transform.position = new Vector3(GameObject.Find("Stairs").transform.position.x, transform.position.y, 88);
            pool.splashlist.Add(-40);    // The splash "pulse" lasts |n| updates. Add(|n|)
        }

        if (target - transform.position.y > 0.0f){
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.05f, 88);
        } else {
            start = 0;
            transform.position = new Vector3(transform.position.x , 310 + (Random.value * 20), 88);
            target = -120;
            state++;
        }
    }

    public void Pause(bool p)
    {
        paused = p;
    }
}
