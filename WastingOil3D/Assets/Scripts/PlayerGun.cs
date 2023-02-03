using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author /Samu Haaja - partly Antti Koponen

//v.0.0.1 (17.1.2019) Added serializefielded values, and a working Shoot input 
//v.0.0.2 (23.1.2019) Some big changes to the code so it works properly with ProjectilePool (Instead of spawning bullets, it pulls them from the pool)
//v.0.0.3 (24.1.2019) Turning of player through mouse has been moved to HandleGunRotationInput and now turns a hidden playergun inside the player sprite instead (In this script)

public class PlayerGun : MonoBehaviour
{
    [SerializeField]
    Transform firingPoint;

    // [SerializeField] mahdollistaa arvojen vaihtamisen unityssä Privatesta huolimatta
    [SerializeField]
    float firingSpeed = 0.5f;
    public float Shootshake = 10f;
    public float ShootshakeDuration = 0.1f;
    private ProjectileManager projectilemanager;
    private PlayerController PlayerController;
    public static PlayerGun Instance; // static, jolloin aseeseen pääsee käsiksi muualta
    private float lastTimeShot = 0;
    public ParticleSystem muzzleflash;
    public Inventory ammo;

    private PauseMenu PM;

    private void Update()
    {
        // HandleGunRotationInput();
    }



    void Awake() // Samu Haaja //v.0.0.1 (17.1.2019)
    {
        firingPoint = GetComponentInChildren<Transform>();
        Instance = GetComponent<PlayerGun>(); //Referoi class PlayerGun	
        PlayerController = (PlayerController)FindObjectOfType(typeof(PlayerController));
        PM = (PauseMenu)FindObjectOfType(typeof(PauseMenu));
        projectilemanager = (ProjectileManager)FindObjectOfType(typeof(ProjectileManager));
        muzzleflash = GetComponentInChildren<ParticleSystem>();
    }

    public void Shoot() // Samu Haaja //v.0.0.1 (17.1.2019)
    {
        projectilemanager.bulletTrail.Clear();
        if (PM.GameIsPaused == false)
        {
            if (lastTimeShot + firingSpeed < Time.time)
            {
                lastTimeShot = Time.time;
                CameraController.instance.shakeDuration = 0.1f; //ShootshakeDuration;
                CameraController.instance.shakeAmount = 12f; //Shootshake;
                muzzleflash.Play();
                ProjectileManager _projectile = ProjectilePool.Instance.Instantiate(firingPoint.position, firingPoint.rotation); // Call instance, factor position and rotation                                                                                                                      // _projectile.GetComponent<AudioSource>().PlayOneShot(_projectile.GetComponent<AudioSource>().clip);
                _projectile.Move();     
                ammo.ReduceAmmo();
                AudioManager.instance.Play("Gun_Fire");
                AudioManager.instance.Play("Shells_fall");

            }
        }
    }
    //public void HandleGunRotationInput() // Samu Haaja //v.0.0.3 (24.1.2019)
    //{
    //    RaycastHit _hit;
    //    Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Follow mouse

    //    if (PM.GameIsPaused == false)
    //    {
    //        if (PlayerController.playerIsRunning == false)
    //        {
    //            if (Physics.Raycast(_ray, out _hit))
    //            {
    //                transform.LookAt(new Vector3(_hit.point.x, transform.position.y, _hit.point.z));
    //            }
    //        }
    //    }
    //}
}
