﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class NewPC : MonoBehaviour
{
	[Header("Character Traits")]
	public float moveSpeed = 15;
	public float jumpForce = 15;

	[Header("Input")]
	public float lookSpeed = 5;

	[Header("Physics")]
	public float gravityConstant = 9.81f;
	public float groundingForce = 5f;

	float gravForce = 0;
	float pitch = 0;

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

		pitch -= Input.GetAxis("Mouse Y") * lookSpeed;
		pitch = Mathf.Clamp(pitch, -85, 85);

		if (cc.isGrounded && gravForce > 0)
		{
			gravForce = groundingForce;
			if (jump) gravForce -= jumpForce;
		}
		gravForce += gravityConstant * Time.deltaTime;

		Vector3 bodyRot = transform.localEulerAngles;
		bodyRot.y += yaw * lookSpeed;

		Vector3 camRot = new Vector3(pitch, 0, 0);

		transform.localEulerAngles = bodyRot;
		cam.transform.localEulerAngles = camRot;
		cc.Move(transform.TransformDirection(new Vector3(lat * moveSpeed, -gravForce, fwd * moveSpeed) * Time.deltaTime));

		if (wasGrounded == true && cc.isGrounded == false)
		{
			// left the ground
			if (gravForce > 0) gravForce = 0;
		}
		else if (wasGrounded == false && cc.isGrounded == true)
		{
			// landed
		}
	}
}