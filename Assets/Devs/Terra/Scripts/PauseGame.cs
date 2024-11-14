using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PauseGame : MonoBehaviour
{
    [SerializeField] Button Button;
    // Start is called before the first frame update
    void Start()
    {
        Button.PauseScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Pause(CallbackContext context)
    {
        if (context.performed)
        {
            Button.PauseScreen.SetActive(true);
            Button.PauseScreenActive = true;
            Time.timeScale = 0;
        }
    }
}
