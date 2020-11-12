using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{

    // A simple timer script to keep track of various elements that need to be timed. Runs in seconds

    public float time; // Current time of timer (in seconds)

    public bool runTimer; // If the timer is currently running

    public Timer()
    {
        time = 0;
        runTimer = false;
    }

    
    public bool updateTimer() // Updates the timer. Call this in the update function of whatever script is using a timer.
    {
        if(runTimer)
        {
            time -= Time.deltaTime;

            if(time <= 0)
            {
                runTimer = false;
                time = 0;
                return true; //  Returns true when the timer has finished.
            }

        }

        return false; //  Returns false while the timer is still counting down

    }

    public void setTimer(float seconds) // Sets the timer in seconds. Make sure you call this before the timer starts updating.
    {
        time = seconds;
        runTimer = true;
    }

}
