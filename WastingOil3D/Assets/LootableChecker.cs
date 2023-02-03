using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootableChecker : MonoBehaviour
{
    public Looting[] lootables;

    // Start is called before the first frame update
    void Start()
    {
        lootables = (Looting[])GameObject.FindObjectsOfType(typeof(Looting));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
