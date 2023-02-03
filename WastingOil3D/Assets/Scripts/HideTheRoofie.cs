using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideTheRoofie : MonoBehaviour
{
    public SpriteRenderer roof;

    public GameObject shadowObstacle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            roof.enabled = false;
            shadowObstacle.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            roof.enabled = true;
            shadowObstacle.SetActive(false);
        }
    }
}
