using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

// Original author
//Author /Samu Haaja - partly Antti Koponen

// REFEROSOINTI TOISEEN SCRIPTIIN: Tee luokasta objecti scriptiin jossa haluat käyttää sitä, anna sille nimi, jonka jälkeen käytä tätä nimeä + .arvo mitä
//haetaan. Esim public HealthManager ADHD   ja myöhemmin if(ADHD.playerhealth < 5)
// STAMINA ARVOT TASAAN?


public class PlayerController : MonoBehaviour {
    public static PlayerController instance;
    [SerializeField] // Komento = Alla olevaa arvoa voi muuttaa Unityssä
    private float movementSpeed = 5;
    [SerializeField]
    private float runningSpeed = 10;
    [SerializeField]
    private float smashShakeAmount = 30;
    [SerializeField]
    private float smashShakeDuration = 0.15f;

    public SpriteRenderer sr;
    public Animator animator;
    public bool isDead;
    public bool playerIsRunning = false;
    public bool playerIsShooting = false;
    public bool playerMoving = false;
    
    public float ShootingNoiseAmount = 10;
    public float QuickLootNoiseAmount = 5;
    public GameObject Player;
    public Inventory ammo;
    public Text InteractionText;
    private NavMeshAgent navAgent;
    private PauseMenu PM;
    


    public float RotateSpeed = 30f;

    public GameObject gun;

    public GameObject gameOverScreen;

    public bool isLooting = false;
    public bool quickLooting = false;

    private float FadeDuration = 2f;
    private Color startColor = new Color32(255, 255, 255, 0);
    private Color endColor = new Color32(255, 255, 255, 255);
    private float lastColorChangeTime;
    private float timer;
    private float elapsedTime;

    private PlayerHealthManager PlayerHPMANAG;  //ESIMERKKI ESIMESI


    private void Awake()
    {
        
        animator = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        navAgent = GetComponent<NavMeshAgent>();
        PM = (PauseMenu)FindObjectOfType(typeof(PauseMenu));
        PlayerHPMANAG = (PlayerHealthManager)FindObjectOfType(typeof(PlayerHealthManager));

    }
    void Start () {
        instance = this;

	}

    void Update() {



        if (isDead == false && isLooting == false)
        {
            HandleMovementInput();      
            HandleShootInput();       
            HandleRotationInput();
            HandleReloadAnimation();
        
        }
        if (isDead == true)
        {
            if(gameOverScreen.activeSelf == false)
            {
                AudioManager.instance.PlayOneAtTime("PlayerDeath");
                
            } 
            GameOverScreen();

        }
        if(PlayerHPMANAG.currentHealth == 0f)
        {
            
            isDead = true;
        }
        if(isLooting == true)
        {
            HandleLootAnimation();
        }
        if (gameOverScreen.transform.GetChild(0).gameObject.activeSelf == true)
        {


            timer += Time.deltaTime;

            var ratio = timer / FadeDuration;
            ratio = Mathf.Clamp01(ratio);
            gameOverScreen.GetComponentInChildren<Image>().color = Color.Lerp(startColor, endColor, ratio);
        }
    }



