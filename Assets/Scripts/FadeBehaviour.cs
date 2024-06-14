using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBehaviour : MonoBehaviour
{
    public CanvasGroup cg;
    public bool fadeIn = false;
    public bool fadeOut = false;
    public float fadeTime;

    void Update()
    {
        if (fadeIn && cg.alpha < 1)
        {
            cg.alpha += fadeTime * Time.deltaTime;
            GetComponent<AudioSource>().volume = 1 - cg.alpha;

            if (cg.alpha >= 1)
            {
                fadeIn = false;
            }
        }
        if (fadeOut && cg.alpha >= 0)
        {
            cg.alpha -= fadeTime * Time.deltaTime;
            GetComponent<AudioSource>().volume = 1 - cg.alpha;

            if (cg.alpha >= 1)
            {
                fadeOut = false;
            }
        }
    }

    public void FadeIn()
    {
        fadeIn = true;
    }

    public void FadeOut()
    {
        fadeOut = true;
    }
}
