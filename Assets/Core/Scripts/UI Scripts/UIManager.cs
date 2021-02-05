using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI lapText;
    public TextMeshProUGUI captionText;


    private void Start()
    {
        captionText.color = new Color(255,255,255, 0);
    }

    public void sendMessage(string messsage, float duration = 5)
    {
        captionText.text = messsage;

        StartCoroutine(waitMessage(duration));
    }

    IEnumerator waitMessage(float duration)
    {
        captionText.color = new Color(255, 255, 255, 0.9f);
        yield return new WaitForSeconds(duration);
        captionText.color = new Color(255, 255, 255, 0);
    }


}
