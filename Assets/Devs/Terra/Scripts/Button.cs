using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour
{
    public GameObject PauseScreen;
    public GameObject OptionsScreen;
    private EventSystem EventSystem;
    private GameObject VolumeSlider;
    private GameObject Continue;
    public bool PauseScreenActive = false;

    private void Awake()
    {
        PauseScreen = GameObject.Find("PauseScreen");
        OptionsScreen = GameObject.Find("Options");
        VolumeSlider = GameObject.Find("Volume");
        Continue = GameObject.Find("Continue");
        EventSystem = EventSystem.current;
    }

    private void Start()
    {
        Debug.Log(EventSystem.gameObject);
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
        OptionsScreen.SetActive(true);
        EventSystem.SetSelectedGameObject(VolumeSlider);
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

    public void OptionsBackPressed()
    {
        OptionsScreen.SetActive(false);
        EventSystem.SetSelectedGameObject(Continue);
    }
}
