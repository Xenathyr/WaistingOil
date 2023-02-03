using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetToDahChoppah : MonoBehaviour
{
    public bool choppaCalled;

    public GameObject choppah;
    public Camera cameraSizing;

    private void OnTriggerEnter(Collider other)
    {
        if(choppaCalled == true && other.tag == "Player")
        {
            cameraSizing.orthographicSize = 20;

            choppah.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (choppaCalled == true && other.tag == "Player")
        {
            cameraSizing.orthographicSize = 9;
        }
    }

}
