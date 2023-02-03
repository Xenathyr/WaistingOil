using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletUI : MonoBehaviour
{
    public Image[] bulletsUI;
    public Inventory bulletamount;


    public float FadeDuration = 1f;
    public Color Color1 = Color.white;
    public Color Color2 = Color.red;

    private Color startColor;
    private Color endColor;
    private float lastColorChangeTime;


    // Start is called before the first frame update
    void Start()
    {
        bulletsUI = GetComponentsInChildren<Image>();

        for (int i = 0; i < bulletsUI.Length; i++)
        {
            bulletsUI[i].color = new Color32(100, 100, 100, 100);
        }

        for (int i = 0; i < bulletamount.ammoClip; i++)
        {
            bulletsUI[i].color = new Color32(255, 255, 225, 255);
        }

        startColor = Color1;
        endColor = Color2;
    }

    private void Update()
    {
        if(GetComponentInChildren<Text>().enabled == true)
        {
            var ratio = (Time.time - lastColorChangeTime) / FadeDuration;
            ratio = Mathf.Clamp01(ratio);
            GetComponentInChildren<Text>().color = Color.Lerp(startColor, endColor, ratio);
            if (ratio == 1f)
            {
                lastColorChangeTime = Time.time;

                // Switch colors
                var temp = startColor;
                startColor = endColor;
                endColor = temp;
            }


        }
    }



    public void ammoUpdate()
    {
        for (int i = 0; i < bulletsUI.Length; i++)
        {
            bulletsUI[i].color = new Color32(100, 100, 100, 100);
        }

        for (int i = 0; i < bulletamount.ammoClip; i++)
        {
            bulletsUI[i].color = new Color32(255, 255, 225, 255);
        }

        if(bulletamount.ammoClip <= 0)
        {
            GetComponentInChildren<Text>().enabled = true;
            if(bulletamount.ammoCount > 0)
            {
                GetComponentInChildren<Text>().text = "Press R to Reload";
                GetComponentInChildren<Animator>().SetBool("OuttaAmmo", true);
            }
            else
            {
                GetComponentInChildren<Text>().text = "Out of Ammo";
                GetComponentInChildren<Animator>().SetBool("OuttaAmmo", false);
            }
        }
        else
        {
            GetComponentInChildren<Text>().enabled = false;
            
        }
    }
}
