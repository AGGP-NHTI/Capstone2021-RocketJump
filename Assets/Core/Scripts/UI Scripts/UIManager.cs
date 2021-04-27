using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Player_Movement_Controller playerMovement;

    
    

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
