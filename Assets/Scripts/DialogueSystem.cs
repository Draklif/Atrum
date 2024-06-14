using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] TMP_Text textComponent;

    public void SetText(string text, float time)
    {
        StartCoroutine(SetTextCorutine(text, time));
    }

    IEnumerator SetTextCorutine(string text, float time)
    {
        yield return new WaitForSeconds(time);
        textComponent.text = text;
    }
}
