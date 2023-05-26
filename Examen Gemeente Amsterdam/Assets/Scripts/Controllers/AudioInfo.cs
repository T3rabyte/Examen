using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioInfo : MonoBehaviour
{
    public List<AudioClip> audioClips;
    public AudioSource audioSource;

    private void Start()
    {
        if(audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }
}
