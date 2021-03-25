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
    [Header("Menus")]
    public GameObject main;
    public GameObject settings;
    public GameObject games;
    public GameObject credits;
    public GameObject lobby;
    public GameObject all;

    [Header("Loading Screen")]
    public GameObject LoadingScreen;
    public AudioSource source;
    public Slider progress;
    public TextMeshProUGUI percent;

    [Header("Settings")]
    public Slider volume;
    public Slider MS;

    public Toggle horizontal;
    public Toggle vertical;

    public TextMeshProUGUI SenNum;
    public TextMeshProUGUI VolNum;

    void Start()
    {       
        PlayerPrefs.GetFloat("Volume", volume.maxValue / 2);
        PlayerPrefs.GetFloat("MouseSensitivity", MS.maxValue / 2);
        PlayerPrefs.GetInt("InvertHorizontal", 1);
        PlayerPrefs.GetInt("Invertvertical", 1);

        volume.value = PlayerPrefs.GetFloat("Volume", 0.5f);
        MS.value = PlayerPrefs.GetFloat("MouseSensitivity", 20f);

        if (main != null)
        {
            Back();
            source.volume = PlayerPrefs.GetFloat("Volume", volume.maxValue / 2);
            if (PlayerPrefs.GetInt("InvertHorizontal", 1) == -1)
            {
                horizontal.isOn = true;
            }
            else
            {
                horizontal.isOn = false;
            }

            if (PlayerPrefs.GetInt("InvertVertical", 1) == -1)
            {
                vertical.isOn = true;
            }
            else
            {
                vertical.isOn = false;
            }
        }
    }

    private void Update()
    {
        if (MS != null && volume != null)
        {
            float m = MS.value * 100f;
            float v = volume.value * 100f;

            m = Mathf.RoundToInt(m);
            v = Mathf.RoundToInt(v);

            SenNum.text = "" + m;
            VolNum.text = "" + v;
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
        credits.SetActive(false);
        lobby.SetActive(false);
        LoadingScreen.SetActive(false);
    }

    public void BackFromLobby()
    {
        lobby.SetActive(false);
        games.SetActive(true);
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

    public void ToCredits()
    {
        main.SetActive(false);
        credits.SetActive(true);
    }

    public void ToLobby()
    {
        games.SetActive(false);
        lobby.SetActive(true);
    }

    public void ChangeVolume()
    {
        PlayerPrefs.SetFloat("Volume", volume.value);
        source.volume = volume.value;
    }

    public void ChangeMS()
    {
        PlayerPrefs.SetFloat("MouseSensitivity", MS.value);
    }

    public void ChangeInvertMouseHorizontal()
    {
        if (horizontal.isOn)
        {
            PlayerPrefs.SetInt("InvertHorizontal", -1);
        }
        else
        {
            PlayerPrefs.SetInt("InvertHorizontal", 1);
        }
    }

    public void ChangeInvertMouseVertical()
    {
        if (vertical.isOn)
        {
            PlayerPrefs.SetInt("InvertVertical", -1);
        }
        else
        {
            PlayerPrefs.SetInt("InvertVertical", 1);
        }
    }

    public void JoinGame(TMP_InputField game)
    {

    }

    public void HostGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ToTutorial()
    {
        SceneManager.LoadScene(0);
    }
   
    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void CloseMenu()
    {
        all.SetActive(false);
    }

    public void OpenMenu()
    {
        all.SetActive(true);
    }

    public void ToScene(int num)
    {
        LoadingScreen.SetActive(true);
        source.Stop();
        StartCoroutine(Loader(num));
    }

    IEnumerator Loader(int num)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(num);

        while (!operation.isDone)
        {
            
            float p = Mathf.Clamp01(operation.progress / .9f);
            progress.value = p;
            percent.text = "" + (p * 100) + "%";

            yield return null;            
        }
    }
}
