using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Looting : MonoBehaviour
{

    private static int flares;
    private static int ammos;
    private static int healthpickups;
    private static int emptycontainers;

    public bool randomised;

    [Header("0=flare, 1=ammo, 2=health, 3=Empty, 4=key")]
    public int lootID;

    public int maxNum;
    public int minNum;

    
    public bool isLooted;

    public Inventory inventory;

    public GameObject textLootGroup;
    public Animator[] lootPopup;

    public GameObject timeBar;

    private bool lootsafetycheck;


    private void Start()
    {
        //inventory = (Inventory)FindObjectOfType(typeof(Inventory));

        emptycontainers = inventory.totalEmpty;
        flares = inventory.totalFlares;
        ammos = inventory.totalAmmos;
        healthpickups = inventory.totalHealthpickups;
        lootPopup = textLootGroup.GetComponentsInChildren<Animator>();
        
    }


    public void LootingObject(bool quickLoot)
    {
        if(isLooted == false)
        {
            if (randomised == true)
            {
                if (quickLoot == true)
                {
                    AudioManager.instance.PlayOneAtTime("LoudSearch");
                }
                if (quickLoot == false)
                {
                    AudioManager.instance.Play("Opening");
                    AudioManager.instance.PlayOneAtTime("QuietSearch");
                }

                RandomiseLootRoll(quickLoot);
            }
            else
            {
                if (quickLoot == true)
                {
                    AudioManager.instance.PlayOneAtTime("LoudSearch");
                }
                if (quickLoot == false)
                {
                    AudioManager.instance.PlayOneAtTime("Opening");
                    AudioManager.instance.PlayOneAtTime("QuietSearch");
                }


                if (quickLoot == false) timeBar.GetComponentInChildren<Animator>().Play("ReloadCursor");
                if (lootsafetycheck == false) StartCoroutine("LootingTimer", quickLoot);
            }
        }
        else
        {
            Debug.Log("Is already looted");
        }
    }

    void LootRoll()
    {
        

        if  (lootID == 0)
        {
            LootFlare();
        }
        else if (lootID == 1)
        {
            LootAmmo();
        }
        else if (lootID == 2)
        {
            LootHPpickup();
        }
        else if (lootID == 3)
        {
            LootEmpty();
        }
        else if(lootID == 4)
        {
            LootKey();
        }
        else
        {
            Debug.Log("Run Outta Items dah loot");
        }

        isLooted = true;
    }

    void RandomiseLootRoll(bool quickLoot)
    {

            while (true)
            {
            Debug.Log("Rolling!");

            int rand = Random.Range(0, 101);

            Debug.Log(rand);

            if (rand >= 0 && rand <= 25)
            {
                lootID = 3;
            }
            else if(rand >= 26 && rand <= 60)
            {
                lootID = 1;
            }
            else if(rand >= 61 && rand <= 80)
            {
                lootID = 0;
            }
            else if(rand >= 81 && rand <= 100)
            {
                lootID = 2;
            }
            else
            {
                Debug.Log("Fucked something up here");
            }
                

                //lootID = Random.Range(minNum, (maxNum + 1));
                if (flares <= 0 && ammos <= 0 && healthpickups <= 0 )
                {
                    Debug.Log("NO ITEMS!");
                    lootID = -1;
                    break;
                }
                else if (flares <= 0 && lootID == 0)
                {
                    Debug.Log("Outta Flares, rerolling");
                }
                else if (ammos <= 0 && lootID == 1)
                {
                    Debug.Log("Outta ammo, rerolling");
                }
                else if (healthpickups <= 0 && lootID == 2)
                {
                    Debug.Log("Outta hpu, rerolling");
                }
                else if (emptycontainers <= 0 && lootID == 3)
                {

                }
                else
                {
                    Debug.Log("Stuff maaaybe is correct, but probably broken anyway, moving to looting!");
                    break;
                }

            }

        if (quickLoot == false) timeBar.GetComponentInChildren<Animator>().Play("ReloadCursor");
        if (lootsafetycheck == false) StartCoroutine("LootingTimer", quickLoot);

    }

    void LootFlare()
    {
        Debug.Log("Looted " + inventory.ItemList[lootID]);
        AudioManager.instance.Play("Loot");
        AudioManager.instance.StopFast("QuietSearch");

        for(int i = 0; i < lootPopup.Length; i++)
        {
            if(lootPopup[i].GetCurrentAnimatorStateInfo(0).IsName("Lootpopup"))
            {

            }
            else
            {
                lootPopup[i].Play("Lootpopup");
                lootPopup[i].GetComponent<Text>().text = "Found a Flare";
                i = lootPopup.Length;
            }
        }

        //lootPopup.Play("Lootpopup");
        //lootPopup.GetComponent<Text>().text = "Found a Flare";
        inventory.flareCount += 1;
        if (randomised == true) --flares;
        inventory.InventoryUpdate();
    }

    void LootAmmo()
    {
        Debug.Log("Looted ammo");
        AudioManager.instance.Play("LootAmmo");
        AudioManager.instance.StopFast("QuietSearch");

        for (int i = 0; i < lootPopup.Length; i++)
        {
            if (lootPopup[i].GetCurrentAnimatorStateInfo(0).IsName("Lootpopup"))
            {

            }
            else
            {
                lootPopup[i].Play("Lootpopup");
                lootPopup[i].GetComponent<Text>().text = "Found x5 Ammo";
                i = lootPopup.Length;
            }
        }

        //lootPopup.Play("Lootpopup");
        //lootPopup.GetComponent<Text>().text = "Found x5 Ammo";
        inventory.AddAmmo();
        if (randomised == true) --ammos;
        
    }


    void LootHPpickup()
    {
        Debug.Log("Looted healthpickup");
        AudioManager.instance.Play("Loot");
        AudioManager.instance.StopFast("QuietSearch");

        for (int i = 0; i < lootPopup.Length; i++)
        {
            if (lootPopup[i].GetCurrentAnimatorStateInfo(0).IsName("Lootpopup"))
            {

            }
            else
            {
                lootPopup[i].Play("Lootpopup");
                lootPopup[i].GetComponent<Text>().text = "Found a Medkit";
                i = lootPopup.Length;
            }
        }

        //lootPopup.Play("Lootpopup");
        //lootPopup.GetComponent<Text>().text = "Found a Medkit";
        inventory.healthpickupCount += 1;
        if (randomised == true) --healthpickups;
        inventory.InventoryUpdate();

    }

    void LootKey()
    {
        Debug.Log("Looted Key");
        for (int i = 0; i < lootPopup.Length; i++)
        {
            if (lootPopup[i].GetCurrentAnimatorStateInfo(0).IsName("Lootpopup"))
            {

            }
            else
            {
                lootPopup[i].Play("Lootpopup");
                lootPopup[i].GetComponent<Text>().text = "Found a Key";
                i = lootPopup.Length;
            }
        }
        //lootPopup.Play("Lootpopup");
        //lootPopup.GetComponent<Text>().text = "Found a Key";
        inventory.obtainedKey = true;


    }

    void LootEmpty()
    {
        int index = Random.Range(0, 6);
        if (index == 0)
        {
            AudioManager.instance.Play("Closing");
        }
        if (index == 1)
        {
            AudioManager.instance.Play("Closing2");
        }
        if (index == 2)
        {
            AudioManager.instance.Play("Closing3");
        }
        if (index == 3)
        {
            AudioManager.instance.Play("Closing4");
        }
        if (index == 4)
        {
            AudioManager.instance.Play("Closing5");
        }
        else
        {
            AudioManager.instance.Play("Closing6");
        }
        AudioManager.instance.StopFast("QuietSearch");

        Debug.Log("Empty");

        for (int i = 0; i < lootPopup.Length; i++)
        {
            if (lootPopup[i].GetCurrentAnimatorStateInfo(0).IsName("Lootpopup"))
            {

            }
            else
            {
                lootPopup[i].Play("Lootpopup");
                lootPopup[i].GetComponent<Text>().text = "It was empty";
                i = lootPopup.Length;
            }
        }

        //lootPopup.Play("Lootpopup");
        //lootPopup.GetComponent<Text>().text = "It was empty";
        if (randomised == true) --emptycontainers;
    }

    public IEnumerator LootingTimer(bool quickLoot)
    {
        lootsafetycheck = true;

        float loottime;

        if(quickLoot == true)
        {
            loottime = 0.1f;
        }
        else
        {
            loottime = 2;
        }

        yield return new WaitForSeconds(loottime);

        
        LootRoll();
    }
}

