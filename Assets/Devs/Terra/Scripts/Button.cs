using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public GameObject PauseScreen;
    public bool PauseScreenActive = false;

    private void Awake()
    {
        PauseScreen = GameObject.Find("PauseScreen");
    }
    public void StartPressed()
    {
        SceneManager.LoadScene(1);
    }

    public void CreditsPressed()
    {
        SceneManager.LoadScene(2);
    }

    public void OptionsPressed()
    {
        SceneManager.LoadScene(3);
    }

    public void ExitPressed()
    {
        Application.Quit();
    }

    public void ContinuePressed()
    {
        PauseScreen.SetActive(false);
        PauseScreenActive = false;
        Time.timeScale = 1;
    }

    public void CharacterSelectPressed()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenuPressed()
    {
        SceneManager.LoadScene(0);
    }
}
