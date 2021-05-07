using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class Player_Pawn : Pawn
{
    public Player_Movement_Controller movementControl;
    public Inventory_Manager inventoryMan;
	public UIManager UIMan;
	public Renderer playerRenderer;
	public Transform eyeStabilizer;
	public Transform eyes;
	public Transform handRight;
	public Transform handLeft;
	//public PositionManager positionManager;

	[Header("Objects")]
	public GameObject cameraPrefab;
	public GameObject UI;

	[Header("Network Settings")]
	public bool loadPlayer = true;
    //public bool initPlayer = false;
	//public bool PNCEnabled = true;
    //public PlayerNetworkCenter PNC;

    [Header("Public Player Info")]
    //public string playerName;

	GameObject localCamera;
	GameObject track;
	GameObject localPlayer;
    private void Start()
    {
		if (IsOwner)
		{
			StartCoroutine(waitForSetupLocalPlayer());

		}
		else
		{
			gameObject.name = gameObject.name + "_Remote_" + OwnerClientId;
		}
    }

	IEnumerator waitForSetupLocalPlayer()
	{

		yield return new WaitUntil(() => IsLocal());
		gameObject.name = gameObject.name + "_Local_" + OwnerClientId;
		setLocalPlayer();
		setCamera();
        setUI();
		playerRenderer.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        setAudioManager();
	}

	
    public void setUI()
	{
		UI = Instantiate(UI, localPlayer.transform);
		if (UI.TryGetComponent(out UIManager manager))
		{
			UIMan = manager;
			UIMan.playerMovement = movementControl;
            controller.plrCntrl.PNC.UI_manager = manager;

            if(IsHost)
            {
                UIMan.isHost = true;
            }
		}
		
	}

	//Move to player_pawn class
	public void setCamera()
	{
		localCamera = Instantiate(cameraPrefab, localPlayer.transform);
		MimicTransform cameraTransform = localCamera.GetComponent<MimicTransform>();
		CameraManager manager = localCamera.GetComponent<CameraManager>();
		if (cameraTransform)
		{
			cameraTransform.target = eyes;

            //inventoryMan.projectileSpawn = cameraTransform.transform;
		}
		if (manager)
		{
			manager.player = movementControl;
		}
	}

	public void setLocalPlayer()
    {
        localPlayer = new GameObject("Local Player");
    }
	public void setAudioManager()
	{
		if (!localPlayer) { return; }
	}
    public bool IsLocal()
    {
		if (controller)
		{
			return controller.IsLocalPlayer;
		}
		else
		{
			if (OwnerClientId == this.OwnerClientId)
			{
				Debug.LogWarning($"Controller does not exist for the client: {OwnerClientId}");
			}
			return false;
		}
    }

    public override void OnGainedOwnership()
    {
        base.OnGainedOwnership();

		
    }


	void sendEyeRotation()
	{
		InvokeServerRpc(serverSetEyeRotation,eyes.transform.rotation, this.NetworkId);
	}
	[ServerRPC(RequireOwnership = false)]
	public void serverSetEyeRotation(Quaternion rot, ulong pawnID)
	{
		InvokeClientRpcOnEveryone(clientUpdateEyeRotation, rot, pawnID);
	}

	[ClientRPC]
	public void clientUpdateEyeRotation(Quaternion rot, ulong pawnID)
	{
		NetworkedObject netObj = GetNetworkedObject(pawnID);
		if(netObj && netObj.gameObject.TryGetComponent(out Player_Pawn pawn))
		{
			if (!pawn.IsLocal())
			{
				pawn.eyes.rotation = rot;
			}
		}

	}



}
