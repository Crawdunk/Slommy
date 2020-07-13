using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public GameObject canvas;
    public GameObject player;
    private static GameManager instance;
    public Vector2 lastCheckpointPos;
    public float lives;
    bool gameHasEnded = false;

    public AudioSource deathNoise;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        } else
        {
            Destroy(gameObject);
        } //Make Singleton
    }
    
    void Start()
    {
        lives = 3;    //Set the lives to 3 as the GameManager starts.
    }

    void Update()
    {
        player = GameObject.FindWithTag("Player");
        canvas = GameObject.FindWithTag("Canvas"); //Grabs the gameobjects for player and canvas by tag.

        if(lives <= 0)
        {
            EndGame(); //Calls the EndGame function if lives is less than or equal to 0.
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit(); //Quit the game when esc is presed.
        }
    }

    public void EndGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            Destroy(player); //Destroy the player
            canvas.GetComponent<Canvas>().enabled = true; //Enable the game over canvas.
            Invoke("Restart", 2f); //Start the restart function after 2 seconds.
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(1); //Load the 2nd scene in the build index which is Level 1.
        lives = 3; //Resets the lives to 3.
        gameHasEnded = false;
        canvas.GetComponent<Canvas>().enabled = false; //Disables the game over canvas.
    }

    
}
