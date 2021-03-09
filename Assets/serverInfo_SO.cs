using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Server Data", menuName = "ScriptableObjects/SeverData", order = 1)]
public class serverInfo_SO : ScriptableObject
{
    public string connectAddress;
    public string connectPort;
    public string relayAddress;
    public string relayPort;
}
