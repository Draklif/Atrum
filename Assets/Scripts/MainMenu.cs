using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    FadeBehaviour fade;

    private void Start()
    {
        fade = GetComponent<FadeBehaviour>();
    }

    public IEnumerator ChangeScene(string scene)
    {
        fade.FadeIn();
        yield return new WaitForSeconds(fade.fadeTime + 1);
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    public void StartGame()
    {
        StartCoroutine(ChangeScene("Hospital"));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
