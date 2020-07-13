using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameManager gm;

    void Start()
    {
        gm = GameObject.FindWithTag("GM").GetComponent<GameManager>(); //Gets the GameManager script so we can access the lives.
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        gm.lives = 3; //Loads the next scene and makes sure to set the lives in the GAme Manager to 3.
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the game on press of the quit button
    }

}
