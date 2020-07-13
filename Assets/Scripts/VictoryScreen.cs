using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit(); //Quit Game
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0); //Loads the very first scene in the build index, aka the Main Menu.
        }
    }
}
