using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
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
}
