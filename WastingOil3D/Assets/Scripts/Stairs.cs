using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Stairs : MonoBehaviour
{

    private Vector3 destination;

    public int floorDestination;


    public GameObject floorTwoSpawner;
    public GameObject floorThreeSpawner;
    public GameObject aallot;

    private bool stairsTimer;

    // Start is called before the first frame update
    void Start()
    {
        destination = GetComponentInChildren<Transform>().Find("Destination").position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void climbStairs(Collider other)
    {
       
            
            if (other.tag == "Player" && stairsTimer == false)
            {
            other.GetComponent<NavMeshAgent>().enabled = false;
            other.transform.position = destination;
            other.GetComponent<Collider>().enabled = false;
            StartCoroutine(timerForStairs(other));
            other.GetComponent<NavMeshAgent>().enabled = true;

                var monsters = GameObject.FindGameObjectsWithTag("smallMonster");
                foreach (var clone in monsters)
                {
                    Destroy(clone);
                }

                floorTwoSpawner.GetComponent<SpawnerScript>().spawnedMonster = 0;
                floorThreeSpawner.GetComponent<SpawnerScript>().spawnedMonster = 0;

                if (floorDestination == 2)
                {
                aallot.SetActive(false);
                floorTwoSpawner.SetActive(true);
                    floorThreeSpawner.SetActive(false);
                }
                if (floorDestination == 3)
                {
                    floorTwoSpawner.SetActive(false);
                    floorThreeSpawner.SetActive(true);
                }
                if (floorDestination == 1)
                {
                aallot.SetActive(true);
                    floorTwoSpawner.SetActive(false);
                    floorThreeSpawner.SetActive(false);
                }

            }
        
    }

    IEnumerator timerForStairs(Collider other)
    {
        stairsTimer = true;
        yield return new WaitForSeconds(0.2f);
        other.GetComponent<Collider>().enabled = true;
        stairsTimer = false;
    }
}
