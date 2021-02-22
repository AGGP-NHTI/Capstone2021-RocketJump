using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [HideInInspector]
    public NewPC player;

    [Tooltip("The FOV when the player is close to a speed of zero")]
    [Range(60, 120)]
    public float minFOV;

    [Tooltip("How close the speed needs to be to zero to set the FOV to minimum.")]
    [Range(0, 5)]
    public float minVelocityThreshhold;

    [Tooltip("The maximum FOV for when the player is not in Super Speed.")]
    [Range(60, 120)]
    public float maxFOV;

    [Tooltip("The FOV set when in Super Speed.")]
    [Range(60, 120)]
    public float superSpeedFOV;

    [Tooltip("How close the speed needs to be to the max to be considered in Super Speed.")]
    [Range(0, 5)]
    public float maxVelocityThreshhold;

    
    [Tooltip("The Rate at wich the FOV will shift.")]
    [Range(1, 10)]
    public float FOVChangeSpeed = 5f;

    [Tooltip("Wait this long after entering super speed to update the FOV.")]
    [Range(0, 5)]
    public float timeNeededToBeInSuperSpeed = 1f;

    [Tooltip("Wait this long after leaving super speed to update the FOV.")]
    [Range(0,5)]
    public float timeNeededToBeOutOfSuperSpeed = 0.75f;

    //the target FOV to gradually move toward
    float wantedFOV;

    //Tells if the speed of the player is in its super speed range
    // IE if the speed is withing maxVelocityThreshhold of the max speed
    bool inSuperSpeed = false;

    //tells how much time in and out of superspeed
    float timeOutOfSuperSpeed = 0f;
    float timeInSuperSpeed = 0f;
    
    Camera cam;

    private void Start()
    {
        

        cam = gameObject.GetComponent<Camera>() ?? gameObject.AddComponent<Camera>();

        cam.fieldOfView = minFOV;
    }
    private void Update()
    {
        
        SetSuperSpeed();
        Debug.Log("IN SUPER SPEED: " + inSuperSpeed + " for: " + timeInSuperSpeed);
        SetWantedFOV();
        UpdateFOV();
    }

    void SetWantedFOV()
    {

        

        if (player.getHorizontalVelocity() <= minVelocityThreshhold)
        {
            wantedFOV = minFOV;
        }
        else 
        {
            float fovDifference = maxFOV - minFOV;
            float percentSpeed = player.getHorizontalVelocity() / player.maxVelocity;
            //Debug.Log(player.getHorizontalVelocity() +" / " + player.maxVelocity);
            //Debug.Log("Percent: " + percentSpeed);

            float FOVVal = minFOV + (percentSpeed * fovDifference);

             

            if (timeInSuperSpeed > timeNeededToBeInSuperSpeed)
            {
                FOVVal = superSpeedFOV;
            }
            else if (FOVVal < minFOV)
            {
                FOVVal = minFOV;
            }


            wantedFOV = FOVVal;            
        }
    }

    void SetSuperSpeed()
    {
        inSuperSpeed = InSuperSpeed();

        if (inSuperSpeed)
        {
            //timeOutOfSuperSpeed = 0f;
            timeInSuperSpeed += Time.deltaTime;

            if (timeInSuperSpeed > timeNeededToBeInSuperSpeed)
            {
                timeOutOfSuperSpeed = 0f;
            }
        }
        else
        {
            
            timeOutOfSuperSpeed += Time.deltaTime;

            if (timeOutOfSuperSpeed > timeNeededToBeOutOfSuperSpeed)
            {
                timeInSuperSpeed = 0;
            }
            else
            {
                timeInSuperSpeed += Time.deltaTime;
            }
        }
    }

    void UpdateFOV()
    {
        //if fov withing 1 value 
        if (Mathf.Abs(cam.fieldOfView - wantedFOV) > 1)
        {

            // difference of the wantedFOV and the actual FOV * change speed * deltaTime
            float changeAmnt = Mathf.Abs(cam.fieldOfView - wantedFOV) * FOVChangeSpeed * Time.deltaTime;
            if (cam.fieldOfView < wantedFOV)
            {
                cam.fieldOfView += changeAmnt;
            }
            else if(timeOutOfSuperSpeed > timeNeededToBeOutOfSuperSpeed) // EXITING SUPER SPEED MODE if been out of it for certain time
            {
                
                cam.fieldOfView -= changeAmnt;
                
            }
        }
    }

    void SetVisuals()
    {
        if (timeInSuperSpeed > timeNeededToBeInSuperSpeed)
        {
            // SET VISUALS TO SUPER SPEED
        }
        else if (timeOutOfSuperSpeed > timeNeededToBeOutOfSuperSpeed)
        { 
            // SET VISUALS TO NORMAL
        }
        
    }

    bool InSuperSpeed()
    {
        if ((player.getHorizontalVelocity() > (player.maxVelocity - maxVelocityThreshhold)))
        {
            return true;
        }

        return false;
    }
}
