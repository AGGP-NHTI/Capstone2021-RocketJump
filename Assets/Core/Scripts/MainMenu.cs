using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using MLAPI;
using MLAPI.Messaging;

public class MainMenu : MonoBehaviour
{
    public GameObject main;
    public GameObject settings;
    public GameObject games;

    public Slider volume;
    public Slider MS;

    public Toggle horizontal;
    public Toggle vertical;

    void Start()
    {
        Back();
        PlayerPrefs.GetFloat("Volume", volume.maxValue / 2);
        PlayerPrefs.GetFloat("MouseSensitivity", MS.maxValue / 2);
        PlayerPrefs.GetInt("InvertHorizontal", 1);
        PlayerPrefs.GetInt("Invertvertical", 1);

        volume.value = PlayerPrefs.GetFloat("Volume", 20f);
        MS.value = PlayerPrefs.GetFloat("MouseSensitivity", 20f);

        if (PlayerPrefs.GetInt("InvertHorizontal", 1) == 1)
        {
            horizontal.isOn = true;
        }
        else
        {
            horizontal.isOn = false;
        }

        if (PlayerPrefs.GetInt("InvertVertical", 1) == 1)
        {
            vertical.isOn = true;
        }
        else
        {
            vertical.isOn = false;
        }
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Debug.Log("Quit");
        Application.Quit();
    }
    public void Back()
    {
        main.SetActive(true);
        settings.SetActive(false);
        games.SetActive(false);
    }
    public void ToSettings()
    {
        main.SetActive(false);
        settings.SetActive(true);
    }
    public void ToGames()
    {
        main.SetActive(false);        
        games.SetActive(true);
    }
    public void ChangeVolume()
    {
        PlayerPrefs.SetFloat("Volume", volume.value);
    }
    public void ChangeMS()
    {
        PlayerPrefs.SetFloat("MouseSensitivity", MS.value);
    }
    public void ChangeInvertMouseHorizontal()
    {
        if (horizontal.isOn)
        {
            PlayerPrefs.SetInt("InvertHorizontal", 1);
        }
        else
        {
            PlayerPrefs.SetInt("InvertHorizontal", -1);
        }
    }
    public void ChangeInvertMouseVertical()
    {
        if (vertical.isOn)
        {
            PlayerPrefs.SetInt("InvertVertical", 1);
        }
        else
        {
            PlayerPrefs.SetInt("InvertVertical", -1);
        }
    }
    public void JoinGame(TMP_InputField game)
    {

    }
    public void HostGame()
    {
        SceneManager.LoadScene(1);
    }
}
