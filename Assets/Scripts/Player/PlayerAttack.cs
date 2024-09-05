using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;

    [Header("Collider Parameters")]
    [SerializeField] private int damage;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask enemyLayer;

    private float attackTimer = Mathf.Infinity;

    private Animator anim;
    private EnemyHealth enemyHealth;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    private void Update()
    {
        attackTimer += Time.deltaTime;

        //Attack only when attack cooldown has ended and player has press correct input
        if ((Input.GetMouseButton(0) || Input.GetKey(KeyCode.Joystick1Button0)) 
            && attackTimer > attackCooldown)
        {
            attackTimer = 0;
            anim.SetTrigger("attack");
        }
    }

    public bool AttackConnected()
    {
        //Changes how big the hitbox is for detection
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, enemyLayer);


        if (hit.collider != null)
        {
            enemyHealth = hit.transform.GetComponent<EnemyHealth>();
        }

        return hit.collider != null;
    }

    public void Damage()
    {
        if (AttackConnected())
        {
            enemyHealth.TakeDamage(damage);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

}
