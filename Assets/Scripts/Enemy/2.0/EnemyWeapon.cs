using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
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
