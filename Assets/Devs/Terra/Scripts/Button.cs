using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour
{
    private CanvasFinder Canvas;
    private EventSystem EventSystem;
    private GameObject VolumeSlider;
    private GameObject Continue;
    public bool PauseScreenActive = false;

    private void Awake()
    {
        Canvas = FindObjectOfType<CanvasFinder>();
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
        Canvas.OptionsScreen.SetActive(true);
        EventSystem.SetSelectedGameObject(VolumeSlider);
    }

    public void ExitPressed()
    {
        Application.Quit();
    }

    public void ContinuePressed()
    {
        Canvas.PauseScreen.SetActive(false);
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
        Canvas.OptionsScreen.SetActive(false);
        EventSystem.SetSelectedGameObject(Continue);
    }

    public void ControlsPressed()
    {
        Canvas.ControlScreen.SetActive(true);
    }

    public void ControlsBackPressed()
    {
        Canvas.ControlScreen.SetActive(false);
    }
}
