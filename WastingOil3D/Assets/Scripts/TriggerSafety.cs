using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSafety : MonoBehaviour
{
    private MonsterAI MA;
    public bool PlayerIsInSafety = false;

    void Awake()
    {
        MA = (MonsterAI)FindObjectOfType(typeof(MonsterAI));
    }

    void Update()
    {
        
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            MA.monsterChasing = false;
            MA.InvestigateTime = 0;
            PlayerIsInSafety = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            PlayerIsInSafety = false;
        }
    }

    /*
    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            MA.monsterChasing = true;
        }
    }
    */
}
