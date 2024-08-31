using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound;
    [SerializeField] Transform startingPoint;
    private Transform currentCheckpoint;
    private PlayerHealth playerHealth;
    private UIManager uiManager;

    private void Awake()
    {
        transform.position = startingPoint.position;
        currentCheckpoint = startingPoint;
        playerHealth = GetComponent<PlayerHealth>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public void CheckRespawn()
    {
        transform.position = currentCheckpoint.position;
        playerHealth.Respawn();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Checkpoint")
        {
            currentCheckpoint = collision.transform; //Stores Checkpoint
            collision.GetComponent<Collider2D>().enabled = false;
            collision.GetComponent<Animator>().SetTrigger("Appear");
        }

        //Takes the player back to last checkpoint
        if (collision.transform.tag == "FallZone")
        {
            playerHealth.TakeDamage(1);
            if (playerHealth.currentHealth > 0)
            {
                transform.position = currentCheckpoint.position;
            }
        }
    }
}
