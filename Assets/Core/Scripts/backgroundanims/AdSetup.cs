using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdSetup : MonoBehaviour
{
    public List<Sprite> ads = new List<Sprite>();
    public Image target;
    void Start()
    {
        target.sprite = ads[Random.Range(0, ads.Capacity)];
    }

    
}
