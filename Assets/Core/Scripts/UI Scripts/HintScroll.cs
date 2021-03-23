using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HintScroll : MonoBehaviour
{
    public Hints source;
    int current = 0;
    public TextMeshProUGUI hinttext;
    public TextMeshProUGUI count;

    public void NextHint(int i)
    {
        current += i;
        if (current < 0)
        {
            current = source.hints.Capacity - 1;
        }
        else if (current > source.hints.Capacity - 1)
        {
            current = 0;
        }
        count.text = "" + (current + 1);
        hinttext.text = source.hints[current];
    }
    public void reset()
    {
        count.text = "?";
    }
}
