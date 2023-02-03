using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    private PlayerHealthManager HP;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && HP.currentHealth > 0)
        {
            if (GameIsPaused == true && optionsMenuUI.activeSelf == false)
            {
                Resume();
                AudioManager.instance.Play("PauseMenuDisappear");
            }
            else
            {
                if (optionsMenuUI.activeSelf == false)
                {
                    AudioManager.instance.Play("PauseMenuAppear");
                }
                Pause();
            }
        }
    }

    public void Start()
    {
        //Resume();
        HP = (PlayerHealthManager)FindObjectOfType(typeof(PlayerHealthManager));
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        GameIsPaused = false;
        Time.timeScale = 1f;
        AudioManager.instance.Play("PauseMenuDisappear");
        AudioManager.instance.StopFast("PauseMenuMusic");
    }

    public void Pause()
    {
        AudioManager.instance.PlayOneAtTime("PauseMenuMusic");
        
        pauseMenuUI.SetActive(true);
        optionsMenuUI.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}



