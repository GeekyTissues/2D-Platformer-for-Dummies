using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    /// <summary>
    /// Script for the enemy weapon that checks if the weapon has collided with the player.
    /// Takes health from the player on collision
    /// </summary>

    [SerializeField] private int damage;
    [SerializeField] private BoxCollider2D swordCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().gameObject.layer == 7)
        {
            collision.GetComponent<PlayerHealth>().TakeDamage(damage);
            swordCollider.enabled = false;
        }
    }
}
