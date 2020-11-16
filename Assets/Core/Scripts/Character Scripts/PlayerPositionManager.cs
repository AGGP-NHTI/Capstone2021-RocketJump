using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionManager : MonoBehaviour
{
    public int nodePosition;
    public int lap;

    private void Awake()
    {
        nodePosition = 0;
    }
}
