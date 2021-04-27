using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class positionDisplay : MonoBehaviour
{

    public TMP_Text positionText;
    public TMP_Text suffixText;

    public void updatePositionText(int position)
    {
        positionText.text = position.ToString();
        switch(position)
        {
            case 1:
                suffixText.text = "st";
                break;
            case 2:
                suffixText.text = "nd";
                break;
            case 3:
                suffixText.text = "rd";
                break;
            default:
                suffixText.text = "th";
                break;
        }
    }
}
