using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    //Load the main game scene
    public void LoadGame()
    {
        SceneManager.LoadScene("GameScene");

    }

    //Quit the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
