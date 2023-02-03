using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flare : MonoBehaviour
{
    public float fadeDuration = 2.0f;
    public AudioSource FlareStart;
    public AudioSource FlareEnd;
    private PauseMenu PM;
    public GameObject gameOverScreen;

    public float maxViewRadius;

    private float t = 0.0f;

    private bool active;

    // Start is called before the first frame update
    void Start()
    {
        maxViewRadius = GetComponentInParent<FieldOfView>().viewRadius;
        active = true;
        GetComponentInParent<FieldOfView>().viewRadius = 0;

        StartCoroutine(Fade());
        PM = (PauseMenu)FindObjectOfType(typeof(PauseMenu));
        gameOverScreen = GameObject.Find("Canvas/GameOvah");
    }

    private void Update()
    {
        if (active == true)
        {
            GetComponentInParent<FieldOfView>().viewRadius = Mathf.Lerp(0, maxViewRadius, t);
            t += (0.5f * Time.deltaTime) * 2;
        }
        else
        {
            GetComponentInParent<FieldOfView>().viewRadius = Mathf.Lerp(maxViewRadius, 0, t);
            t += (0.5f * Time.deltaTime) * 2;
        }

        if (PM.GameIsPaused == true /*|| gameOverScreen.transform.GetChild(0).gameObject.activeSelf == true*/)
        {
            FlareStart.volume = 0;
            FlareEnd.volume = 0;
        }
        else
        {
            FlareStart.volume = 1;
            FlareEnd.volume = 1;
        }

    }

    public IEnumerator Fade()
    {
        FlareStart.Play();
        float fadespeed = (float)1.0 / fadeDuration;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * fadespeed)
        {

            yield return true;
        }
        FlareEnd.Play();
        yield return new WaitForSeconds(1f);
        FlareStart.Stop();
        active = false;
        t = 0;
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
            
    }

}
