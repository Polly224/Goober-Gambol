using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PauseGame : MonoBehaviour
{
    private CanvasFinder Canvas;
    private Button Button;

    private void Awake()
    {
        Canvas = FindObjectOfType<CanvasFinder>();
        Button = FindObjectOfType<Button>();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Pause(CallbackContext context)
    {
        if (context.performed)
        {
            Canvas.PauseScreen.SetActive(true);
            Button.PauseScreenActive = true;
            Time.timeScale = 0;
        }
    }
}
