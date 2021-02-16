using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPan : MonoBehaviour
{
    public GameObject Target;
    //public List<GameObject> Targets = new List<GameObject>();
    Vector3 Startpos;
    public GameObject LookAt;
    public CamManager manager;
    public float speed;
    public float WithinRange = 1f;
    Vector3 MoveDirection;

    void Start()
    {
        Startpos = gameObject.transform.position; 
    }

    void FixedUpdate()
    {
        if (!IsCloseToTarget())
        {
            MoveDirection = (Target.transform.position - transform.position).normalized;

            transform.position += (Time.fixedDeltaTime * MoveDirection * speed);
            //gameObject.transform.forward = MoveDirection;
        }
        else
        {
            manager.NextCam();
        }

        if (LookAt != null)
        {
            gameObject.transform.forward = (LookAt.transform.position - transform.position).normalized;
        }
        else
        {
            gameObject.transform.forward = (Target.transform.position - transform.position).normalized;
        }
    }
    public bool IsCloseToTarget()
    {

        if (GetDistanceTo(Target.gameObject) < WithinRange)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    public float GetDistanceTo(GameObject Other)
    {
        float distanceTo = (Other.transform.position - transform.position).magnitude;

        return distanceTo;
    }

    public void EndCam()
    {
        gameObject.transform.position = Startpos;
        gameObject.SetActive(false);
    }
}
