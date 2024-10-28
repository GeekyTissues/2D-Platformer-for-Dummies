using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    //Script for the box collider area detector
    public bool PlayerInArea { get; private set; }

    public Transform Player { get; private set; }

    [SerializeField] private string detectionTag = "Player";

    //Checks for any collisions in the detector and checks if the collision is the player
    //If collision is the player, the location of the player is saved and check for the player is true
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(detectionTag))
        {
            PlayerInArea = true;
            Player = collision.gameObject.transform;
        }
    }

    //Checks if any collisions exiting the area detector is the player
    //If the collision exiting is the player, the check for the player is false and the player location is null
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(detectionTag))
        {
            PlayerInArea = false;
            Player = null;
        }
    }
}
