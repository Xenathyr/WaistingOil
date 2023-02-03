using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioScript : MonoBehaviour
{
    public AudioSource audio1;
    public AudioSource audio2;
    private GetToDahChoppah Choppa;
    private bool AudioHasStarted = false;

    // Start is called before the first frame update
    void Awake()
    {
        Choppa = (GetToDahChoppah)FindObjectOfType(typeof(GetToDahChoppah));
    }

    // Update is called once per frame
    void Update()
    {
        if(Choppa.choppaCalled == true && AudioHasStarted == false)
        {
            audio1.Pause();
            audio2.Play();
            AudioHasStarted = true;
        }
    }
}
