using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LapManager : MonoBehaviour
{

    //A testing script

    public List<PlayerPositionManager> playerPositions;
    public bool compare = false;

    // Start is called before the first frame update
    void Start()
    {
        //playerPositions.Add(new PlayerPositionManager(new GameObject(), new PositionManager(), "test1"));
        //playerPositions.Add(new PlayerPositionManager(new GameObject(), new PositionManager(), "test2"));
        //playerPositions.Add(new PlayerPositionManager(new GameObject(), new PositionManager(), "test3"));
    }

    // Update is called once per frame
    void Update()
    {
        if(compare)
        {
            compare = false;
            comparePlayerPositions();
            
        }
    }

    public void comparePlayerPositions()
    {
        print("compare positions");

        playerPositions = (playerPositions.OrderByDescending(p => p.lap).ThenByDescending(p => p.nodePosition)).ToList();

    }

}
