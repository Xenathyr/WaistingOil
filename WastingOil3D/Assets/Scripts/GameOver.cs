using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    public void QuitToMenu()
    {
        Debug.Log("QUIT");
        SceneManager.LoadScene("MainMenu");
    }
}
