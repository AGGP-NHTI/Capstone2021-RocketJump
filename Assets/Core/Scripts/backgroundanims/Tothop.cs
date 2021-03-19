using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tothop : MonoBehaviour
{
    public GameObject hopdistance;
    Vector3 downpos;
    Vector3 uppos;
    public float mintime, maxtime;
    float current = 0;
    public float speed = 30f;
    Vector3 MoveDirection;
    Vector3 target;
    bool goingup = false;
    

    void Start()
    {
        downpos = gameObject.transform.position;
        uppos = hopdistance.transform.position;
        randomize();
    }

    
    void FixedUpdate()
    {
        current -= Time.fixedDeltaTime;

        if (current < 0)
        {
            randomize();
            goingup = true;
           
        }
        if (goingup)
        {
            target = uppos;
            if (GetDistanceTo(uppos) <= 0.3f)
            {
                goingup = false;
            }
        }
        else
        {
            target = downpos;
        }
        MoveDirection = (target - transform.position).normalized;

        transform.position += (Time.fixedDeltaTime * MoveDirection * speed);
    }

    public void randomize()
    {
        current = Random.Range(mintime, maxtime);
    }

    public float GetDistanceTo(Vector3 Other)
    {
        float distanceTo = (Other - transform.position).magnitude;

        return distanceTo;
    }
}
