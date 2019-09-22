using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RasonNetwork;

public class ChatPlayerInfo : NetworkedBehaviour
{
    public RPlayer info;
    public Text tex;
    public GameObject speak;
    AudioSource audioSource;
    RVoice voice;

    public void Intia(RPlayer i) {
        audioSource = GetComponent<AudioSource>();
        voice = GetComponent<RVoice>();
        info = i;
        tex.text = info.name;
    }

    void LateUpdate()
    {
        if (audioSource.isPlaying || voice.isTransmitting)
        {
            speak.SetActive(true);
        }
        else { speak.SetActive(false); }
    }
}
