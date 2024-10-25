using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    /// <summary>
    /// Script for the health collectible. If the collision detected is the player, give the player health back. 
    /// Deactivates the collectible when collected 
    /// </summary>

    [SerializeField] private float healthValue;
    [SerializeField] private AudioClip pickUpSound;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SoundManager.instance.PlaySound(pickUpSound);
            collision.GetComponent<PlayerHealth>().AddHealth(healthValue);
            gameObject.SetActive(false);
        }
    }
}
