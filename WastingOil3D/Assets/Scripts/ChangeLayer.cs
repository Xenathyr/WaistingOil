using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLayer : MonoBehaviour
{
    //public bool visible;

    //public FieldOfView fov;

    //public float timer = 0.1f;

    void Update()
    {
        

        //if (visible == true)
        //{
        //    GetComponent<SpriteRenderer>().sortingOrder = 10;
        //}
        //else
        //{
        //    GetComponent<SpriteRenderer>().sortingOrder = 8;
        //}

        

        //if (timer > 0f) timer -= Time.deltaTime;

        //for (int i=0; i<fov.visibleTargets.Count; i++)
        //{
        //    if (fov.visibleTargets[i].name.StartsWith("Box"))
        //    {
        //        Debug.Log("ASDKPOSADK");
        //        timer = 0.1f;
                
        //    }
        //    else
        //    {
        //        Debug.Log("SALAD!");
        //    }
        //    Debug.Log(fov.visibleTargets[i].name);
        //}

        

        //if(timer < 0f)
        //{
        //    visible = false;
        //}

        
    }

    //private void OnTriggerEnter(Collider other)
    //{
    // if (other.gameObject.tag == "purkkalayer" )
    //    {
    //        GetComponent<SpriteRenderer>().sortingOrder = 8;
    //    }
    //}
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "purkkalayer")
        {
            GetComponent<SpriteRenderer>().sortingOrder = 8;
        }
    }




}
