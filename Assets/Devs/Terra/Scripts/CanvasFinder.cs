using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFinder : MonoBehaviour
{
    public GameObject PauseScreen;
    public GameObject OptionsScreen;
    public GameObject RoundOverScreen;
    public GameObject ControlScreen;

    private void Awake()
    {
        PauseScreen = GameObject.Find("PauseScreen");
        OptionsScreen = GameObject.Find("Options");
        RoundOverScreen = GameObject.Find("RoundEndScreen");
        ControlScreen = GameObject.Find("ControlScreen");
    }

    private void Start()
    {
        PauseScreen.SetActive(false);
        OptionsScreen.SetActive(false);
        ControlScreen.SetActive(false);
        RoundOverScreen.SetActive(false);
    }
}
