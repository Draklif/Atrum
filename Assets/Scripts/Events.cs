using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startup : MonoBehaviour
{
    void Start()
    {
        DialogueSystem ds = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<DialogueSystem>();
        ds.SetText("�D�nde estoy?", 3);
        ds.SetText("", 7);
        ds.SetText("Tengo que salir de aqu�...", 10);
        ds.SetText("Pero...", 15);
        ds.SetText("La salida necesita un c�digo...", 18);
        ds.SetText("", 24);
    }
}
