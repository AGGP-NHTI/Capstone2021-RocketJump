using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : NetworkedBehaviour
{
	public Transform body;
	public Transform eyes;
	public SphereCollider groundcheck;
	public Pawn ControlledPawn;

	public float movementGround;
	public float movementAir;
	public float topSpeed;
	public float jumpForce;

	
	public float decelFactorGround;

	public Vector2 mouseSensitivity = Vector2.one;
	public Vector2 pitchLimits = new Vector2(-80, 80);
	
	Vector2 input;
	float eyesPitch;
	float bodyYaw;
	bool grounded;
	bool jump;

	Vector3 surfaceNormal = Vector3.zero;

	Rigidbody rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>() ?? gameObject.AddComponent<Rigidbody>();
		Cursor.lockState = CursorLockMode.Locked;
	}
	
	void Update()
    {
		DoMouseLook();

		input = PollWASD();
		if (!jump) jump = PollJump(); // required due to timing between Update/FixedUpdate
		

		Debug.DrawRay(groundcheck.transform.position, surfaceNormal * 10f, Color.red);
    }

	private void FixedUpdate()
	{
		Vector3 forceVec;
		Vector3 flatDirection;
		float flatMagnitude;

		grounded = GroundTest();

		if (surfaceNormal == Vector3.zero)
		{
			transform.up = Vector3.Slerp(transform.up, Vector3.up, 0.1f);
		}
		else
		{
			transform.up = Vector3.Slerp(transform.up, surfaceNormal, 0.1f);
		}

		forceVec = (body.forward * input.y + body.right * input.x);
		flatDirection = rb.velocity;
		flatDirection.y = 0;
		flatMagnitude = flatDirection.magnitude;

		if (grounded)
		{
			rb.AddForce(-rb.velocity * decelFactorGround, ForceMode.Impulse);
			rb.AddForce(forceVec * movementGround, ForceMode.Impulse);
		}
		else
		{
			rb.AddForce(forceVec * movementAir, ForceMode.Force);
		}
		

		if (jump && grounded)
		{
			jump = false;
			rb.AddForce(body.up * jumpForce, ForceMode.Impulse);
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

	void DoMouseLook()
	{
		eyesPitch -= Input.GetAxisRaw("Mouse Y") * mouseSensitivity.y;
		bodyYaw += Input.GetAxisRaw("Mouse X") * mouseSensitivity.x;

		eyesPitch = Mathf.Clamp(eyesPitch, pitchLimits.x, pitchLimits.y);

		eyes.localEulerAngles = new Vector3(eyesPitch, 0, 0);
		body.localEulerAngles = new Vector3(0, bodyYaw, 0);
	}

	public bool GroundTest()
	{
		Collider[] colliders = Physics.OverlapSphere(groundcheck.transform.position, groundcheck.radius, ~(1 << LayerMask.NameToLayer("LocalPlayer")));

		surfaceNormal = Vector3.zero;
		if (colliders.Length > 0)
		{
			foreach (Collider col in colliders)
			{
				surfaceNormal += groundcheck.transform.position - col.ClosestPoint(groundcheck.transform.position);
			}
			surfaceNormal.Normalize();

			if (Vector3.Dot(surfaceNormal, Vector3.up) > 0.5f)
				return true;
			else
				return false;
		}
		return false;
	}

	public void PossesssPawn(Pawn p)
    {
		if(ControlledPawn)
        {

        }
    }
}
