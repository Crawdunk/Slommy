using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private GameManager gm;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>(); // Gets the game manager script.
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            gm.lastCheckpointPos = transform.position; //On colliding with the trigger of the checkpoint, it sets the position of the last checkpoint to the current checkpoint position.
        }


    }

}
