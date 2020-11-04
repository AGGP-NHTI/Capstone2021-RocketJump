using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUIHandler : MonoBehaviour
{

    public Image meter;
    public Material blue;
    public Material lighterBlue;
    private float fade;
    private float fadeEnd;
    private bool colorSwap;

    //For Testing purposes
    public float charge; // current ability recharge
    public float chargeRate; // ability recharge rate
    public float minCharge; // minimum charge (what it gets set to when used, probably keep at 0)
    public float maxCharge; // maximum charge (charge needed to be used, don't go over 100)
    private bool isReady; // ability ready to be used (this will most likely be handled in player script, this is here for testing purposes)
    public bool useAbility; // For testing purposes, trigger to use ability if ready;
    //

    void Start()
    {
        charge = 0;
        chargeRate = 25; 
        maxCharge = 100;
        minCharge = 0;
        fade = 0;
        fadeEnd = 1;
        colorSwap = true;
}

    
    void Update()
    {

        if(isReady)
        {
            if(fade < fadeEnd)
            {
                fade += Time.deltaTime * fadeEnd;

                if(colorSwap)
                {
                    meter.color = Color.Lerp(blue.color, lighterBlue.color, fade);
                }
                if(!colorSwap)
                {
                    meter.color = Color.Lerp(lighterBlue.color, blue.color, fade);
                }

                if(meter.color == blue.color)
                {
                    colorSwap = true;
                    fade = 0;
                }
                if(meter.color == lighterBlue.color)
                {
                    colorSwap = false;
                    fade = 0;
                }

            }
        }

        if(useAbility)
        {
            if(isReady)
            {
                charge = 0;
                isReady = false;
                Debug.Log("ability would be, in theory, used now");
            }
            else
            {
                Debug.Log("ability, in theory, can not be used because it is not charged");
            }

            useAbility = false;
        }

        if(charge < maxCharge)
        {

            charge += chargeRate * Time.deltaTime;

        }
        else
        {
            isReady = true;
        }


        meter.fillAmount = charge / 100;
    }
}
