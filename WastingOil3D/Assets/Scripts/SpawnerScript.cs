using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject monster;
    public Transform[] spawnSpots;
    public Inventory inventory;

    public float timeBtwSpawns;
    public float startTimeBtwSpawns = 7;

    public FieldOfView fov;

    private bool isInList;

    public int maxMonster;
    public int spawnedMonster;

    // Start is called before the first frame update
    void Start()
    {
        spawnSpots = GetComponentsInChildren<Transform>();
        timeBtwSpawns = startTimeBtwSpawns;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (spawnedMonster <= maxMonster)
        {

            if (timeBtwSpawns <= 0)
            {
                int randPos = Random.Range(0, spawnSpots.Length);

                isInList = false;

                for (int i = 0; i < fov.visibleTargets.Count; i++)
                {

                    if (fov.visibleTargets[i].name == spawnSpots[randPos].name)
                    {
                        isInList = true;
                        Debug.Log("WasFoundInList");
                    }
                    else
                    {
                        Debug.Log("Wasn't in the list");
                    }

                }

                if (isInList == false)
                {
                    Instantiate(monster, spawnSpots[randPos].position, Quaternion.identity);
                    ++spawnedMonster;
                }


                timeBtwSpawns = startTimeBtwSpawns;
                Debug.Log("SmallMonster spawned!");
            }
            else
            {
                timeBtwSpawns -= Time.deltaTime;
            }
        }

    }   
}
