using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditHover : CustomButton
{
    
    public GameObject Facts;
    
    public override void OnHighlight()
    {
        base.OnHighlight();
        Facts.SetActive(true);
    }

    public override void OnHighlightEnd()
    {
        base.OnHighlightEnd();
        Facts.SetActive(false);
    }
}
