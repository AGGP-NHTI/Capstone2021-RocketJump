using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupUIScript : MonoBehaviour
{
    
    // Minus the UI variables, most of these variables will be handled and stored by the Powerup class, whenever that is made. These are just here for testing beforehand

    public Image meter;
    public Image powerupImage; // Will be set to pineapple. For testing!
    private Transform powerupImageStartPos;
    public List<Sprite> powerupImageSource; // The image symbol or whatever for each unique powerup

    private bool isActive; // If a powerup is active
    private bool isStored; // If a powerup is being stored (held by player without being used)
    public bool trigger; // A trigger to trigger the powerup. Since Powerups dont exist yet, this will act as a manual trigger from the insepctor to test the UI. This will be removed once powerups are actually added.

    Timer timer; // Timer script for timing

    float examplePowerupDuration; // Example duration of the nonexistant pineapple powerup. For testing?

    

    void Start()
    {
        timer = new Timer();
        isActive = false;
        isStored = true;
        examplePowerupDuration = 5f;
        powerupImageStartPos = powerupImage.gameObject.transform;
    }

    
    void Update()
    {

        if (trigger)
        {
            trigger = false;
            timer.setTimer(examplePowerupDuration);
            powerupImage.sprite = powerupImageSource[0];
            isActive = true;
            isStored = false;
        }

        if (isActive)
        {
            
            if (timer.updateTimer())
            {
                isActive = false;
            }
            
            setUIElements(true, true);

            meter.fillAmount = ((timer.time - 0) / (examplePowerupDuration - 0));
            var alpha = meter.color;
            alpha.a = ((timer.time - 0) / (examplePowerupDuration - 0));
            meter.color = alpha;


        }
        else if(isStored)
        {
            setUIElements(false, true);

        }
        else
        {
            setUIElements(false, false);

        }

        

    }

    void setUIElements(bool state1, bool state2)
    {
        meter.gameObject.SetActive(state1);
        powerupImage.gameObject.SetActive(state2);
    }

}
