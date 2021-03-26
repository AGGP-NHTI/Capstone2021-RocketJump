using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdSetup : MonoBehaviour
{
    public List<Sprite> ads = new List<Sprite>();
    public Image target;

    public float min, max;
    float current = 0;

    void Start()
    {
        Randomize();
    }

    public void FixedUpdate()
    {
        current -= Time.fixedDeltaTime;
        if (current <= 0)
        {
            Randomize();
        }
    }

    public void Randomize()
    {
        target.sprite = ads[Random.Range(0, ads.Capacity)];
        current = Random.Range(min, max);
    }
    
}
