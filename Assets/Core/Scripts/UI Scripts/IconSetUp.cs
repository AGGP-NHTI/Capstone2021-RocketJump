using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconSetUp : MonoBehaviour
{
    public CharacterIcons icon;
    public GameObject endpos;
    public GameObject bigpic;
    public GameObject background;
    public float speed = 10f;

    void Start()
    {
        icon.endpoint = endpos;
        icon.speed = speed;
        icon.bigPic = bigpic;
        icon.backing = background;
        background.SetActive(false);
        bigpic.SetActive(false);
    }

}
