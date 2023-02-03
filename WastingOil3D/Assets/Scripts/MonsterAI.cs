using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Samun lisäykset: cs10, 78, 116,

public class MonsterAI : MonoBehaviour
{
    public Transform eyes; // SAMU: Monsterin näkö, eli lapsiobjekti (On myös nimettävä "eyes" koska paska scripti ei toimi muuten :D)
    public float waitTime;
    public float startWaitTimeMin = 3;
    public float startWaitTimeMax = 6;
    public float damageToGive = 4;
    
    [SerializeField]
    private float timeBetweenAttacks = 1.5f;
    private float attackTimer;

    public float GiveUpTime;
    public float startGiveUpTimeMin = 3;
    public float startGiveUpTimeMax = 6;

    public float InvestigateTime;
    public float startInvestigateTimeMin = 3;
    public float startInvestigateTimeMax = 6;

    public Transform[] moveSpots;
    private int randomSpot;
    public NavMeshAgent agent;
    public int monsterSpeed = 3;
    public int monsterSpeedChasing = 9;
    public int monsterSpeedInvestigating;
    private int stopped = 0;

    private GameObject player;
    public bool monsterChasing = true;
    public bool monsterHearsPlayer = false;
    public bool monsterIsInvestigating = false;
    public bool monsterIsPatrolling = true;
    public float maxSightDistance = 25;
    public bool spotted;


    private Animator anim; //SAMU: ANIMAATIO
    private float animspeed; //SAMU: ANIMAATIO
    public LineRenderer lineOfSight;
    public Gradient redColor;
    public Gradient greenColor;

    public float playerNoiseDistance = 100;
    public float enemyHearingDistance;

    private Vector3 investigateSpot;

    private PlayerController playerController;

    public Rigidbody rb;

    public AudioSource BigMonsterBreathingReverb;
    public AudioSource BigMonsterChasing;
    public AudioSource BigMonsterHit;
    public AudioSource BigMonsterIdleReverb;
    public AudioSource BigMonsterInvestigatingReverb;
    public AudioSource BigMonsterRunningReverb;
    public AudioSource BigMonsterWalkingReverb;
    public AudioSource BigMonsterBulletHurt1;
    public AudioSource BigMonsterBulletHurt2;
    public AudioSource BigMonsterBulletHurt3;

