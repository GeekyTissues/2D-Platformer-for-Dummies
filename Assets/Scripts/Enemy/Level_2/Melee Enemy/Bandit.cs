using UnityEngine;
using System.Collections;

public class Bandit : MonoBehaviour
{

    [SerializeField] float m_speed = 2.0f;  // Speed for patrolling
    [SerializeField] float m_jumpForce = 7.5f;
    [SerializeField] Transform pointA;  // Left patrol point
    [SerializeField] Transform pointB;  // Right patrol point
    [SerializeField] int m_health = 100;  // Health of the bandit
    [SerializeField] int m_damage = 10;   // Damage bandit can inflict
    [SerializeField] float m_detectionRange = 5.0f;  // Detection range for player
    [SerializeField] LayerMask playerLayer;     // Layer for detecting the player

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_Bandit m_groundSensor;
    private bool m_grounded = false;
    private bool m_combatIdle = false;
    private bool m_isDead = false;
    private bool movingRight = true;  // For patrol direction
    private Transform player;  // Reference to the player object
    private bool isAttacking = false;  // Whether the bandit is attacking

    // Original localScale of the Bandit sprite for flipping
    private Vector3 originalScale;

    // Use this for initialization
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();

        // Store the original scale at the start to preserve the correct size when flipping
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isDead) return;  // Stop updating if dead

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // Handle patrol or attack logic
        if (PlayerDetected())
        {
            AttackPlayer();
        }
        else
        {
            Patrol();
        }

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);

        // -- Handle Animations --
        //Run animation when patrolling or moving towards player
        if (Mathf.Abs(m_body2d.velocity.x) > Mathf.Epsilon)
            m_animator.SetInteger("AnimState", 2);
        else
            m_animator.SetInteger("AnimState", 0);
    }

    // Patrol between two points
    void Patrol()
    {
        if (isAttacking) return; // If attacking, stop patrol

        // Move right
        if (movingRight)
        {
            m_body2d.velocity = new Vector2(m_speed, m_body2d.velocity.y);

            // Flip sprite to face right direction
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);

            // If reached point B, switch direction
            if (transform.position.x >= pointB.position.x)
            {
                movingRight = false;
            }
        }
        // Move left
        else
        {
            m_body2d.velocity = new Vector2(-m_speed, m_body2d.velocity.y);

            // Flip sprite to face left direction
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);

            // If reached point A, switch direction
            if (transform.position.x <= pointA.position.x)
            {
                movingRight = true;
            }
        }
    }

    // Detect if the player is within the detection range
    bool PlayerDetected()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, m_detectionRange, playerLayer);

        if (playerCollider != null)
        {
            player = playerCollider.transform;
            return true;
        }

        player = null;
        return false;
    }

    // Attack the player
    void AttackPlayer()
    {
        if (player == null) return;  // No player to attack

        isAttacking = true;

        // Face the player
        if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);  // Face right
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);   // Face left
        }

        // Move towards the player if not too close
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer > 1.0f)
        {
            // Move towards the player
            Vector2 direction = (player.position - transform.position).normalized;
            m_body2d.velocity = new Vector2(direction.x * m_speed, m_body2d.velocity.y);
        }
        else
        {
            // If close enough, trigger attack
            m_animator.SetTrigger("Attack");
            // You can add logic here to deal damage to the player
        }
    }

    // Method to handle taking damage
    public void TakeDamage(int damageAmount)
    {
        if (m_isDead)
            return;

        m_health -= damageAmount;

        if (m_health <= 0)
        {
            m_health = 0;
            m_isDead = true;
            m_animator.SetTrigger("Death");
        }
        else
        {
            m_animator.SetTrigger("Hurt");
        }
    }

    // For visualizing the detection range in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_detectionRange);
    }
}
