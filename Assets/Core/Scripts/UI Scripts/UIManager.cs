﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Player_Movement_Controller playerMovement;


    [Header("Finish Screen")]
    public TMP_Text winner_text;
    public TMP_Text second_text;
    public TMP_Text third_text;
    public GameObject participationTitle_text;
    public TMP_Text participation_text;
    public TMP_Text countdown_text;
    public GameObject failsafe_button;

    [Header("Player Info")]
    public TMP_Text playerPlacementText;
    public TMP_Text playerPlacementSuffix;
    public TMP_Text lapText;
    public TMP_Text maxLapText;

    [Header("Weapon Info")]
    public TMP_Text clipText;
    public TMP_Text clipSizeText;

    [Header("Extra")]
    public TMP_Text captionText;
    public GameObject PauseMenu;
    bool paused = false;
    public bool isHost;
    Timer timer = new Timer();
    public int countdown;
    private float lastCountdownNumber = 100;


    [Range(0.01f, 5)]
    public float captionFadeRate = 1;
    [Range(0.01f, 5)]
    public float captionMoveRate = 1;

    Vector3 captionStartingPosition;
    Vector3 captionTargetPosition;


    bool captionWait = false;
    private void Start()
    {
        captionText.color = new Color(255,255,255, 0);

        captionStartingPosition = captionText.rectTransform.position;

        if (gameObject)
        {
            SpeedometerScript speedometer = gameObject.GetComponentInChildren<SpeedometerScript>();
            if (speedometer)
            {
                speedometer.player = playerMovement;
            }
            

        }
    }

    private void Update()
    {
        if (!captionWait)
        {
            if (captionText.color.a > 0)
            {
                captionText.color = new Color(255, 255, 255, captionText.color.a - (captionFadeRate * Time.deltaTime));
            }

            float distance = Vector3.Distance(captionText.rectTransform.position, captionTargetPosition);

            if (distance > 10)
            {
                Vector3 dir = captionTargetPosition - captionText.rectTransform.position;
                captionText.rectTransform.Translate(dir * captionMoveRate * Time.deltaTime);
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                pause();
            }
        }

        if (isHost)
        {

            if (timer.updateTimer())
            {
                failsafe_button.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                PlayerInformation.controller.PNC.shutdownServer();
            }
            if (timer.runTimer)
            {
                countdown_text.text = "returning to menu in " + Mathf.Round(timer.time);
                countdown = (int)Mathf.Round(timer.time);

                if (Mathf.Round(timer.time) < Mathf.Round(lastCountdownNumber))
                {
                    lastCountdownNumber = Mathf.Round(timer.time);
                    updateClientFinishScreens(countdown);
                }
            }
        }
        else
        {
            countdown_text.text = "returning to menu in " + countdown;
        }

    }

    public void failsafeReturnToMenu()
    {
        PlayerInformation.controller.PNC.failsafeDisconnectHost();
    }

    public void sendMessage(string messsage, Vector3? startPositionChange = null, float duration = 3)
    {
        captionWait = true;
        StartCoroutine(waitMessage(duration));
        if(!startPositionChange.HasValue) { startPositionChange = Vector3.zero; }

        captionTargetPosition =  captionStartingPosition + startPositionChange.Value;
        captionText.rectTransform.position = captionStartingPosition;
        
        captionText.text = messsage;

        captionText.color = new Color(255, 255, 255, 0.9f);
    }

    IEnumerator waitMessage(float duration)
    {
        captionWait = true;
        yield return new WaitForSeconds(duration);
        captionWait = false;
    }

    public void setAmmo(int clip, int clipSize)
    {
        clipText.text = clip.ToString();
        clipSizeText.text = clipSize.ToString();
    }
    public void setPlace(int place)
    {
        playerPlacementText.text = place.ToString();
        playerPlacementSuffix.text = "st";
    }
    public void setLapText(int lap, int maxLap)
    {
        lapText.text = lap.ToString();
        maxLapText.text = ("/" + maxLap);
    }

    public void updateClientFinishScreens(int countdown)
    {
        PlayerInformation.controller.PNC.updateFinishCountdown(countdown);
    }

    public void updateFinishCountdown(int c)
    {
        countdown = c;
    }

    public void displayFinishScreen(string[] players)
    {

        foreach(Transform child in transform)
        {
            if (child.gameObject.name == "victoryScreen")
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }

        }

        winner_text.text = "";
        second_text.text = "";
        third_text.text = "";
        participation_text.text = "";

        for(int i = 0; i < players.Length; i++)
        {
            if (i == 0) winner_text.text = players[i];
            else if (i == 1) second_text.text = players[i];
            else if (i == 2) third_text.text = players[i];
            else
            {
                participationTitle_text.SetActive(true);
                participation_text.text += players[i] + "\n";
            }
        }

        if(isHost)
        {
            timer.setTimer(10);
        }
    }

    public void pause()
    {
        if (!paused)
        {
            PauseMenu.SetActive(true);
            paused = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            PauseMenu.SetActive(false);
            paused = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void updatePositionText(int position)
    {
        playerPlacementText.text = position.ToString();
        switch (position)
        {
            case 1:
                playerPlacementSuffix.text = "st";
                break;
            case 2:
                playerPlacementSuffix.text = "nd";
                break;
            case 3:
                playerPlacementSuffix.text = "rd";
                break;
            default:
                playerPlacementSuffix.text = "th";
                break;
        }
    }
}
