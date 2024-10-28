using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    /// <summary>
    /// Controls the player attack. 
    /// </summary>

    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;

    [Header("Collider Parameters")]
    [SerializeField] private int damage;
    [SerializeField] private BoxCollider2D swordCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask enemyLayer;

    //Primitives
    private float attackTimer = Mathf.Infinity;

    //Components
    private Animator anim;
    private EnemyHealth enemyHealth;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        swordCollider.enabled = false;
    }

    private void Update()
    {
        attackTimer += Time.deltaTime;

        //Attack only when attack cooldown has ended and player has press correct input
        if ((Input.GetMouseButton(0) || Input.GetKey(KeyCode.Joystick1Button0)) 
            && attackTimer > attackCooldown)
        {
            swordCollider.enabled = true;
            attackTimer = 0;
            anim.SetTrigger("attack");
        }
    }
}
