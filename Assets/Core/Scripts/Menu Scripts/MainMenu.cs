using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    [Header("Menus")]
    public GameObject main;
    public GameObject settings;
    public GameObject games;
    public GameObject credits;
    public GameObject lobby;
    public GameObject all;
    public GameObject characterSelect;

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

    public AudioMixer mixer;

    [Header("Player Relevant")]
    public GameObject playerInformationCarrier;

    void Start()
    {       
        PlayerPrefs.GetFloat("Volume", volume.maxValue / 2);
        PlayerPrefs.GetFloat("MouseSensitivity", MS.maxValue / 2);
        PlayerPrefs.GetInt("InvertHorizontal", 1);
        PlayerPrefs.GetInt("Invertvertical", 1);

        Cursor.lockState = CursorLockMode.None;

        volume.value = PlayerPrefs.GetFloat("Volume", 0.5f);
        MS.value = PlayerPrefs.GetFloat("MouseSensitivity", 20f);

        if (main != null)
        {
            Back();
            //source.volume = PlayerPrefs.GetFloat("Volume", volume.maxValue / 2);
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

        if(GameObject.FindGameObjectWithTag("PlayerInformationTag"))
        {
            Destroy(GameObject.FindGameObjectWithTag("PlayerInformationTag"));
        }

        //playerInformationCarrier = Instantiate(new GameObject());
        //playerInformationCarrier.name = "PlayerInformation";
        //DontDestroyOnLoad(playerInformationCarrier);
    }

    private void Update()
    {
        if (MS != null && volume != null)
        {
            float m = MS.value * 100f;
            float v = volume.value;

            m = Mathf.RoundToInt(m);
            v = Mathf.RoundToInt(v);

            SenNum.text = "" + m;
            VolNum.text = "" + v + " dB";
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
        if (games != null)
        games.SetActive(false);
        if (credits != null)
        credits.SetActive(false);
        if (lobby != null)
        lobby.SetActive(false);
        if (LoadingScreen != null)
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
        //source.volume = volume.value;
        if (mixer)
        {
            mixer.SetFloat("MusicVolume", volume.value);
        }
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

    public void HostGame(int mode) // 1 = race, 2 = payload
    {
        //SceneManager.LoadScene(1);

        characterSelect.GetComponent<CharacterSelection>().isStartingAsHost = true;

        games.SetActive(false);
        characterSelect.SetActive(true);

        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>().enabled = false;
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
        if (LoadingScreen)
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
