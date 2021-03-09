using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

[RequireComponent(typeof(CharacterController))]
public class NewPC : Controller
{
	public Transform eyes;

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
	public float groundFriction = 10f;
	public float airFriction = 0.5f;

	[Header("Objects")]
	public GameObject cameraPrefab;
	public GameObject UI;


	[Header("Network Settings")]
	public bool loadPlayer = true;

	[Header("Stats")]
	public float maxVelocity = 20;

	(float x, float y) movement = (0,0);
	Vector3 externalForce = Vector3.zero;
	float downForce = 0;
	float eyePitch = 0;

	bool wasGrounded = false;

	CharacterController cc;

	GameObject localPlayer;
	GameObject localCamera;
	GameObject track;
	
	PositionManager positionManager;
	Inventory_Manager inventoryManager;

	//public GameObject startingTest;

	private void Awake()
	{
		cc = GetComponent<CharacterController>();
	}

	private void Start()
	{
;

		//giveItem(startingTest);

		if (!IsLocalPlayer)
		{
			this.enabled = false;

		}
		else 
		{

			
			setLocalPlayer();
			setCamera();
			setUI();
			
			Cursor.lockState = CursorLockMode.Locked;
		}

		if (IsHost)
		{
			setTrack();
			setPositionManager();
		}
		else if (IsClient)
		{
			loadPlayer = true;
			InvokeServerRpc(clientAddPlayer, gameObject);
		}

	}

	void Update()
    {
		//UI.GetComponent<UIManager>().sendMessage(("Speed: " + getHorizontalVelocity()), new Vector3(0,-100,0), 0.1f);

		//Debug.Log("VELOCITY: " + getVelocity());

        if(!loadPlayer) { initializePositionManager(); }

		wasGrounded = cc.isGrounded;
		
		// TODO: handle input via controller, not internally
		float lat = Input.GetAxis("Horizontal");
		float fwd = Input.GetAxis("Vertical");
		bool jump = Input.GetKey(KeyCode.Space);
		float yaw = Input.GetAxis("Mouse X");

		eyePitch -= Input.GetAxis("Mouse Y") * lookSpeed;
		eyePitch = Mathf.Clamp(eyePitch, -85, 85);


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

		Vector3 eyeRot = new Vector3(eyePitch, 0, 0);

		// movement deflection
		if (cc.isGrounded == false && (movement.x > 0.01f || movement.y > 0.01f))
		{
			float len = Mathf.Sqrt(movement.x * movement.x + movement.y * movement.y);
			movement = (movement.x - (movement.y / len) * (yaw),
				movement.y + (movement.x / len) * (yaw));

			float newlen = Mathf.Sqrt(movement.x * movement.x + movement.y * movement.y);
			movement = (movement.x / newlen * len,
				movement.y / newlen * len);
		}

		// apply rotation
		transform.localEulerAngles = bodyRot;
		eyes.transform.localEulerAngles = eyeRot;

		// apply motion
		cc.Move(externalForce * Time.deltaTime + transform.TransformDirection(new Vector3(movement.x, -downForce, movement.y) * Time.deltaTime));

		// degrade external forces
		float friction = (cc.isGrounded ? groundFriction : airFriction) * Time.deltaTime;
		int sign;

		if (externalForce.x != 0)
		{
			sign = externalForce.x < 0 ? -1 : 1;
			externalForce.x *= sign;
			externalForce.x -= friction;
			if (externalForce.x < 0) { externalForce.x = 0; }
			else { externalForce.x *= sign; }
		}

		if (externalForce.y != 0)
		{
			sign = externalForce.y < 0 ? -1 : 1;
			externalForce.y *= sign;
			externalForce.y -= friction;
			if (externalForce.y < 0) { externalForce.y = 0; }
			else { externalForce.y *= sign; }
		}

		if (externalForce.z != 0)
		{
			sign = externalForce.z < 0 ? -1 : 1;
			externalForce.z *= sign;
			externalForce.z -= friction;
			if (externalForce.z < 0) { externalForce.z = 0; }
			else { externalForce.z *= sign; }
		}

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
			eyePitch = 0;
			movement = (0, 0);
			
		}
		// TODO
	}

	public void lerpTo(Vector3 destination)
	{ 
		
	}

	public void AddForce(Vector3 force)
	{
		externalForce += force;
	}

	public float getVelocity()
	{
		return cc.velocity.magnitude;
	}
	public float getHorizontalVelocity()
	{
		Vector3 horizontalVelocity = new Vector3(cc.velocity.x, 0, cc.velocity.z);
		return horizontalVelocity.magnitude;
	}
	public float getLocalForwardVelocity()
	{
		return Mathf.Abs(eyes.InverseTransformDirection(cc.velocity).z);
	}

	public void giveItem(GameObject item)
	{
		Inventory_Manager inv = gameObject.GetComponent<Inventory_Manager>();
		if (inv)
		{
			inv.GiveExtraWeapon(item);
		}
	}


	//INITIALIZATION FUNCTIONS
	public void setUI()
	{
		UI = Instantiate(UI, localPlayer.transform);
		if (UI)
		{
			SpeedometerScript speedometer = UI.GetComponentInChildren<SpeedometerScript>();
			if (speedometer)
			{
				speedometer.player = this;
			}
		}
	}

	public void setCamera()
	{
		localCamera = Instantiate(cameraPrefab, localPlayer.transform);
		MimicTransform cameraTransform = localCamera.GetComponent<MimicTransform>();
		CameraManager manager = localCamera.GetComponent<CameraManager>();
		if (cameraTransform)
		{
			cameraTransform.target = eyes;
		}
		if (manager)
		{
			manager.player = this;
		}
	}

	public void setLocalPlayer()
	{
		localPlayer = new GameObject();
		localPlayer.name = "Local Player";
	}

	public void setPositionManager()
	{
		positionManager = track.AddComponent<PositionManager>();
		if (positionManager && track)
		{
			positionManager.track = track;
		}
	}

    public void initializePositionManager()
    {
        positionManager.updatePlayerList(gameObject);
        loadPlayer = true;
    }

	public void setTrack()
	{
		track = GameObject.Find("track");
	}

    //NETWORK FUNCTIONS

    public void updateNodePosition(PositionNodeScript node)
    {
        if (IsHost)
        {
            positionManager.updatePlayerPosition(gameObject, node.nodeNumber);
        }
        else if (IsClient)
        {
            //clientUpdateNodePosition(node, gameObject);
            InvokeServerRpc(clientUpdateNodePosition, node.nodeNumber, gameObject);
        }
    }

    [ServerRPC(RequireOwnership = false)]
	private void clientAddPlayer(GameObject player)
	{
		//positionManager.updatePlayerList(player);
		var pm = GameObject.Find("track").GetComponent<PositionManager>();
		pm.updatePlayerList(player);
	}

	[ServerRPC(RequireOwnership = false)]
	private void clientUpdateNodePosition(int nodeNumber, GameObject player)
	{
		//positionManager.updatePlayerPosition(player, nodeNumber);
		var pm = GameObject.Find("track").GetComponent<PositionManager>();
		pm.updatePlayerPosition(player, nodeNumber);
	}
}
