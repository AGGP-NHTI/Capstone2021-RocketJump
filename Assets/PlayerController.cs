using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public SphereCollider groundcheck;
	public float movementGround;
	public float movementAir;
	public float topSpeed;
	public float jumpForce;

	Rigidbody rb;
	Vector2 input;
	bool grounded;
	bool jump;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>() ?? gameObject.AddComponent<Rigidbody>();
	}
	
	void Update()
    {
		input = PollWASD();
		if (!jump) jump = PollJump(); // required due to timing between Update/FixedUpdate
		grounded = IsGrounded();
    }

	private void FixedUpdate()
	{
		Vector3 forceVec = (transform.forward * input.y + transform.right * input.x);
		Vector3 flatDirection = rb.velocity;
		flatDirection.y = 0;
		float flatMagnitude = flatDirection.magnitude;

		
		
		if (Vector3.Dot(forceVec, flatDirection) > 0)
		{
			rb.AddForce(Mathf.Max(0, topSpeed - flatMagnitude) * (forceVec * movementGround));

		}
		else
		{
			rb.AddForce(forceVec * movementGround);
		}
		
		if (jump && grounded)
		{
			jump = false;
			rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
		}
	}

	public Vector2 PollWASD()
	{
		Vector2 vec = Vector3.zero;
		bool left = Input.GetKey(KeyCode.A);
		bool right = Input.GetKey(KeyCode.D);
		bool fwd = Input.GetKey(KeyCode.W);
		bool back = Input.GetKey(KeyCode.S);
		if (left != right)
		{
			if (left)	{vec.x = -1;}
			else		{vec.x = 1;}
		}
		if (fwd != back)
		{
			if (back)	{ vec.y = -1; }
			else		{ vec.y = 1; }
		}
		return vec;
	}

	public bool PollJump()
	{
		return Input.GetKeyDown(KeyCode.Space);
	}

	public bool IsGrounded()
	{
		return Physics.OverlapSphere(groundcheck.transform.position, groundcheck.radius, ~(1 << LayerMask.NameToLayer("LocalPlayer"))).Length > 0;
	}
}
