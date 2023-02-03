using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Author / Samu Haaja




public class TakeDamage : MonoBehaviour {

    //This script is a collision damage trigger on Player AND ENEMY, add to the enemy & player bullet prefab or other hazards
    // 
    [SerializeField]
    public float damageToGive;
    public float force;
    public float knockBackduration;
   // public GameObject bloodEffect;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "smallMonster" && other.gameObject.GetComponent<NewAITest>().currentHealth >= 0)
        {
          //Instantiate(bloodEffect, transform.position, bloodEffect.transform.rotation);
           // CameraController.instance.shakeDuration = 0.3f;
            other.gameObject.GetComponent<NewAITest>().HurtEnemy(damageToGive);          
        }
    }
    
}
