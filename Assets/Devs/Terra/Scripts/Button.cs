using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour
{
    private CanvasFinder Canvas;
    private PauseGame pauseGame;
    private EventSystem EventSystem;
    private GameObject VolumeSlider;
    private GameObject Continue;
    private GameObject Back;
    public bool PauseScreenActive = false;

    private void Awake()
    {
        pauseGame = FindObjectOfType<PauseGame>();
        Canvas = FindObjectOfType<CanvasFinder>();
        VolumeSlider = GameObject.Find("Volume");
        Continue = GameObject.Find("Continue");
        Back = GameObject.Find("ControlsBack");
        EventSystem = EventSystem.current;
    }

    public void StartPressed()
    {
        SceneManager.LoadScene("CharacterSelect") ;
    }

    public void CreditsPressed()
    {
        SceneManager.LoadScene("Credits");
    }

    public void OptionsPressed()
    {
        Canvas.OptionsScreen.GetComponent<Canvas>().enabled = true;
        Canvas.PauseScreen.GetComponent<Canvas>().enabled = false;
        EventSystem.SetSelectedGameObject(VolumeSlider);
    }

    public void ExitPressed()
    {
        Application.Quit();
    }

    public void ContinuePressed()
    {
        Canvas.PauseScreen.GetComponent<Canvas>().enabled = false;
        PauseScreenActive = false;
        pauseGame.gamePaused = false;
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
        Canvas.OptionsScreen.GetComponent<Canvas>().enabled = false;
        Canvas.PauseScreen.GetComponent<Canvas>().enabled = true;
        EventSystem.SetSelectedGameObject(Continue);
    }

    public void ControlsPressed()
    {
        Canvas.ControlScreen.GetComponent<Canvas>().enabled = true;
        Canvas.OptionsScreen.GetComponent<Canvas>().enabled = false;
        EventSystem.SetSelectedGameObject(Back);
    }

    public void ControlsBackPressed()
    {
        Canvas.ControlScreen.GetComponent<Canvas>().enabled = false;
        Canvas.OptionsScreen.GetComponent<Canvas>().enabled = true;
    }

    public void CreditsBack()
    {
        SceneManager.LoadScene(0);
    }
}
