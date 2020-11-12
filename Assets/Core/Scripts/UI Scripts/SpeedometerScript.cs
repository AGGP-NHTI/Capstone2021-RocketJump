using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedometerScript : MonoBehaviour
{

    //Obviously the art aspects of the speedometer still need to be made, however the actual functionality is already made with this script
    //The current needle is just a placeholder

    public Image speedometer; // the actual image for the speedometer needle
    private Transform speed_trans; // the images transform
    private float max_rotation; // the maximum rotation the needle can have (max speed)
    private float min_rotation; // the minumum rotation the needle can have (no speed)
    public PlayerController player;

    //FOR TESTING (until actual player is hooked up)//
    public float speed; // current speed
    public float maxSpeed; // max speed;
    // Once the actual player is ready to be hooked up, the "speed" and "maxSpeed" can probably just be retied to the respective variables in the player script

    public float speedPerc; // the percentage of speed on its way to max speed;

    // ((value - min) / (max - min)) * 100 = percentage;
    // -((min * perc) / 100) + min + ((max * perc) / 100) = value;

    void Start()
    {
        speed_trans = speedometer.transform;
        max_rotation = 150; // dont mess with
        min_rotation = 0; // leave at 0
        speed = 0; // starting speed, 0 as default. It's for testing purposes
        maxSpeed = 200; //  max speed, mess with if you like. Its for testing purposes
    }

    
    void Update()
    {
        speedPerc = ((speed - 0) / (maxSpeed - 0)) * 100; // get the speed percentage

        if(speedPerc > 100) { speedPerc = 100; } // limit testing
        if(speedPerc < 0) { speedPerc = 0; } // limit testing

        speed_trans.rotation = Quaternion.Euler(new Vector3(180, 0, (((min_rotation * speedPerc) / 100) + min_rotation + ((max_rotation * speedPerc) / 100)))); // disgusting math
    }
}
