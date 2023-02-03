using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    public NavMeshAgent agent;

    private GameObject player;

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {

        agent.SetDestination(player.transform.position);

    }
}
