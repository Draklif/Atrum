using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watcher : MonoBehaviour
{
    [SerializeField] AudioSource screamerAudio;
    [SerializeField] GameObject deadScreen;
    Transform player;
    FirstPersonController playerFpc;
    public float speed = 0.1f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerFpc = player.GetComponent<FirstPersonController>();
    }

    private void Update()
    {
        transform.LookAt(player, Vector3.up);
        if (!playerFpc.flashlight.isActiveAndEnabled && speed < 6f)
        {
            if (speed > 2)
            {
                speed -= 0.0001f;
            }
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, player.position) < 4f)
            {
                speed += 0.5f;
            }
        }
        else if (playerFpc.flashlight.isActiveAndEnabled && speed >= 6f)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, player.position) < 4f)
            {
                speed += 0.5f;
            }
        }
        else if (speed >= 6f)
        {
            speed -= 0.0001f;
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else
        {
            StartCoroutine(MadCorutine());
        }
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, player.position) < 1f)
        {
            deadScreen.SetActive(true);
            if (!screamerAudio.isPlaying)
            {
                screamerAudio.Play();
            }
            StartCoroutine(DeadCorutine());
        }
    }

    IEnumerator MadCorutine()
    {
        yield return new WaitForSeconds(2);
        if (playerFpc.flashlight.isActiveAndEnabled)
        {
            speed += 0.001f;
        }
    }

    IEnumerator DeadCorutine()
    {
        yield return new WaitForSeconds(2);
        Application.Quit();
    }
}
