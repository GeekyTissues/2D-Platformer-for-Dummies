using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerMovement playerMovement;

    [SerializeField] private float attackCooldown;

    private float attackTimer;
    
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        
    }


    public bool canAttack()
    {
        return true;
    }

}
