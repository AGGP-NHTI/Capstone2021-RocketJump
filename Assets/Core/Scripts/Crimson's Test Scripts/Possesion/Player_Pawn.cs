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
	public PositionManager positionManager;

	[Header("Objects")]
	public GameObject cameraPrefab;
	public GameObject UI;

	[Header("Network Settings")]
	public bool loadPlayer = true;
    public bool initPlayer = false;
    public PlayerNetworkCenter PNC;

    [Header("Public Player Info")]
    public string playerName = PlayerInformation.playerScreenName;

	GameObject localCamera;
	GameObject track;

	GameObject localPlayer;
    private void Start()
    {

        PNC = new PlayerNetworkCenter(this);

        if (IsLocal())
        {
            setLocalPlayer();
            setCamera();
            //setUI();

            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void Update()
    {
		if (IsLocal())
		{
			if (IsServer)
			{
                if (!initPlayer)
                {
                    print("!");
                    initializePositionManager();
                }
            }
			else if (IsClient)
			{
                if(!initPlayer)
                {
                    initPlayer = true;
                    InvokeServerRpc(PNC.clientAddPlayer, gameObject);
                }
			}
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
        localPlayer = new GameObject();
        localPlayer.name = "Local Player";
    }

	public void setPositionManager()
	{
		if (track)
		{
			positionManager = track.AddComponent<PositionManager>();
			if (positionManager && track)
			{
				positionManager.track = track;
				positionManager.initPositionManager();
			}
		}
	}

	public void initializePositionManager()
	{
        initPlayer = true;
        setTrack();
        setPositionManager();
		if (positionManager)
		{
			positionManager.updatePlayerList(gameObject);
		}
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
			InvokeServerRpc(PNC.clientUpdateNodePosition, node.nodeNumber, gameObject);
		}
	}

    /*

	[ServerRPC(RequireOwnership = false)]
	private void clientAddPlayer(GameObject player)
	{
		//positionManager.updatePlayerList(player);
		//var pm = GameObject.Find("track").GetComponent<PositionManager>();
		//pm.updatePlayerList(player);
	}

	[ServerRPC(RequireOwnership = false)]
	private void clientUpdateNodePosition(int nodeNumber, GameObject player)
	{
		//positionManager.updatePlayerPosition(player, nodeNumber);
		var pm = GameObject.Find("track").GetComponent<PositionManager>();
		pm.updatePlayerPosition(player, nodeNumber);
	}

    */

    public bool IsLocal()
    {
		if (controller)
			return controller.IsLocalPlayer;
		else
			return false;
    }


}
