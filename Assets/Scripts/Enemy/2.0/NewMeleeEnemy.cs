using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NewMeleeEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;

    [Header("Movement Parameters")]
    private Vector3 initScale;
    [SerializeField] private int moveSpeed;
    [SerializeField] private int maxDist;
    [SerializeField] private int minDist;


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
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer = Time.deltaTime;
        playerDetected = GetComponentInChildren<PlayerDetection>().PlayerInArea;
        if(playerDetected)
        {
            anim.SetBool("idle", false);
            player = GetComponentInChildren<PlayerDetection>().Player;
            MoveTowardsPlayer();
            if (PlayerInRange())
            {
                StopMovement();
            }
        }
        if (!playerDetected) //Stops Enemy movement
        {
            player = null;
            StopMovement();
        }
    }

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

    private bool PlayerInRange()
    {
        if (Vector3.Distance(transform.position, player.position) <= maxDist)
        {
            return true;
        }
        return false;
    }
}
