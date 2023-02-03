using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FloatAndFade : MonoBehaviour {

    Text text;
    public float fadeDuration = 2.0f;
    public float speed = 2.0f;

	// Use this for initialization
	void Start () {
        text = this.GetComponent<Text>();
        StartCoroutine(Fade());
		
	}

    public IEnumerator Fade ()
    {
        float fadespeed = (float)1.0 / fadeDuration;
        Color c = text.color;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * fadespeed)
        {
            c.a = Mathf.Lerp(1, 0, t);
            text.color = c;
            yield return true;
        }

        Destroy(this.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.Translate(Vector3.up * Time.deltaTime * speed);
	}
}
