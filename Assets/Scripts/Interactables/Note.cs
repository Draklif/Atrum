using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Note : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameObject canvas;
    [SerializeField] int eventId;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Load()
    {
        switch (eventId)
        {
            case 1:
                StartHeartbeat();
                eventId = 0;
                break;
            default:
                break;
        }

        player.GetComponent<FirstPersonController>().cameraCanMove = false;
        player.GetComponent<FirstPersonController>().playerCanMove = false;
        player.GetComponent<FirstPersonController>().enableCrouch = false;
        player.GetComponent<FirstPersonController>().enableFlashlight = false;

        canvas.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
    }

    public void Close()
    {
        player.GetComponent<FirstPersonController>().cameraCanMove = true;
        player.GetComponent<FirstPersonController>().playerCanMove = true;
        player.GetComponent<FirstPersonController>().enableCrouch = true;
        player.GetComponent<FirstPersonController>().enableFlashlight = true;

        canvas.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
    }

    void StartHeartbeat()
    {
        audioSource.Play();
    }
}
