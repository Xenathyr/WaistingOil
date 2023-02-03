using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
        
    //public GameObject flare;

    public GameObject[] ItemList;

    //public List<GameObject> itemList = new List<GameObject>();

   
    [Header("0=flare, 1=health")]
    public int EquippedItem;

    public Sprite m_Herb;
    public Sprite m_Flare;

    [Header("Carried Item Count")]
    public int flareCount;
    public int ammoCount;
    public int healthpickupCount;

    public int ammoClip;
    public int maxAmmoClip;


    //How many of each objects is in the game
    [Header("Loot Table")]
    public int totalFlares;
    public int totalAmmos;
    public int totalHealthpickups;
    public int totalEmpty;

    [Header("UI")]
    public Text text;
    //public Image activeItem;
    public GameObject itemUIGroup;
    public Text itemcount;

    public Image flareIcon;
    public Image medikitIcon;

    public Image reloadBar;

    public PlayerHealthManager healing;

    public Animator itemAnim;
    public Animator reloadCursor;

    public bool reloading;

    public bool obtainedKey;

    public BulletUI bulletUI;

    public PauseMenu pausemenu;

    private void Start()
    {
        reloading = false;
        ChangeItem();
        ammoClip = maxAmmoClip;
        text.text = ammoClip + " \\ " + ammoCount;
        pausemenu = (PauseMenu)FindObjectOfType(typeof(PauseMenu));
    }

    void DropFlare()
    {
        flareCount -= 1;
        Debug.Log("Used " + ItemList[0]);
        ItemList[0].transform.position = new Vector3(0.90f, 0.0f, 0.0f);
        //GameObject flareClone = (GameObject)Instantiate(flare, transform.position, flare.transform.rotation);
        GameObject ItemListclone = (GameObject)Instantiate(ItemList[0], transform.position, ItemList[0].transform.rotation);
        itemcount.text = "" + flareCount;


    }

    void useHealthPickUp()
    {
        healing.HealPlayer();
        Debug.Log("Omnommed some copyright free herbs");
        healthpickupCount -= 1;
        itemcount.text = "" + healthpickupCount;
        AudioManager.instance.Play("PlayerUseMedkit");
    }

    public void ReduceAmmo()
    {
        //--ammoCount;
        --ammoClip;
        text.text = ammoClip + " \\ " + ammoCount;

        bulletUI.ammoUpdate();
    }

    public void AddAmmo()
    {
        ammoCount += 5;
        text.text = ammoClip + " \\ " + ammoCount;
        bulletUI.ammoUpdate();
    }

    public IEnumerator Reload()
    {
        if (ammoClip < 8 && ammoCount > 0)
        {
            

            reloading = true;          
            AudioManager.instance.Play("Reload");
            reloadBar.enabled = true;
            reloadBar.GetComponentInChildren<Text>().enabled = true;
            reloadCursor.Play("ReloadCursor");
            yield return new WaitForSeconds(2f);
            reloadBar.enabled = false;
            reloadBar.GetComponentInChildren<Text>().enabled = false;
            reloading = false;

        }
        else
        {
            yield return new WaitForSeconds(0f);
        }

        

        int newClip = maxAmmoClip;

        ammoCount += ammoClip;

        if (newClip > ammoCount)
        {
            newClip = ammoCount;

        }

        
        
        ammoCount -= newClip;

        ammoClip = newClip;
        text.text = ammoClip + " \\ " + ammoCount;

        bulletUI.ammoUpdate();
    }

        // Update is called once per frame
        void Update()
    {
        if (GetComponent<PlayerController>().isDead == false && pausemenu.GameIsPaused == false )
        {

            if (Input.GetButtonDown("Fire2") && EquippedItem == 0)
            {
                if (flareCount > 0)
                {
                    DropFlare();
                }
            }
            else if (Input.GetButtonDown("Fire2") && EquippedItem == 1)
            {
                if (healthpickupCount > 0)
                {
                    useHealthPickUp();
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                EquippedItem = 0;
                ItemChange();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                EquippedItem = 1;
                ItemChange();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (reloading == false)
                {
                    StartCoroutine(Reload());
                }
            }
        }
    }

    void ItemChange()
    {
        
        StartCoroutine(ItemFucker());

    }

    private IEnumerator ItemFucker()
    {

        itemAnim.Play("ItemOut");

        yield return new WaitForSeconds(0.2f);

        ChangeItem();
        

    }

    void ChangeItem()
    {
        if (EquippedItem == 0)
        {
            itemcount.text = "" + flareCount;
            //activeItem.sprite = m_Flare;
            flareIcon.enabled = true;
            medikitIcon.enabled = false;
            itemUIGroup.SetActive(true);


        }
        else if (EquippedItem == 1)
        {
            itemcount.text = "" + healthpickupCount;
            //activeItem.sprite = m_Herb;
            flareIcon.enabled = false;
            medikitIcon.enabled = true;
            itemUIGroup.SetActive(true);

        }
        else
        {
            itemUIGroup.SetActive(false);
        }
    }

    public void InventoryUpdate()
    {

        if(EquippedItem == 0)
        {
            itemcount.text = "" + flareCount;
        }
        else if (EquippedItem == 1)
        {
            itemcount.text = "" + healthpickupCount;
        }
        
    }
}
