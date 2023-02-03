using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author /Samu Haaja - partly Antti Koponen

//v.0.0.1 (17.1) Added basic functionality, bullets spawn and fly forward for a set distance [SerializeFields]
//v.0.0.2 (23.1) Major refactoring of the code to work properly with ProjectilePool script (Bullets no longer spawned, bullets no longer deleted upon flying too far)
//v.0.0.3 (24.1) ProjectlePool bug fixed (added "shoudMove bool so that the bullet recognizes when it is in pool and should be static)
//v.0.0.4 (30.1) Damage values added, break on collision with object or enemy added aswell
//v.0.0.5 (31.1) Added bools isEnemy, player, boss and their respective Pools to help Unity distinct between pools for each object ingame)
//v.0.0.6 (6.2) Realised that bullets were still being destroyed upon collision, changed to ReturnToPool functionality 
//v.0.0.7 (6.2) Antti Koponen: Audio
//v.0.0.8 (20.2) Added separate collision functions based on shooter
//v.0.0.9 (21.2) Added Boss damage taking mechanic through BossController
//v.0.0.10 (21.2) Removed maximum projectile distance as colliders and walls now control it. // Reverted as of 23.2
//v.0.0.11 (23.2) HurtEnemy function has been moved to "HurtPlayer" script for simplification. Minus the name
public class ProjectileManager : MonoBehaviour
{
    public Vector3 mousePosition;
    public Vector3 firingPoint;
    // private Vector3 target;
    [SerializeField] // [SerializeField] mahdollistaa arvojen vaihtamisen unityssä Privatesta huolimatta
    private float projectileSpeed = 70;
    [SerializeField]
    private float maxProjectileDistance = 100f;
    [SerializeField]
    // private int damageToGive = 1;
    public TrailRenderer bulletTrail;
    private bool trailActive;
    
    [SerializeField]
    private float currentProjectileDistance;
    public bool shouldMove = false;
    public GameObject _projectile;
    [SerializeField]
    private bool isPlayer = true;

    public GameObject playerPool;

    //public AudioClip[] sounds; 
    //AudioSource audioSouce;    

    void Awake()
    {
        // audioSouce = GetComponent<AudioSource>(); 

        if (isPlayer == true)
        {
            playerPool = GameObject.FindGameObjectWithTag("playerPool");
        }
    }

    void Start()
    {
        firingPoint = transform.position;      
    }
    
    // Update is called once per frame
    void Update()
    {
       
        if (shouldMove == true)
        {
            MoveProjectile();
        }

    }

    public void Move()
    {
        shouldMove = true;
    }

    void MoveProjectile()
    { 
        if (Vector3.Distance(firingPoint, transform.position) > maxProjectileDistance) // If bullet flies too far, its returned to pool
        {
            Debug.Log("returned");
            if (isPlayer == true)
            {
                playerPool.GetComponent<ProjectilePool>().ReturnToPool(this);
            }
        }
        else // It flies forward
        {
            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {  //IMPORTANT Remember to tag enemies as ENEMY (Unity tag) 
        if (isPlayer == true)
        {
            if (other.gameObject.tag == "smallMonster")
            {             
                playerPool.GetComponent<ProjectilePool>().ReturnToPool(this);
            }
            if (other.gameObject.tag == "Wall")
            {
                playerPool.GetComponent<ProjectilePool>().ReturnToPool(this);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {  //IMPORTANT Remember to tag enemies as ENEMY (Unity tag) 
        if (isPlayer == true)
        {
            if (other.gameObject.tag == "smallMonster")
            {
                playerPool.GetComponent<ProjectilePool>().ReturnToPool(this);
            }
            if (other.gameObject.tag == "Wall")
            {
                playerPool.GetComponent<ProjectilePool>().ReturnToPool(this);
            }
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        CollisionDetection(other);
    }
    private void OnCollisionExit(Collision other)
    {
        CollisionDetection(other);
    }
    private void OnCollisionStay(Collision other)
    {
        CollisionDetection(other);
    }
    private void CollisionDetection(Collision other)
    {
        if (other.gameObject.tag == "smallMonster")
        {
            playerPool.GetComponent<ProjectilePool>().ReturnToPool(this);
        }
        if (other.gameObject.tag == "Wall")
        {
            playerPool.GetComponent<ProjectilePool>().ReturnToPool(this);
        }
    }
}


