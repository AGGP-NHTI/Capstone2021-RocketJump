using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Pawn : Pawn
{
    public Player_Movement_Controller movementControl;
    public Inventory_Manager inventoryMan;

    public bool IsLocal()
    {
        return controller.IsLocalPlayer;
    }
}
