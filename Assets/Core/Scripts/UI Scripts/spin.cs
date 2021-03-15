using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spin : MonoBehaviour
{
    public float speed = 60f;
    public bool worldspace = false;

    void FixedUpdate()
    {
        if (!worldspace)
        {
            transform.Rotate(speed * Time.fixedDeltaTime * Vector3.forward);
        }
        else
        {
            transform.Rotate(speed * Time.fixedDeltaTime * transform.forward);
        }
    }
}
