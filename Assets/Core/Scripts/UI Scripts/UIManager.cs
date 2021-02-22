using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI lapText;
    public TextMeshProUGUI captionText;

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


}
