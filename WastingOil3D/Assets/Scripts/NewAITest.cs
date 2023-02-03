using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NewAITest : MonoBehaviour
{
    //Components
    private GameObject player;
    private NavMeshAgent nav;
    private AudioSource sound;
    private Animator anim;
    public string state = "idle";
    private ParticleSystem bloodEffect;
    private PlayerController playerController;
    private IObjects iObjects;
    private SpriteRenderer spriteRenderer;
    private FieldOfView FoV;
    private PauseMenu PM;
    private GameObject gameOverScreen;

    //Patrolling floats
    private float waitTime;
    public float waitTimeMin;
    public float waitTimeMax;
    public float startingStunTime;
    private float stunTimeLeft;

    //Hearing 
    public float playerNoiseDistance = 1000;
    private float enemyHearingDistance = 15;
    private float enemyHearingDistanceOnAlert;
    public float enemyHearingDistanceByDefault = 15f;
    public float InvestigateTime;
    public float startInvestigateTimeMin = 1;
    public float startInvestigateTimeMax = 2;
    private Vector3 investigateSpot;
    public bool monsterHearsPlayer = false;
    private float stopped = 0;


    //Stats
    private bool highAlert = false;
    private float alertness = 20f;
    public float currentHealth;
    public float maxHealth = 1;
    public float damageToGive = 1;
    private bool alive = true;
    private bool canAttack;
    
    [SerializeField]
    private float speed = 7;
    [SerializeField]
    private float chasingspeed = 15;
    [SerializeField]
    private float investigatespeed = 10;
    [SerializeField]
    private float animSpeed = 1.5f;
    [SerializeField]
    private float animChaseSpeed = 2.5f;


    //public Audiosource Screech;
    //public AudioClio[] slitherSound;

    public SpawnerScript spawnNumber;

    [HideInInspector]
    public bool stunnedMonster;

    public bool stunImmunity;
    public float stunImmunityDuration;
    public float stunDuration;

    private bool deathCheck = false;
    private bool smallMonsterIsScreaming = false;

    public AudioSource SmallMonsterAttack1;
    public AudioSource SmallMonsterAttack2;
    public AudioSource SmallMonsterHit;
    public AudioSource SmallMonsterMoving;
    public AudioSource SmallMonsterScream;
    public AudioSource SmallMonsterScream2;
    public AudioSource SmallMonsterScream3;
    public AudioSource SmallMonsterScream4;
    public AudioSource SmallMonsterScream5;
    public AudioSource SmallMonsterScream6;
    public AudioSource SmallMonsterScream7;

    private void Awake()
    {
        FoV = (FieldOfView)FindObjectOfType(typeof(FieldOfView));
        enemyHearingDistanceOnAlert = enemyHearingDistance + 5f;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        playerController = (PlayerController)FindObjectOfType(typeof(PlayerController));
        iObjects = (IObjects)FindObjectOfType(typeof(IObjects));
        bloodEffect = GetComponentInChildren<ParticleSystem>();
        //sound = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        PM = (PauseMenu)FindObjectOfType(typeof(PauseMenu));

        gameOverScreen = GameObject.Find("Canvas/GameOvah/GameOverText");
    }
    private void Start()
    {
        waitTime = Random.Range(waitTimeMin, waitTimeMax);
        nav.speed = speed;
        anim.speed = animSpeed;
        currentHealth = maxHealth;
        stunTimeLeft = startingStunTime;
        playerNoiseDistance = 1000;
        InvestigateTime = Random.Range(startInvestigateTimeMin, startInvestigateTimeMax);
        spawnNumber = (SpawnerScript)FindObjectOfType(typeof(SpawnerScript));
    }



    //Checking monster sight//
    public void checkSight() //Ignore raycast -layer on the object "Eyes cone" for better vision
        {
            if (alive)
            {              
                    // print("hit " + rayHit.collider.gameObject.name);                                
                        if(state != "Attack")
                        {
                            state = "chase";
                            nav.speed = chasingspeed;
                            anim.speed = animChaseSpeed;                           
                        //Screech.pitch = 1.2f
                        //Screech.Play();
                   
                        }                
            }
        }
    private void Update()
    {
        monsterSoundsHandler();

        //if(FoV.stunnedMonster == false)
       // {
           // anim.SetBool("Stunned", false);
       // }
        if (alive == true)
        {
            
            Debug.Log(state);
            monsterHearing();
            // Debug.DrawLine(eyes.position, player.transform.position, Color.green);
            //anim.SetFloat("stunTime", stunTimeLeft);
            anim.SetFloat("velocity", nav.velocity.magnitude); 
            
            if(nav.velocity.x >= 1) // If movement detected to certain direction, flips the sprite
            {
                spriteRenderer.flipX = false;
            }
            else if (nav.velocity.x <= -1)
            {
                spriteRenderer.flipX = true;
            }

            if (nav.speed == 0.1f)
            {
                stunTimeLeft -= Time.deltaTime;
                if (stunTimeLeft < 0)
                {
                    nav.speed = speed;
                    stunTimeLeft = startingStunTime;
                }
            }

            if (stunnedMonster == true && stunImmunity == false)
            {
                GetComponentInChildren<Animator>().SetBool("Stunned", true);
                StartCoroutine(Stuntimer());
                canAttack = false;
            }
            else
            {
                GetComponentInChildren<Animator>().SetBool("Stunned", false);
                canAttack = true;
            }
        }

        //SmallMonster moving sound
        if (state != "idle")
        {
            if (SmallMonsterMoving.isPlaying == false && PM.GameIsPaused == false)
            SmallMonsterMoving.Play();
        }

        //Idle
        if (state == "idle")
        {
            anim.speed = animSpeed;
            Vector3 randomPos = Random.insideUnitSphere * alertness;
            NavMeshHit navHit;
            NavMesh.SamplePosition(transform.position + randomPos, out navHit, 20f, NavMesh.AllAreas);
            nav.speed = speed;

            //Go near the player//
            if (highAlert)
            {
                NavMesh.SamplePosition(transform.position + randomPos, out navHit, 20f, NavMesh.AllAreas);
                //each time, lose awareness of player general position
                alertness += 5f;
                nav.speed = chasingspeed;
                enemyHearingDistance = enemyHearingDistanceOnAlert; //Alert hearing is +5.0f by default
                if (alertness > 20f)
                {
                    highAlert = false;
                    nav.speed =  speed;
                    enemyHearingDistance = enemyHearingDistanceByDefault;
                }
            }
            nav.SetDestination(navHit.position);
            state = "walk";
        }

        //Walk//
        if (state == "walk")
        {
            if (nav.remainingDistance <= nav.stoppingDistance && !nav.pathPending && monsterHearsPlayer == false)
            {
                state = "search";
                waitTime = Random.Range(waitTimeMin, waitTimeMax);
                anim.speed = 1.5f;
                nav.speed = speed;
                //state = "idle";
            }
        }

        //Search//
        if (state == "search")
        {
            if (waitTime > 0f)
            {
                waitTime -= Time.deltaTime;
            }
            else
            {
                state = "idle";
            }
        }

        //Chase//
        if (state == "chase")
        {
            if (smallMonsterIsScreaming == false)
            {
                int index = Random.Range(0, 7);

                if (index == 0)
                {
                    smallMonsterIsScreaming = true;
                    SmallMonsterScream.Play();
                    StartCoroutine(smallMonsterScream());
                }
                if (index == 1)
                {
                    smallMonsterIsScreaming = true;
                    SmallMonsterScream2.Play();
                    StartCoroutine(smallMonsterScream());
                }
                if (index == 2)
                {
                    smallMonsterIsScreaming = true;
                    SmallMonsterScream3.Play();
                    StartCoroutine(smallMonsterScream());
                }
                if (index == 3)
                {
                    smallMonsterIsScreaming = true;
                    SmallMonsterScream4.Play();
                    StartCoroutine(smallMonsterScream());
                }
                if (index == 4)
                {
                    smallMonsterIsScreaming = true;
                    SmallMonsterScream5.Play();
                    StartCoroutine(smallMonsterScream());
                }
                if (index == 5)
                {
                    smallMonsterIsScreaming = true;
                    SmallMonsterScream6.Play();
                    StartCoroutine(smallMonsterScream());
                }
                if (index == 6)
                {
                    smallMonsterIsScreaming = true;
                    SmallMonsterScream7.Play();
                    StartCoroutine(smallMonsterScream());
                }
                else
                {
                    StartCoroutine(smallMonsterScream());
                }
            }

            nav.destination = player.transform.position;

            //Losing sight of player//
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance > 15f)
            {
                state = "hunt";
                anim.speed = 1.5f;
            }
            else if(nav.remainingDistance <= nav.stoppingDistance && !nav.pathPending)
            {
                if (!IsInvoking("PerformAttack") && playerController.isDead == false)
                {
                    
                    InvokeRepeating("PerformAttack", .25f, 1.5f);
                }
            }
            else
            {
                CancelInvoke("PerformAttack");
                anim.SetBool("attacking", false);
            }
            nav.SetDestination(player.transform.position);


        }

        //Hunt//
        if (state == "hunt")
        {         
            if (nav.remainingDistance <= nav.stoppingDistance && !nav.pathPending)
            {
                state = "search";
                waitTime = Random.Range(waitTimeMin, waitTimeMax);
                highAlert = true;
                alertness = 5f; //highAlert returns false once alertness reaches >20
                checkSight();
            }
        }
        if (state == "investigate")
        {
            nav.SetDestination(investigateSpot);
            nav.speed = investigatespeed;
            if (Vector3.Distance(transform.position, investigateSpot) < 4f)
            {
                if (InvestigateTime <= 0)
                {
                    monsterHearsPlayer = false;
                    nav.speed = speed;
                    state = "hunt";                   
                }
                else
                {
                    InvestigateTime -= Time.deltaTime;
                    nav.speed = stopped;
                }
            }
        }
    }
    public void HurtEnemy(float damage)
    {

        //BloodPart.AddComponent<AudioSource>();
        //  BloodPart.GetComponent<AudioSource>().PlayOneShot(enemyDie);
        bloodEffect.GetComponent<ParticleSystem>().Play();
        currentHealth -= damage;
        alive = false;
        //anim.SetBool("dead", true);
        
        if (currentHealth <= 0 && deathCheck == false)
        {
            anim.SetBool("dead", true);
            deathCheck = true;
            StartCoroutine(enemyDeath());

            int index = Random.Range(0, 4);
            if (index == 0)
            {
                AudioManager.instance.PlayOneAtTime("SmallMonsterDeath1");
            }
            if(index == 1)
            {
                AudioManager.instance.PlayOneAtTime("SmallMonsterDeath2");
            }
            if(index == 2)
            {
                AudioManager.instance.PlayOneAtTime("SmallMonsterDeath3");
            }
            if(index == 3)
            {
                AudioManager.instance.PlayOneAtTime("SmallMonsterDeath4");
            }         
        }
    }

    public void PerformAttack()
    {    
        if (canAttack == true)
        {
            Debug.Log("Hit player!");
            anim.SetBool("attacking", true);
            player.GetComponent<PlayerHealthManager>().HurtPlayer(damageToGive);
            int index = Random.Range(0, 2);

            if (index == 0)
            {
                if (SmallMonsterAttack1.isPlaying == false)
                    SmallMonsterAttack1.Play();
            }
            else
            {
                if (SmallMonsterAttack2.isPlaying == false)
                    SmallMonsterAttack2.Play();
            }
        }
    }
    IEnumerator enemyDeath()
    {
        
        yield return new WaitForSeconds(1.5f);
        if (spawnNumber != null)
        {
            --spawnNumber.spawnedMonster;
        }
        Destroy(gameObject);
    }

    IEnumerator smallMonsterScream()
    {
        yield return new WaitForSeconds(5f);
        smallMonsterIsScreaming = false;

    }

    void monsterHearing()
    {
        playerNoiseDistance = 1000;
        monsterHearsPlayer = false;

        if (playerController.playerIsRunning == true)
        {
            playerNoiseDistance = CalculatePathLenght(player.transform.position);
        }

        if (playerController.playerIsShooting == true)
        {
            playerNoiseDistance = CalculatePathLenght(player.transform.position);
            playerNoiseDistance = playerNoiseDistance - playerController.ShootingNoiseAmount;
        }
        if (iObjects.playerIsSmashing == true)
        {
            playerNoiseDistance = CalculatePathLenght(player.transform.position);
            playerNoiseDistance = playerNoiseDistance - iObjects.SmashingNoiseAmount;
        }

        if (playerNoiseDistance < enemyHearingDistance)
        {
            InvestigateTime = Random.Range(startInvestigateTimeMin, startInvestigateTimeMax);
           // GiveUpTime = Random.Range(startGiveUpTimeMin, startGiveUpTimeMax);
            investigateSpot = player.transform.position;
            monsterHearsPlayer = true;
            state = "investigate";
            Debug.Log("A small monster heard you!");
        }
    }
    float CalculatePathLenght(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();

        if (nav.enabled)
        {
            nav.CalculatePath(targetPosition, path);
        }


        Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];

        allWayPoints[0] = transform.position;
        allWayPoints[allWayPoints.Length - 1] = targetPosition;

        for (int i = 0; i < path.corners.Length; i++)
        {
            allWayPoints[i + 1] = path.corners[i];
        }

        float pathLength = 0f;
        for (int i = 0; i < allWayPoints.Length - 1; i++)
        {
            pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
        }

        return pathLength;
    }

    IEnumerator Stuntimer()
    {
        yield return new WaitForSeconds(stunDuration);
        stunImmunity = true;
        yield return new WaitForSeconds(stunImmunityDuration);
        stunImmunity = false;

    }

    void monsterSoundsHandler()
    {
        if (PM.GameIsPaused == true || gameOverScreen.activeSelf == true)
        {
            SmallMonsterAttack1.Stop();
            SmallMonsterAttack2.Stop();
            SmallMonsterHit.Stop();
            SmallMonsterMoving.Stop();
            SmallMonsterScream.Stop();
            SmallMonsterScream2.Stop();
            SmallMonsterScream3.Stop();
            SmallMonsterScream4.Stop();
            SmallMonsterScream5.Stop();
            SmallMonsterScream6.Stop();
            SmallMonsterScream7.Stop();
        }
    }
}

