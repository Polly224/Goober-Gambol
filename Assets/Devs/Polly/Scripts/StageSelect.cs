using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class StageSelect : MonoBehaviour
{
    public int pickedIndex = 0;
    public Vector3[] stagePositions;
    public float changeSpeed = 0.2f;
    Vector3 intendedPos;
    [SerializeField] InputAction iA;
    [SerializeField] InputAction conTinue;
    bool canContinue = false;

    private void Start()
    {
        iA.performed += SwitchStage;
        iA.Enable();
        conTinue.performed += ConfirmPick;
        conTinue.Enable();
        StartCoroutine(TimeDelay());
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, stagePositions[pickedIndex], changeSpeed);
    }

    public void SwitchStage(CallbackContext context)
    {
        pickedIndex = pickedIndex == 0 ? 1 : 0; 
    }

    public void ConfirmPick(CallbackContext context)
    {
        if (canContinue)
        {
            conTinue.Disable();
            if (pickedIndex == 0)
            {
                PlayerDataStorage.pickedStage = PlayerDataStorage.PickedStage.Rooftop;
                SceneManager.LoadScene("Rooftops");
            }
            else
            {
                PlayerDataStorage.pickedStage = PlayerDataStorage.PickedStage.Docks;
                SceneManager.LoadScene("Docks");
            }
        }
    }

    private IEnumerator TimeDelay()
    {
        yield return new WaitForSeconds(0.5f);
        canContinue = true;
        yield break;
    }
}
