using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author /Samu Haaja - partly Antti Koponen

//v.0.0.1 (4.4) Working camera behavior script. Remains unchanged as of 6.2.2019
//v.0.0.2 (4.4) Author:Antti Koponen - Camera Shake functionality, class made into singleton

public class CameraController : MonoBehaviour
{
    public static CameraController instance = null; //v.0.0.2 

    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 targetOffset = new Vector3(0, 20, 0);
    //[SerializeField] // [SerializeField] mahdollistaa arvojen vaihtamisen unityssä Privatesta huolimatta
    private float movementSpeed = 30;

    //v.0.0.2 Camera Shake functionality
    public float shakeDuration;// = 0f;    //v.0.0.2  - How long the object should shake for
    public float shakeAmount;// = 0.7f;    //v.0.0.2  - Amplitude of the shake. A larger value shakes the camera harder
    public float decreaseFactor = 1.0f; //v.0.0.2  - Shake decrease time
    private Vector3 originalPos;        //v.0.0.2  - Stores camera's coordinates before shakes and follows
    private Vector3 offset;             //v.0.0.2  - Offset distance between the player and camera     
    public float smoothSpeed = 0.125f;  //v.0.0.2  - How smoothly camera follows Player
    public int cameraZ = -10;           //v.0.0.2  - Distance of the camera from the world

    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); //IT WORKS??!?!?
        //v.0.0.2 (14.2) Antti Koponen class made into singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    
    void Update()
    {
        MoveCamera(); 
    }

    void MoveCamera()
    {
        //v.0.0.2 (14.2) Antti Koponen 
        if (shakeDuration > 0)
        {
            // Get player position
            originalPos = new Vector3(PlayerController.instance.transform.localPosition.x, transform.position.y, PlayerController.instance.transform.localPosition.z);
            // Randomize new position inside sphere shape, which size is calculated using shakeAmount
            transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount * Time.deltaTime;
            // Calculate time to stop shaking
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            // Move to target(player) position
            transform.position = Vector3.Lerp(transform.position, target.position + targetOffset, movementSpeed * Time.deltaTime);
        }

    }

}