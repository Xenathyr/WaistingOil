using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider loadingSlider;

    public void Start()
    {
        AudioManager.instance.PlayOneAtTime("MenuMusic");
    }

    public void PlayGame()
    {
        AudioManager.instance.Stop("MenuMusic");
        StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().buildIndex + 1));     
    }

    public void QuitGame()
    {
        Application.Quit();
    }      

    IEnumerator LoadAsynchronously (int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            loadingSlider.value = progress;
            yield return null;
        } 
    }

}
