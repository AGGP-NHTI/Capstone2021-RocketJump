using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ammo_UI_Script : MonoBehaviour
{

    public TextMeshProUGUI magazine;
    public TextMeshProUGUI reserve;

    public void SetMagazine(int amnt)
    {
        magazine.text = amnt.ToString();
    }

    public void SetReserve(int amnt)
    {
        reserve.text = amnt.ToString();
    }
}
