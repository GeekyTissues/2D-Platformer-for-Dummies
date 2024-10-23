using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
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

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        anim.SetTrigger("attack");
    }

    private void RangedAttack()
    {
        cooldownTimer += Time.deltaTime;
        arrows[FindArrow()].transform.position = firePoint.position;
        arrows[FindArrow()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

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
