using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    /// <summary>
    /// Ranged enemy script. Checks for the player, if the player is detected, the enemy shoots projectiles in the direction of the player.
    /// </summary>

    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;

    [Header("Ranged Attack")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] arrows;
    
    [Header("Collider Parameters")]
    [SerializeField] private BoxCollider2D areaDetector;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    

    //Components
    private Animator anim;
    private PlayerHealth playerHealth;

    //primitives
    private float cooldownTimer = Mathf.Infinity;
    private bool playerDetected;
    private Transform player;
    private bool facingRight = true;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>(); 
        areaDetector = GetComponent<BoxCollider2D>();
        anim.SetBool("idle", true);
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        playerDetected = GetComponentInChildren<PlayerDetection>().PlayerInArea;
        if(playerDetected)
        {
            anim.SetBool("idle", false);
            player = GetComponentInChildren<PlayerDetection>().Player;
            if(player.transform.position.x < transform.position.x && facingRight)
            {
                Flip();
            }
            if (player.transform.position.x > transform.position.x && !facingRight)
            {
                Flip();
            }
            if (cooldownTimer > attackCooldown)
            {
                anim.SetTrigger("attack");
                cooldownTimer = 0;
                
            }
            anim.SetBool("idle", true);
        }
    }

    //Flips the enemy to face the player
    private void Flip()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;

        facingRight = !facingRight;
    }

    //Calls for the projectile to activate and fire in the direction being faced
    private void RangedAttack()
    {
        arrows[FindArrow()].transform.position = firePoint.position;
        arrows[FindArrow()].GetComponent<EnemyProjectile>()
            .ActivateProjectile(Mathf.Sign(transform.localScale.x));
    }

    //Checks for any arrows currently available
    private int FindArrow()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            if (!arrows[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }

}
