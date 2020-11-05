using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoSpin : MonoBehaviour
{
    public GameObject centerpoint;
    public GameObject offset;
    public float speed;
    public float distancex;
    public float distancey;
    Vector3 MoveDirection;
    
    void Start()
    {
        offset.transform.position = new Vector3(centerpoint.transform.position.x + distancex, centerpoint.transform.position.y + distancey, centerpoint.transform.position.z);
    }

    
    void FixedUpdate()
    {
        centerpoint.transform.Rotate(speed * Time.fixedDeltaTime * Vector3.forward);
        MoveDirection = (offset.gameObject.transform.position - transform.position).normalized;
        gameObject.transform.up = MoveDirection;
    }
}
