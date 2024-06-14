using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    FadeBehaviour fade;

    void Start()
    {
        fade = FindObjectOfType<FadeBehaviour>();
        fade.FadeOut();
    }
}
