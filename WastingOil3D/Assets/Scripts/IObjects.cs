using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IObjects : MonoBehaviour
{
    private Animator _animator; // Animator for door

    public bool isDoor;
    private bool doorOpened;
    public bool isCabin;
    public bool isCorpse; //Bools to determinate which object the player is interacting with
    public bool isDesk;
    public bool isBox;
    public bool isStairs;
    public bool isRadio;
    public bool isChopper;
    public bool isBook;
    public bool playerIsSmashing;
    public float SmashingNoiseAmount = 5;
    public SpawnerScript spawner;


    //Text for interaction buttons

    public Looting looting;
    public Inventory inventory;
    public GetToDahChoppah choppah;
    public GameObject page;

    private void Awake()
    {

        spawner = (SpawnerScript)FindObjectOfType(typeof(SpawnerScript));
    }
    // Start is called before the first frame update
    void Start()
    {
        //if (isBox == true || isStairs == true || isRadio == true || isChopper == true) //This "purkka koodi" exists to get around the error because these didn't have animator
        //{
           
        //}
        //else
        //{
        //    _animator = GetComponent<Animator>();
        //    _animator.SetBool("open", false);
        //}
    }

    void OnTriggerStay(Collider other)
    {


        if (other.tag == "Player") //If player enters the interaction range
        {
            if (isDoor == true && gameObject.GetComponent<SpriteRenderer>().enabled == true) // If the object is a door
            {
                
                    if (other.GetComponent<Inventory>().obtainedKey == true )
                    {
                        other.GetComponent<PlayerController>().InteractionText.text = ("Door was opened");
                        Debug.Log("OPEN SESAME STREET!");
                    // _animator.SetBool("open", true);
                    // doorOpened = true;
                    gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    
                    }
                    else
                    {
                        other.GetComponent<PlayerController>().InteractionText.text = ("It's locked, I need to find a key");
                        Debug.Log("ISSA LOCKED!");
                    }
                    //other.GetComponent<PlayerController>().InteractionText.text = ("[E] to Open");
                
                if(doorOpened == true)
                {
                    other.GetComponent<PlayerController>().InteractionText.text = (" ");
                }

                ; // Text shown in UI

            }
            if (isBox == true)
            {
                if (GetComponent<Looting>().isLooted == false && other.GetComponent<PlayerController>().isLooting == false)
                {
                    HideShowButtons(true, other);
                    other.GetComponent<PlayerController>().InteractionText.text = ("       Search carefully \n\rSmash open");
                    
                }
                if (Input.GetKey(KeyCode.E) && other.GetComponent<PlayerController>().isLooting == false) // Looting objects slowly
                {
                    PlayerController player = other.GetComponent<PlayerController>();
                    looting.LootingObject(false);
                    if (GetComponent<Looting>().isLooted == false)
                    {
                        other.GetComponent<PlayerController>().isLooting = true;
                        StartCoroutine("Loottimer", player);
                        other.GetComponent<PlayerController>().InteractionText.text = (" ");
                        HideShowButtons(false, other);
                        
                    }

                }
                if (Input.GetKey(KeyCode.Q) && other.GetComponent<PlayerController>().isLooting == false) //Looting objects quickly  (Sound)
                {
                    if (GetComponent<Looting>().isLooted == false)
                    {
                        PlayerController player = other.GetComponent<PlayerController>();
                        playerIsSmashing = true;
                        player.GetComponent<PlayerController>().quickLooting = true;
                        other.GetComponent<PlayerController>().isLooting = true;
                        StartCoroutine("Smashtimer", player); // Tämän ainakin suorittaa
                        other.GetComponent<PlayerController>().InteractionText.text = (" ");
                        HideShowButtons(false, other);
                        looting.LootingObject(true);
                    }
                }                                                           
            }
            else if (isStairs == true)
            {
                other.GetComponent<PlayerController>().InteractionText.text = ("             Go through the stairs");
                ShowE(other);
                if (Input.GetKey(KeyCode.E))
                {
                    GetComponent<Stairs>().climbStairs(other);
                }
            }
            if (isRadio == true && choppah.choppaCalled == false)
            {
                if(inventory.obtainedKey == true)
                {
                    other.GetComponent<PlayerController>().InteractionText.text = (" Call for help");
                    ShowE(other);
                    if (Input.GetKey(KeyCode.E))
                    {
                        choppah.choppaCalled = true;
                        HideShowButtons(false, other);
                        other.GetComponent<PlayerController>().InteractionText.text = ("Chopper has been called to the top floor!");
                        spawner.timeBtwSpawns = spawner.timeBtwSpawns - 2; //This part is giving an error, good sir.
                    }
                }
                else
                {
                    other.GetComponent<PlayerController>().InteractionText.text = ("I need to find a key to operate this");
                }
                
            }
            if (isChopper == true)
            {
                other.GetComponent<PlayerController>().InteractionText.text = (" Win the game");
                ShowE(other);
                if (Input.GetKey(KeyCode.E))
                {
                    HideShowButtons(false, other);
                    SceneManager.LoadScene("WinningScene");
                }
            }
            if (isBook == true && page.activeSelf == false)
            {
                other.GetComponent<PlayerController>().InteractionText.text = (" Hold to Read");
                ShowE(other);
                if (Input.GetKey(KeyCode.E))
                {
                    page.SetActive(true);
                }
            }
            else if (isBook == true && page.activeSelf == true)
            {
                if (Input.GetKeyUp(KeyCode.E))
                {
                    page.SetActive(false);
                }
            }
        }
    }



    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") //If player enters the interaction range
        {
            //playerSprite.GetComponent<SpriteRenderer>().sortingOrder = 2;
            other.GetComponent<PlayerController>().InteractionText.text = (" ");

            HideShowButtons(false, other);
        }
        if (isBook == true && page.activeSelf == true)
        { 
                page.SetActive(false);
        }
    }


     IEnumerator Loottimer(PlayerController player)
    {
        yield return new WaitForSeconds(2);  
        player.isLooting = false;
    }
    IEnumerator Smashtimer(PlayerController player)
    {
        yield return new WaitForSeconds(0.5f);
        player.isLooting = false;
        playerIsSmashing = false;
    }

    void HideShowButtons(bool showing, Collider other)
    {
        Image[] buttonImages = other.GetComponent<PlayerController>().InteractionText.GetComponentsInChildren<Image>();

        for (int i = 0; i < buttonImages.Length; i++)
        {
            buttonImages[i].enabled = showing;
        }
    }

    void ShowE(Collider other)
    {
        Image[] buttonImages = other.GetComponent<PlayerController>().InteractionText.GetComponentsInChildren<Image>();

        for (int i = 0; i < buttonImages.Length; i++)
        {
            if(buttonImages[i].name == "E") buttonImages[i].enabled = true;
        }
    }



}
