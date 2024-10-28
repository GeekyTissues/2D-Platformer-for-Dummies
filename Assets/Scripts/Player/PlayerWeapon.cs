using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    /// <summary>
    /// Controls the player's weapon.
    /// </summary>

    [SerializeField] private int damage;
    [SerializeField] private BoxCollider2D swordCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().gameObject.layer == 8)
        {
            collision.GetComponent<NewEnemyHealth>().TakeDamage(damage);
            swordCollider.enabled = false;
        }
    }
}
