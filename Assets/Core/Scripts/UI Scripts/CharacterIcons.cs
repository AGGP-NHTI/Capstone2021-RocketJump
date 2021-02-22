using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIcons : CustomButton
{
    public GameObject endpoint;
    public GameObject bigPic;
    public GameObject backing;
    Vector3 target;

    Vector3 startpos;

    public float speed = 20f;

    Vector3 MoveDirection;

    protected override void Start()
    {
        startpos = gameObject.transform.position;
        target = startpos;
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        MoveDirection = (target - transform.position).normalized;

        transform.position += (Time.fixedDeltaTime * MoveDirection * speed * GetDistanceTo(target) * 0.5f);
    }

    public override void OnHighlight()
    {
        bigPic.SetActive(true);
        backing.SetActive(true);
        target = endpoint.transform.position;
        base.OnHighlight();
    }

    public override void OnHighlightEnd()
    {
        bigPic.SetActive(false);
        backing.SetActive(false);
        target = startpos;
        base.OnHighlightEnd();
    }

    public float GetDistanceTo(Vector3 Other)
    {
        float distanceTo = (Other - transform.position).magnitude;

        return distanceTo;
    }
}
