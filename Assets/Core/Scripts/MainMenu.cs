using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject main;
    public GameObject settings;
    public GameObject games;
    
    
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
}
