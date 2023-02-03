using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    public static AudioManager instance;

    public AudioMixerGroup AudioMixer;

    private PauseMenu PM;
    private MonsterAI MA;
    private TriggerSafety TG;
    private GetToDahChoppah Choppa;
    public GameObject gameOverScreen;

    private bool MusicIsOn = false;
    private bool MonsterChasingMusic = false;
    private bool RadioMusicHasPlayed = false;
    private float originalVolume = 1;

    void Awake()
    {

        PM = (PauseMenu)FindObjectOfType(typeof(PauseMenu));
        MA = (MonsterAI)FindObjectOfType(typeof(MonsterAI));
        TG = (TriggerSafety)FindObjectOfType(typeof(TriggerSafety));
        Choppa = (GetToDahChoppah)FindObjectOfType(typeof(GetToDahChoppah));
        gameOverScreen = GameObject.Find("Canvas/GameOvah/GameOverText");

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        //DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.outputAudioMixerGroup = AudioMixer;
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.soundIsPlaying = false;
        }
    }

    public void Update()
    {

        if (MusicIsOn == false)
        {
            MusicPlayer();
        }

        //Pause Menu Music
        foreach (Sound s in sounds)
        {
            if(s.source.volume != 0)
            originalVolume = s.source.volume;

            if (PM.GameIsPaused == true || gameOverScreen.activeSelf == true)
            {
                if(s.name != "PauseMenuMusic")
                {
                    if(s.name != "PauseMenuAppear")
                    {
                        if(s.name != "HoverOverUI")
                        {
                            if (s.name != "ToggleSound")
                            {
                                if(s.name != "DeathSong")
                                {
                                    s.source.volume = 0f;
                                }                
                            }
                        }
                    }          
                }
            }
            else
            {
                s.source.volume = originalVolume;
            }
            
            /*
            if (|| gameOverScreen.transform.GetChild(0).gameObject.activeSelf == true)
            {
                if(s.name != "GameOverMusic")
                {
                    if(s.name != "HoverOverUI")
                    {
                        if(s.name != "ToggleSound")
                        {
                            s.source.volume = 0f;
                        }
                    }
                }
            }
            */
        }

        //Monster Chasing Music
        foreach (Sound s in sounds)
        {
            if (MA.monsterChasing == true)
            {
                if (s.name == "Music0" || s.name == "Music1" || s.name == "Music2" || s.name == "Music3" || s.name == "RadioMusic" || s.name == "WaypointIntro" || s.name == "Waypoint")
                {
                    s.source.volume = 0f;
                }
            }
            else if (MA.monsterChasing == false && TG.PlayerIsInSafety == true)
            {
                s.source.volume = originalVolume;
            }
        }


        if(MA.monsterChasing == true && TG.PlayerIsInSafety == false)
        {
            instance.PlayOneAtTime("MusicX");
        }
        else if (MA.monsterChasing == false && TG.PlayerIsInSafety == true)
        {
            Stop("MusicX");
        }

        //Radio Music
        foreach (Sound s in sounds)
        {
            if (Choppa.choppaCalled == true)
            {
                if (s.name == "Music0" || s.name == "Music1" || s.name == "Music2" || s.name == "Music3")
                {
                    s.source.pitch = 0f;
                }
            }
            else
            {
                s.source.pitch = 1f;
            }
        }

        if (RadioMusicHasPlayed == false)
        {
            if (Choppa.choppaCalled == true)
            {
                instance.PlayMusic("RadioMusic");
                RadioMusicHasPlayed = true;
            }
        }


    }

    //Many instances of the same sound can exist at a time
    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        /*
        if (PM.GameIsPaused == true)
        {
            s.source.pitch *= .5f;
        }
        else
        {
            s.source.pitch = 1f;
        }
        */

        s.source.Play();
    }

    //Only one instance of sound can exist at a time
    public void PlayOneAtTime(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        if (s.soundIsPlaying == false)
        {
            s.source.volume = s.volume;
            s.source.Play();
            s.soundIsPlaying = true;
            StartCoroutine(Stop(s, s.clip.length));
        }
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        if (s.soundIsPlaying == false)
        {
            s.source.volume = s.volume;
            s.source.Play();
            s.soundIsPlaying = true;
            StartCoroutine(Stop(s, s.clip.length));
        }
    }


    //Stop with normal Fade Out
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        StartCoroutine(fadeSound(s));
    }

    //Stop with long Fade Out
    public void StopSlow(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        StartCoroutine(fadeSoundSlow(s));
        s.soundIsPlaying = false;
    }

    public void StopFast(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        StartCoroutine(Stop(s, 0));
        s.soundIsPlaying = false;
    }

    IEnumerator Stop(Sound s, float delay)
    {
        yield return new WaitForSeconds(delay);
        s.source.Stop();
        s.soundIsPlaying = false;

        if (s.name == "RadioMusic")
        {
            instance.PlayMusic("WaypointIntro");
        }

    }

    IEnumerator StopMusic(Sound s, float delay)
    {
        yield return new WaitForSeconds(delay);
        s.source.Stop();
        s.soundIsPlaying = false;
        MusicIsOn = false;
    }

    IEnumerator fadeSound(Sound s)
    {
        while(s.source.volume > 0.01f)
        {
            s.source.volume -= Time.deltaTime / 15f;
            yield return null;
        }
        if(s.source.volume <= 0.01f)
        {
            s.source.Stop();
        }

    }

    IEnumerator fadeSoundSlow(Sound s)
    {
        while (s.source.volume > 0.01f)
        {
            s.source.volume -= Time.deltaTime / 60f;
            yield return null;
        }
        if (s.source.volume <= 0.01f)
        {
            s.source.Stop();
            s.soundIsPlaying = false;
        }

    }

    public void MusicPlayer()
    {
        MusicIsOn = true;

        int index = UnityEngine.Random.Range(0, 4);

        if (index == 0)
        {
            instance.PlayMusic("Music0");
            Sound s = Array.Find(sounds, sound => sound.name == "Music0");
            StartCoroutine(StopMusic(s, s.clip.length));
        }
        else if (index == 1)
        {
            instance.PlayMusic("Music1");
            Sound s = Array.Find(sounds, sound => sound.name == "Music1");
            StartCoroutine(StopMusic(s, s.clip.length));
        }
        else if (index == 2)
        {
            instance.PlayMusic("Music2");
            Sound s = Array.Find(sounds, sound => sound.name == "Music2");
            StartCoroutine(StopMusic(s, s.clip.length));
        }
        else if (index == 3)
        {
            instance.PlayMusic("Music3");
            Sound s = Array.Find(sounds, sound => sound.name == "Music3");
            StartCoroutine(StopMusic(s, s.clip.length));
        }
    }
}