    private PauseMenu PM;
    private GameObject gameOverScreen;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>(); // SAMU: ANIMAATIOON
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = (PlayerController)FindObjectOfType(typeof(PlayerController)); //SAMU: MOVED TO AWAKE
        PM = (PauseMenu)FindObjectOfType(typeof(PauseMenu));
        gameOverScreen = GameObject.Find("Canvas/GameOvah/GameOverText");
    }

    void Start()
    {
        waitTime = Random.Range(startWaitTimeMin, startWaitTimeMax);
        GiveUpTime = Random.Range(startGiveUpTimeMin, startGiveUpTimeMax);
        InvestigateTime = Random.Range(startInvestigateTimeMin, startInvestigateTimeMax);
        randomSpot = Random.Range(0, moveSpots.Length);
        // playerNoiseDistance = 1000;
        Physics2D.queriesStartInColliders = false;

        monsterIsPatrolling = true;
        GiveUpTime = 0;
        InvestigateTime = 0;

        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        monsterAIHandler();
        monsterHearing();
        monsterSounds();
        anim.SetFloat("velocity", agent.velocity.magnitude); //SAMU: ANIMAATIOON

        if (attackTimer < 10.0f) // Päivittää attackTimeria niin että cooldown on 2sekunttia, sekä lopettaa turhan päivittämisen jos monsteri ei hyökkää (Laskee vain 10)
        {
            attackTimer += Time.deltaTime;
        }
    }

    void monsterAIHandler()
    {

        if (monsterChasing == true)
        {
            
            anim.SetBool("chasing", true); //SAMU: ANIMAATIO
            agent.speed = monsterSpeedChasing;
            monsterIsPatrolling = false;
            monsterIsInvestigating = false;
            waitTime = Random.Range(startWaitTimeMin, startWaitTimeMax);
            GiveUpTime = Random.Range(startGiveUpTimeMin, startGiveUpTimeMax);
            agent.SetDestination(player.transform.position);
            CameraController.instance.shakeDuration = 10f;
            CameraController.instance.shakeAmount = 8f;
        }
        else
        {
            anim.SetBool("chasing", false); //SAMU: ANIMAATIO
            if (GiveUpTime <= 0 && InvestigateTime <= 0)
            {
                monsterIsPatrolling = true;
                monsterIsInvestigating = false;
                agent.SetDestination(moveSpots[randomSpot].transform.position);
               // CameraController.instance.shakeAmount = 0.7f;
            }
            else
            {
                monsterIsPatrolling = false;
                GiveUpTime -= Time.deltaTime;
            }

            if (monsterIsPatrolling == true)
            {
                agent.speed = monsterSpeed;
                //eyes.transform.Rotate(0f, 120f * Time.deltaTime, 0f); // SAMU: Esimerkki miten kääntää näkökenttää
                if (Vector3.Distance(transform.position, moveSpots[randomSpot].position) < 4f)
                {
                    if (waitTime <= 0)
                    {
                        waitTime = Random.Range(startWaitTimeMin, startWaitTimeMax);
                        randomSpot = Random.Range(0, moveSpots.Length);
                        agent.speed = monsterSpeed;
                    }
                    else
                    {
                        waitTime -= Time.deltaTime;
                        stopped = monsterSpeed - monsterSpeed;
                        agent.speed = stopped;
                    }
                }
            }  
            else
            {
                monsterIsPatrolling = false;
            }

            if (monsterIsInvestigating == true)
            {
                if (Vector3.Distance(transform.position, investigateSpot) < 4f)
                {
                    if (InvestigateTime <= 0)
                    {

                        agent.speed = monsterSpeed;
                        monsterIsPatrolling = true;
                        monsterIsInvestigating = false;
                        
                    }
                    else
                    {
                        InvestigateTime -= Time.deltaTime;                        
                        agent.speed = stopped;
                    }
                }
            }
        }
    }

    public void monsterInvestigating()
    {
        
        monsterIsPatrolling = false;

        if (monsterChasing == false)
        {
            agent.SetDestination(investigateSpot);
            agent.speed = monsterSpeedInvestigating;
            monsterIsInvestigating = true;        
        }
    }


    public void monsterSight()
    {

        monsterChasing = true;

        /*
        Ray ray = new Ray(transform.position, shitfuck2.transform.position);
        RaycastHit hit;

        Debug.DrawLine(transform.position, shitfuck2.transform.position * maxSightDistance, Color.red);

        if(Physics.Raycast(ray, out hit, maxSightDistance))
        {
            Debug.DrawLine(hit.point, hit.point + Vector3.up * 5, Color.green);
        }

        
        RaycastHit rayHit;
        if (Physics.Linecast(transform.position, player.transform.position, out rayHit))
        {

            print("hit " + rayHit.collider.gameObject.name);

            Debug.Log("Hello World");

            if (rayHit.collider.gameObject.CompareTag("Player"))
            {
                monsterChasing = true;
            }
        }

        
        Debug.DrawLine(shitfuck.transform.position, shitfuck2.transform.position, Color.red);
        if (Physics.Linecast(shitfuck.transform.position, shitfuck2.transform.position, 1 << LayerMask.NameToLayer("Player")))
        {
            Debug.Log("Spotted");
        }
   
        Debug.DrawLine(transform.position, player.transform.position, Color.green);
        
          RaycastHit hitInfo;
          var ray = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out hitInfo, distance);

          if (hitInfo.collider != null)
          {
              Debug.DrawLine(transform.position, hitInfo.point, Color.red);

              lineOfSight.SetPosition(1, hitInfo.point);
              lineOfSight.colorGradient = redColor;

              if (hitInfo.collider.CompareTag("Player"))
              {
                  monsterChasing = true;
                  Debug.Log("He sees u!");
              }
          }
          else
          {
              Debug.DrawLine(transform.position, transform.position + transform.right * distance, Color.green);
              lineOfSight.SetPosition(1, transform.position + transform.right * distance);
              lineOfSight.colorGradient = greenColor;
          }

          lineOfSight.SetPosition(0, transform.position);
          */
    }

    void monsterHearing()
    {
        playerNoiseDistance = 1000;
        monsterHearsPlayer = false;

        if (playerController.playerIsRunning == true)
        {
            playerNoiseDistance = CalculatePathLenght(player.transform.position);
        }

        if(playerController.playerIsShooting == true)
        {
            playerNoiseDistance = CalculatePathLenght(player.transform.position);
            playerNoiseDistance = playerNoiseDistance - playerController.ShootingNoiseAmount;
        }
        
        if(playerController.quickLooting == true)
        {
            playerNoiseDistance = CalculatePathLenght(player.transform.position);
            playerNoiseDistance = playerNoiseDistance - playerController.QuickLootNoiseAmount;
        }

        if (playerNoiseDistance < enemyHearingDistance)
        {
            InvestigateTime = Random.Range(startInvestigateTimeMin, startInvestigateTimeMax);
            GiveUpTime = Random.Range(startGiveUpTimeMin, startGiveUpTimeMax);
            investigateSpot = player.transform.position;
            monsterHearsPlayer = true;
            monsterIsPatrolling = false;
            monsterIsInvestigating = true;
            monsterInvestigating();
            Debug.Log("He hears u!");
            
        }
    }

    float CalculatePathLenght(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();

        if (agent.enabled)
        {
            agent.CalculatePath(targetPosition, path);
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


    void monsterSounds()
    {

        if (agent.speed > 0 && (monsterIsPatrolling == true || monsterIsInvestigating == true || monsterChasing == true))
        {
            if(BigMonsterWalkingReverb.isPlaying == false)
            BigMonsterWalkingReverb.Play();
        }
        else
        {
            BigMonsterWalkingReverb.Stop();
        }


        if (monsterIsPatrolling == true)
        {
            AudioManager.instance.StopSlow("BigMonsterChasingShake");
            if (BigMonsterBreathingReverb.isPlaying == false)
            BigMonsterBreathingReverb.Play();        
        }
        else
        {
            BigMonsterBreathingReverb.Stop();
        }
        
        
        if (monsterIsInvestigating == true && monsterChasing == false)
        {
            AudioManager.instance.StopSlow("BigMonsterChasingShake");
            if(BigMonsterInvestigatingReverb.isPlaying == false)
            BigMonsterInvestigatingReverb.Play();

        }
        else
        {
            BigMonsterInvestigatingReverb.Stop();
        }

        if (monsterChasing == true)
        {
            AudioManager.instance.PlayOneAtTime("BigMonsterChasingShake");
            if(BigMonsterChasing.isPlaying == false)
            BigMonsterChasing.Play();
            if(BigMonsterRunningReverb.isPlaying == false)
            BigMonsterRunningReverb.Play();

        }
        else
        {
            BigMonsterChasing.Stop();
            BigMonsterRunningReverb.Stop();
        }

        if (monsterIsPatrolling == false && monsterIsInvestigating == false && monsterChasing == false)
        {
            if(BigMonsterIdleReverb.isPlaying == false)
            BigMonsterIdleReverb.Play();
        }
        else
        {
            BigMonsterIdleReverb.Stop();
        }


        if (PM.GameIsPaused == true || gameOverScreen.activeSelf == true)
        {
            BigMonsterBreathingReverb.Stop();
            BigMonsterChasing.Stop();
            BigMonsterHit.Stop();
            BigMonsterIdleReverb.Stop();
            BigMonsterInvestigatingReverb.Stop();
            BigMonsterRunningReverb.Stop();
            BigMonsterWalkingReverb.Stop();
            BigMonsterBulletHurt1.Stop();
            BigMonsterBulletHurt2.Stop();
            BigMonsterBulletHurt3.Stop();
        }
    }


    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            monsterChasing = true;
            if (BigMonsterHit.isPlaying == false)
            {
                BigMonsterHit.Play();
                if (attackTimer > timeBetweenAttacks)
                {
                    attackTimer = 0;
                    player.GetComponent<PlayerHealthManager>().HurtPlayer(damageToGive);
                }
               
                Debug.Log("Player died!");
            }
        }

        if (collider.gameObject.tag == "Bullet")
        {
            monsterChasing = true;

            int index = Random.Range(0, 3);
            if (index == 0)
            {
                if(BigMonsterBulletHurt1.isPlaying == false)
                BigMonsterBulletHurt1.Play();
            }
            if (index == 1)
            {
                if(BigMonsterBulletHurt2.isPlaying == false)
                BigMonsterBulletHurt2.Play();
            }
            if (index == 2)
            {
                if(BigMonsterBulletHurt3.isPlaying == false)
                BigMonsterBulletHurt3.Play();
            }
        }
    }
}
