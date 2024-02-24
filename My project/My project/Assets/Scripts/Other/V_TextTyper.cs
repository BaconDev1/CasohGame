using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class V_TextTyper : MonoBehaviour
{
    [Header("Main")]
    [Space(5)]
    public float letterPause = 0.2f;
    [Header("Sound")]
    [Space(5)]
    public bool useSound;
    public AudioSource audioSource;
    public AudioClip clip;
    public float volume = 0.5f;

    string message;
    TextMeshProUGUI textComp;

    // Use this for initialization
    void Start()
    {
        textComp = GetComponent<TextMeshProUGUI>();
        message = textComp.text;
        textComp.text = "";
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        foreach (char letter in message.ToCharArray())
        {
            textComp.text += letter;
            if (useSound)
            {
                audioSource.PlayOneShot(clip, volume);
            }
            yield return 0;
            yield return new WaitForSeconds(letterPause);
        }
    }
}
