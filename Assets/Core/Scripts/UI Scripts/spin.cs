using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spin : MonoBehaviour
{
    public float speed = 60f;
    public bool worldspace = false;
    public bool yaxis = false;
    public float yspeed = 60f;

    void FixedUpdate()
    {
        if (!worldspace)
        {
            transform.Rotate(speed * Time.fixedDeltaTime * Vector3.forward);
        }
        else
        {
            transform.RotateAround(gameObject.transform.position, transform.forward, speed * Time.fixedDeltaTime);

        }
        if (yaxis)
        {
            transform.RotateAround(gameObject.transform.position, transform.up, yspeed * Time.fixedDeltaTime);
        }
    }
}
