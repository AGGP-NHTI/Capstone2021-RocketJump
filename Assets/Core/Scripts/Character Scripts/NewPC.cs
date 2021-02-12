using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class NewPC : MonoBehaviour
{
	[Header("Character Traits")]
	public float groundAcceleration = 20;
	public float groundDeceleration = 15;
	public float airAcceleration = 5;
	public float airDeceleration = 5;
	public float moveSpeed = 10;
	public float jumpForce = 15;

	[Header("Input")]
	public float lookSpeed = 5;

	[Header("Physics")]
	public float gravityConstant = 9.81f;
	public float groundingForce = 5f;

	(float x, float y) movement = (0,0);
	float downForce = 0;
	float camPitch = 0;

	bool wasGrounded = false;

	CharacterController cc;
	Camera cam;

	private void Awake()
	{
		cc = GetComponent<CharacterController>();
		cam = GetComponentInChildren<Camera>();
	}

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update()
    {
		wasGrounded = cc.isGrounded;
		

		float lat = Input.GetAxis("Horizontal");
		float fwd = Input.GetAxis("Vertical");
		bool jump = Input.GetKey(KeyCode.Space);
		float yaw = Input.GetAxis("Mouse X");

		camPitch -= Input.GetAxis("Mouse Y") * lookSpeed;
		camPitch = Mathf.Clamp(camPitch, -85, 85);


		float accel = cc.isGrounded ? groundAcceleration : airAcceleration;
		float decel = cc.isGrounded ? groundDeceleration : airDeceleration;

		// lateral movement momentum
		if (lat != 0) movement.x += lat * accel * Time.deltaTime;
		if (movement.x > 0)
		{
			if (lat > 0) movement.x -= (1 - lat) * decel * Time.deltaTime;
			else if (lat < 0) movement.x -= (-1 - lat) * decel * Time.deltaTime;
			else movement.x -= decel * Time.deltaTime;
			movement.x = Mathf.Max(movement.x, 0);
		}
		else if (movement.x < 0)
		{
			if (lat > 0) movement.x += (1 - lat) * decel * Time.deltaTime;
			else if (lat < 0) movement.x += (-1 - lat) * decel * Time.deltaTime;
			else movement.x += decel * Time.deltaTime;
			movement.x = Mathf.Min(movement.x, 0);
		}
		movement.x = Mathf.Clamp(movement.x, -moveSpeed, moveSpeed);

		// forward movement momentum
		if (fwd != 0) movement.y += fwd * accel * Time.deltaTime;
		if (movement.y > 0)
		{
			if (fwd > 0) movement.y -= (1 - fwd) * decel * Time.deltaTime;
			else if (fwd < 0) movement.y -= (-1 - fwd) * decel * Time.deltaTime;
			else movement.y -= decel * Time.deltaTime;
			movement.y = Mathf.Max(movement.y, 0);
		}
		else if (movement.y < 0)
		{
			if (fwd > 0) movement.y += (1 - fwd) * decel * Time.deltaTime;
			else if (fwd < 0) movement.y += (-1 - fwd) * decel * Time.deltaTime;
			else movement.y += decel * Time.deltaTime;
			movement.y = Mathf.Min(movement.y, 0);
		}
		movement.y = Mathf.Clamp(movement.y, -moveSpeed, moveSpeed);


		// gravity and jump
		if (cc.isGrounded && downForce > 0)
		{
			downForce = groundingForce;
			if (jump) downForce -= jumpForce;
		}
		downForce += gravityConstant * Time.deltaTime;


		// rotation
		Vector3 bodyRot = transform.localEulerAngles;
		bodyRot.y += yaw * lookSpeed;

		Vector3 camRot = new Vector3(camPitch, 0, 0);

		// ------------------------------- TODO: degrade & deflect movement based on yaw

		transform.localEulerAngles = bodyRot;
		cam.transform.localEulerAngles = camRot;
		cc.Move(transform.TransformDirection(new Vector3(movement.x, -downForce, movement.y) * Time.deltaTime));
		Debug.Log(movement);

		if (wasGrounded == true && cc.isGrounded == false)
		{
			// left the ground
			if (downForce > 0) downForce = 0;
		}
		else if (wasGrounded == false && cc.isGrounded == true)
		{
			// landed
		}
	}

	public void Teleport(Vector3 pos, bool cleanReset)
	{
		if (cleanReset)
		{
			downForce = 0;
			camPitch = 0;
			movement = (0, 0);
			
		}
		// TODO
	}
}
