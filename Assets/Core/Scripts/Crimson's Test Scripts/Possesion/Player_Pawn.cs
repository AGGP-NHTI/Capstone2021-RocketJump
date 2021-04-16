using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class Player_Pawn : Pawn
{
    public Player_Movement_Controller movementControl;
    public Inventory_Manager inventoryMan;
	public Transform eyes;
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

        //playerName = PlayerInformation.playerScreenName;

        //PNC = new PlayerNetworkCenter(this);
        //PNC.enabled = PNCEnabled;

        if (IsLocal())
        {
            setLocalPlayer();
            setCamera();
            //setUI();

            Cursor.lockState = CursorLockMode.Locked;
        }

        //setupPNC();
    }

    /*
    private void setupPNC()
    {
        if (IsLocal())
        {
            if (IsServer)
            {
                if (PNC.enabled)
                {
                    PNC.initHost();
                }
                
            }
            else if (IsClient)
            {
                if (PNC.enabled)
                {
                    PNC.initClient();
                }
                
            }
        }
    }
    */

    private void Update()
    {
		if (IsServer)
		{
			testVar++;
		}
	}

    public void setUI()
	{
		UI = Instantiate(UI, localPlayer.transform);
		if (UI)
		{
			SpeedometerScript speedometer = UI.GetComponentInChildren<SpeedometerScript>();
			if (speedometer)
			{
				speedometer.player = movementControl;
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

    public bool IsLocal()
    {
		if (controller)
		{
			return controller.IsLocalPlayer;
		}
		else
		{
			Debug.LogWarning($"Controller does not exist for the client: {OwnerClientId}");
			return false;
		}
    }

    public override void OnGainedOwnership()
    {
        base.OnGainedOwnership();

		
    }

}
