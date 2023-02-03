using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound 
{

    public string name;

    public AudioClip clip;

    [Range(0f, 2f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;

    public bool loop;

    public bool soundIsPlaying;

    [HideInInspector]
    public AudioSource source;

    // Update is called once per frame
    void Update()
    {
        
    }
}
