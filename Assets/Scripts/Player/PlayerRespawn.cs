using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Manual Set References")]
    [SerializeField] private AudioClip checkpointSound;
    [SerializeField] Transform startingPoint;
    [SerializeField] Transform bossRoomStart;

    [Header("References")]
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
            SoundManager.instance.PlaySound(checkpointSound);   
        }

        //Takes the player back to last checkpoint
        if (collision.transform.tag == "FallZone")
        {
            if (playerHealth.currentHealth > 0)
            {
                transform.position = currentCheckpoint.position;
            }
            playerHealth.TakeDamage(1);
        }

        if (collision.transform.tag == "FinishPoint")
        {
            uiManager.LevelCompleted();
        }

        if (collision.transform.tag == "BossRoomTeleport")
        {
            transform.position = bossRoomStart.position;
        }
    }
}
