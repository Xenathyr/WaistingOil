using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegAnimation : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriterenderer;
    public PlayerController playerController;



    private PlayerHealthManager PlayerHPMANAG;

    // Start is called before the first frame updat
    private void Awake()
    {
        PlayerHPMANAG = (PlayerHealthManager)FindObjectOfType(typeof(PlayerHealthManager));
        animator = GetComponent<Animator>();
        spriterenderer = GetComponent<SpriteRenderer>();
        playerController = (PlayerController)FindObjectOfType(typeof(PlayerController)); 
}
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(playerController.playerIsRunning == true)
        {
            animator.SetBool("running", true);
        }
        else
        {
            animator.SetBool("running", false);
        }
        if (playerController.playerMoving == true)
        {
            animator.SetBool("moving", true);
        }
        else animator.SetBool("moving", false);
        //ESIMERKKI WORKS 
        //AudioManager.instance.PlayOneAtTime("PlayerRunning");

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            transform.eulerAngles = new Vector3(90, 45, 0);
        }
        else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S))
        {
            transform.eulerAngles = new Vector3(90, 315, 0);
        }
        else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S))
        {
            transform.eulerAngles = new Vector3(90, 225, 0);
        }
        else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W))
        {
            transform.eulerAngles = new Vector3(90, 135, 0);
           // animator.SetInteger("Direction", 9);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            //animator.SetInteger("Direction", 8);
            transform.eulerAngles = new Vector3(90, 90, 0);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.eulerAngles = new Vector3(90, 0, 0);
            // animator.SetInteger("Direction", 4);
            //sr.flipY = true;          
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.eulerAngles = new Vector3(90, 180, 0);
            //animator.SetInteger("Direction", 6);
            //sr.flipY = false;            
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.eulerAngles = new Vector3(90, 270, 0);
           // animator.SetInteger("Direction", 2);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

    }
}
