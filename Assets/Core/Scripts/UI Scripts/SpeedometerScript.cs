using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedometerScript : MonoBehaviour
{
    //Obviously the art aspects of the speedometer still need to be made, however the actual functionality is already made with this script
    //The current needle is just a placeholder

    public Image speedometer; // the actual image for the speedometer needle
    public Image background;

    private Transform speed_trans; // the images transform

    private float max_rotation = 190; // the maximum rotation the needle can have (max speed)
    private float min_rotation = 0; // the minumum rotation the needle can have (no speed)

    public float speed; // current speed
    public float maxSpeed; // max speed;
    public float speedPerc; // the percentage of speed on its way to max speed;

    float velocity;

    public PlayerController player;

    private bool playerLoaded = false;

    Rigidbody rb;

    // ((value - min) / (max - min)) * 100 = percentage;
    // -((min * perc) / 100) + min + ((max * perc) / 100) = value;

    void Start()
    {
        speed_trans = speedometer.transform;
        
        player = gameObject.transform.root.GetComponent<PlayerController>();
        rb = player.GetComponent<Rigidbody>();

        if(!player)
        {
            Debug.Log("Player could not be found; " + gameObject.name);
        }
    }
    
    void Update()
    {
        if(player == null) { return; }
        else
        {
            if(!playerLoaded)
            {
                maxSpeed = player.topSpeed;
            }
        }
        
        speed = rb.velocity.magnitude;

        //speedPerc = ((speed - 0) / (maxSpeed - 0)) * 100; // get the speed percentage
        speedPerc = (speed / maxSpeed) * 100; // get the speed percentage

        if(speedPerc > 100) { speedPerc = 100; } // limit testing
        if(speedPerc < 0) { speedPerc = 0; } // limit testing

        speed_trans.rotation = Quaternion.Euler(new Vector3(180, 0, (((min_rotation * speedPerc) / 100) + min_rotation + ((max_rotation * speedPerc) / 100)))); // disgusting math

        background.fillAmount = Mathf.SmoothDamp(background.fillAmount, player.GetSpeedometer(), ref velocity, 0.1f);
    }
}
