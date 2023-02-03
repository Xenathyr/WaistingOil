using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author / Samu Haaja

//v.0.0.1 (23.1) Created as suggested to me. Pool created and reworked functionality with PlayerGun and ProjectileManager to fit the code
//v.0.0.2 (24.1) Fixed glitch where bullets fly out of the pool upon returning "shouldMove functionality"
//v.0.0.3 (31.1) Separate pool functionality for player,enemies and bosses
//v.0.0.4 (6.2) Small reformating


public class ProjectilePool : MonoBehaviour {
    //Pooling muuttaa lentävien objektejen käsittelyä kevyemmäksi
    // Projectilet on jo olemassa, eikä niitä tarvitse jatkuvasti generoida/tuhota, sen sijaan niiden sijainti on niin sanottu "Pool"
    // Tästä Pool tilasta, ne voidaan asettaa lentämään ampujan radalla, kunnes ne siirtyvät takaisin pooliin elinkaarensa lopussa.

    [SerializeField]
    float poolSize = 10;
    
    [SerializeField]
    GameObject projectilePrefab;

    private List<ProjectileManager> projectilesInPool;
    public static ProjectilePool Instance;


    

    void Awake ()
    {
        Instance = GetComponent<ProjectilePool>();
	}

    void Start()
    {
        InitializePool();
    }
    // Update is called once per frame
    void Update () {
		
	}

    public ProjectileManager Instantiate (Vector3 position, Quaternion rotation)
    {
        ProjectileManager _projectile = projectilesInPool[0];
        _projectile.transform.position = position;
        _projectile.transform.rotation = rotation;
        projectilesInPool.Remove(_projectile);
        _projectile.firingPoint = _projectile.transform.position;
        return _projectile;
    }

    public void ReturnToPool(ProjectileManager _projectile)
    {
        _projectile.transform.position = transform.position;
        _projectile.GetComponent<Rigidbody>().velocity = Vector3.zero;
        projectilesInPool.Add(_projectile);
        _projectile.shouldMove = false;
    }
    void InitializePool()
    {
        projectilesInPool = new List<ProjectileManager>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject _projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
            projectilesInPool.Add(_projectile.GetComponent<ProjectileManager>());

        }
    }
}