    void HandleMovementInput()
    {
        float _horizontal = Input.GetAxis("Horizontal");  //Get input for horizontal and vertical from Unity
        float _vertical = Input.GetAxis("Vertical");
        // Animate(_vertical,_horizontal); // Animate player according to input
        Vector3 _movement = new Vector3(_horizontal, 0f, _vertical);


        if (_movement != Vector3.zero)

        {
            animator.SetBool("moving", true);
            playerMoving = true;
        }
        else
        { 
            animator.SetBool("moving", false);
            playerMoving = false;
        }

        if (Input.GetKey(KeyCode.LeftShift) && PlayerHPMANAG.currentStamina > 0) //ESIMERKKI WORKS 
        {
            animator.SetBool("running", true);
            transform.Translate(_movement * runningSpeed * Time.deltaTime, Space.World);
            playerIsRunning = true;
            

            if (Input.GetKey(KeyCode.W) &&  Input.GetKey(KeyCode.A))
            {
                transform.eulerAngles = new Vector3(0, 315, 0);
            }
            else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S))
            {
                transform.eulerAngles = new Vector3(0, 225, 0);
            }
            else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S))
            {
                transform.eulerAngles = new Vector3(0, 135, 0);
            }
            else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W))
            {
                transform.eulerAngles = new Vector3(0, 45, 0);
            }
            else if (Input.GetKey(KeyCode.W))
            {
               // animator.SetInteger("Direction", 8);
                transform.eulerAngles = new Vector3(0, 0, 0);                
            }
            else if (Input.GetKey(KeyCode.A))
            {
                //animator.SetInteger("Direction", 6);
                //sr.flipY = true;
                transform.eulerAngles = new Vector3(0, 270, 0);
            }
            else if (Input.GetKey(KeyCode.D))
            {
               // animator.SetInteger("Direction", 6);
                //sr.flipY = false;
                transform.eulerAngles = new Vector3(0, 90, 0);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
              //  animator.SetInteger("Direction", 2);
            }

        }
        else
        {

            transform.Translate(_movement * movementSpeed * Time.deltaTime, Space.World); // Normal speed
            playerIsRunning = false; // For bool checks
            animator.SetBool("running", false); //Animator bool for running animation
            
        }

        //Player Walking sound
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (playerIsRunning == false)
            {
                AudioManager.instance.PlayOneAtTime("PlayerWalking");
            }
            if(playerIsRunning == true)
            {
                AudioManager.instance.PlayOneAtTime("PlayerRunning");
            }
        }
        else
        {
            AudioManager.instance.Stop("PlayerWalking");
            AudioManager.instance.Stop("PlayerRunning");
        }

    }
    public void HandleRotationInput()
    {
        RaycastHit _hit;
        Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Follow mouse
        if (PM.GameIsPaused == false)
        {
            if (playerIsRunning == false)
            {
                if (Physics.Raycast(_ray, out _hit))
                {
                    transform.LookAt(new Vector3(_hit.point.x, transform.position.y, _hit.point.z));
                }
            }
        }
    }

    public void HandleLootAnimation()
    {
        if(isLooting == true)
        {
            animator.SetBool("looting", true);
        }
        else
        {
            animator.SetBool("looting", false);
        }

        Debug.Log(quickLooting);

        if ( quickLooting == true)
        {
            Debug.Log("ANIMASHOON BOOL!");
            animator.SetBool("smashing", true);
            StartCoroutine(SmashImpact());
            quickLooting = false;
        }
        else
        {
            animator.SetBool("smashing", false);
        }

    }
    public void HandleReloadAnimation()
    {
        if (ammo.reloading == true)
        {
            animator.SetBool("reloading", true);
        }
        else animator.SetBool("reloading", false);
    }




    //{
    //    if (playerIsRunning == false)
    //    {
    //        RaycastHit _hit;
    //        Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Follow mouse
    //        if (Physics.Raycast(_ray, out _hit))
    //        {


    //            float mousePosX = _hit.point.x - transform.position.x;
    //            float mousePosZ = _hit.point.z - transform.position.z;

    //            //Debug.Log(mousePosX + " ||||| " + mousePosZ);

    //            if (mousePosX > -3 && mousePosX < 3 && mousePosZ < -2)
    //            {
    //                animator.SetInteger("Direction", 2);

    //            }
    //            else if (mousePosX > -3 && mousePosX < 3 && mousePosZ > 2)
    //            {
    //                animator.SetInteger("Direction", 8);

    //            }
    //            else if (mousePosZ > -3 && mousePosZ < 3 && mousePosX > 1)
    //            {
    //                sr.flipX = false;
    //                animator.SetInteger("Direction", 6);

    //            }
    //            else if (mousePosZ > -3 && mousePosZ < 3 && mousePosX < -1)
    //            {
    //                sr.flipX = true;
    //                animator.SetInteger("Direction", 6);

    //            }

    //        }
    //    }




    //}

    void HandleShootInput()
    {
        if (Input.GetButton("Fire1") && ammo.ammoClip > 0) //Control "Fire1" from Unity Input
        {
            //Shoot

            if (ammo.reloading == false)
            {
                PlayerGun.Instance.Shoot();
                playerIsShooting = true;
                animator.SetBool("shooting", true);
            }
        }
        else if(Input.GetButtonDown("Fire1") && ammo.ammoClip <= 0)
        {
            AudioManager.instance.Play("PistolEmpty");
        }
        else
        {
            animator.SetBool("shooting", false);
            playerIsShooting = false;
        }
    }
    
    void GameOverScreen()
    {
        StartCoroutine(playerDeath());
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "eyes")
        {
            other.transform.parent.GetComponent<NewAITest>().checkSight();          
        }

        if (other.gameObject.name == "EnemySight")
        {
            other.transform.parent.GetComponent<MonsterAI>().monsterSight();
        }
        
    }
    IEnumerator playerDeath()
    {
        yield return new WaitForSeconds(3.5f);


        gameOverScreen.transform.GetChild(0).gameObject.SetActive(true);


        yield return new WaitForSeconds(2f);

        
        gameOverScreen.transform.GetChild(1).gameObject.SetActive(true);

    }

    IEnumerator SmashImpact()
    {
        yield return new WaitForSeconds(0.4f);
        CameraController.instance.shakeDuration = smashShakeDuration;
        CameraController.instance.shakeAmount = smashShakeAmount;
    }
}
