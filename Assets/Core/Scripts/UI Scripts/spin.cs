using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spin : MonoBehaviour
{
    public float speed = 60f;
    
    void FixedUpdate()
    {
        transform.Rotate(speed * Time.fixedDeltaTime * Vector3.forward);
    }
}
