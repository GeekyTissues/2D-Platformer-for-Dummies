using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NewMeleeEnemy : MonoBehaviour
{
    /// <summary>
    /// Controls the melee enemies. Checks if the player is nearby, moves toward the player, and attacks when in range. 
    /// </summary>

    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;

    [Header("Movement Parameters")]
    private Vector3 initScale;
    [SerializeField] private int moveSpeed;
    [SerializeField] private float inRange;

    [Header("Collider Parameters")]
    [SerializeField] private CapsuleCollider2D cc;
    [SerializeField] private BoxCollider2D swordCollider;
    [SerializeField] private BoxCollider2D areaCollider;


    private float cooldownTimer = Mathf.Infinity;

    //References
    private PlayerHealth playerHealth;


    //Components
    private Animator anim;

    //Primitives
    private bool playerDetected;
    private Transform player;
    private float direction;
    private bool facingRight = true;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("idle", true);
        swordCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;
        playerDetected = GetComponentInChildren<PlayerDetection>().PlayerInArea;

        //If statements checking for when the player has been detected
        if (playerDetected)
        {
            anim.SetBool("idle", false);
            player = GetComponentInChildren<PlayerDetection>().Player;

            //If player is out of melee range, moves toward the player
            if( Vector2.Distance(transform.position, player.position) >= inRange)
            {
                MoveTowardsPlayer();
            }
            //If the player is in range and the attackcooldown is over, attacks the player
            else
            {
                StopMovement();
                if(cooldownTimer > attackCooldown)
                {
                    swordCollider.enabled = true;
                    anim.SetTrigger("attack");
                    cooldownTimer = 0;
                }
            }
        }
        //When the player is no longer in range, stops all movement
        if (!playerDetected) //Stops Enemy movement
        {
            player = null;
            StopMovement();
        }
    }

    /// <summary>
    /// Region contains functions for movement i.e. moving towards the player, stopping movement
    /// </summary>
    #region Movement
    private void MoveTowardsPlayer()
    {
        anim.SetBool("walk", true);
        transform.position = Vector2.MoveTowards(transform.position, 
            new Vector2(player.transform.position.x, transform.position.y), moveSpeed * Time.deltaTime); // Enemy moves toward player in the x axis
        if(player.transform.position.x < transform.position.x && facingRight) //Changes which direction it faces when moving
        {
            Flip();
        }
        if (player.transform.position.x > transform.position.x && !facingRight)
        {
            Flip();
        }
    }

    private void StopMovement()
    {
        anim.SetBool("walk", false);
        anim.SetBool("idle", true);
    }
    private void Flip()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;

        facingRight = !facingRight;
    }
    #endregion
}
