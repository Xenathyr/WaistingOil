using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Author /Samu Haaja


public class PlayerHealthManager : MonoBehaviour {
    public static PlayerHealthManager instance = null; 

    public float startingHealth;
    public float currentHealth;
    private float healthAmount;

    public Transform healthMeter;
    public Transform staminaMeter;

    private float staminaDrainTimer = 0;// StaminaMeter functionality
    private float staminaRegenTimer; // SStaminaMeter functionality
    public float maxStamina;
    public float currentStamina;
    public ParticleSystem deathblood;

    public PlayerController PlayerControlsc;

    public Animator animator; // Death animaatioon

    void Awake() 
    {
        
        if (instance == null)
            instance = this;
        animator = GetComponentInChildren<Animator>();
    }


     void Start () {
        currentHealth = startingHealth; //Aloittaessa säädetään valmiiksi current health
        currentStamina = maxStamina;
    }   
    void Update ()
    {
        PlayerStatusUpdate();
        staminaRegenTimer += Time.deltaTime;
    }
    public void HurtPlayer(float damageAmount) //Deal damage to the player according to the damageamount
    {
        currentHealth -= damageAmount; //Dealing damage      
        if (currentHealth <= 0f) // If health is 0 or below
        {
            deathblood.Play();
            //animator.SetBool("playerdead", true);
            animator.Play("PlayerDie");// Death animaatio
            AudioManager.instance.PlayOneAtTime("DeathSong");

            PlayerControlsc.isDead = true; // Change bool in PlayerController script  , for disabling movement and shooting
                currentHealth = 0; //Health cant go to negative
        }

        int index = Random.Range(0, 4);
        if (index == 0 && currentHealth > 0)
        {
            AudioManager.instance.Play("PlayerHurt1");
        }
        if (index == 1 && currentHealth > 0)
        {
            AudioManager.instance.Play("PlayerHurt2");
        }
        if (index == 2 && currentHealth > 0)
        {
            AudioManager.instance.Play("PlayerHurt3");
        }
        else if (index == 3 && currentHealth > 0)
        {
            AudioManager.instance.Play("PlayerHurt4");
        }


    }
    public void PlayerStatusUpdate()
    {
        healthMeter.GetComponent<RectTransform>().localScale = new Vector3(currentHealth / startingHealth, 1, 1);
        staminaMeter.GetComponent<RectTransform>().localScale = new Vector3(currentStamina / maxStamina, 1, 1);
        if (Input.GetKey(KeyCode.LeftShift))
            
        { 
        staminaDrainTimer += Time.deltaTime;
        }
        else
        {
            if (currentStamina < maxStamina)
                if (staminaRegenTimer > 1)
                {
                    staminaRegenTimer = 0;

                    currentStamina += 10f;

                    if (currentStamina > 100)
                    {
                        currentStamina = 100f;
                    }
                }
        }
        if (currentStamina > 0)
        {
            if (staminaDrainTimer > 0.10)
            {
                staminaDrainTimer = 0;
                staminaRegenTimer = 0;
                currentStamina -= 2f;
                if(currentStamina < 0)
                {
                    currentStamina = 0;
                }
            }
        }
        
    }

    public void HealPlayer()
    {
        currentHealth = startingHealth;
    }
}
