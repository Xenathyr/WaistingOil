using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaSound : MonoBehaviour
{
    public AudioSource seaSound;
    private PlayerHealthManager HP;
    private bool isPlaying = false;

    public void Awake()
    {
        HP = (PlayerHealthManager)FindObjectOfType(typeof(PlayerHealthManager));
    }

    public void Update()
    {
        if(isPlaying == false)
        {
            seaSound.Play();
            isPlaying = true;
        }

        if(HP.currentHealth <= 0)
        {
            seaSound.Stop();
        }
    }


}
