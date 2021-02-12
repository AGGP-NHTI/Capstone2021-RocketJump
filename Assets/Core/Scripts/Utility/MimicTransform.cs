using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicTransform : MonoBehaviour
{
	public Transform target;

    void LateUpdate()
    {
        if (target)
		{
			transform.position = target.position;
			transform.rotation = target.rotation;
		}
    }
}
