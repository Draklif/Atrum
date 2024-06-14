using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Keypad : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] string combination;

    private GameObject player;
    private TMP_Text keypadText;
    private bool isFinished;

    private void Start()
    {
        keypadText = canvas.GetComponentInChildren<TMP_Text>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void PressNumber(string number)
    {
        if (keypadText.color != Color.green)
        {
            if (keypadText.color == Color.red)
            {
                keypadText.text = "";
                keypadText.color = Color.white;
                return;
            }

            if (keypadText.text == "")
            {
                keypadText.text = number;
                return;
            }

            keypadText.text = keypadText.text + number;

            if (keypadText.text.Length >= combination.Length)
            {
                if (keypadText.text == combination)
                {
                    keypadText.color = Color.green;
                }
                else
                {
                    keypadText.color = Color.red;
                }
            }
        }
    }

    public void Load()
    {
        FirstPersonController fpc = player.GetComponent<FirstPersonController>();

        fpc.cameraCanMove = false;
        fpc.playerCanMove = false;
        fpc.enableCrouch = false;
        fpc.enableFlashlight = false;

        Keypad actualKeypad = canvas.GetComponent<Keypad>();
        if (!actualKeypad.isFinished)
        {
            actualKeypad.combination = combination.ToLower();
            canvas.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void Close()
    {
        FirstPersonController fpc = player.GetComponent<FirstPersonController>();
        fpc.cameraCanMove = true;
        fpc.playerCanMove = true;
        fpc.enableCrouch = true;
        fpc.enableFlashlight = true;

        canvas.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
    }
}
