using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Data", menuName = "ScriptableObjects/PlayerData", order = 2)]
public class PlayerInformation_SO : ScriptableObject
{
    public int playerCharacter; // 0 = spectator, 1 = dictator, 2 = chappie, 3 = sasha
    public bool isHosting;
}
