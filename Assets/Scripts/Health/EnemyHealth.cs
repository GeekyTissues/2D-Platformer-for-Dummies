using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    /// <summary>
    /// Controls the enemy health attribute. Contains a take damage and stun damage function
    /// </summary>
    
    [Header("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private bool dead;

    [Header("Stun")]
    [SerializeField] private float stunDuration;
    [SerializeField] private int noOfFlashes;
    private SpriteRenderer spriteRend;

    [SerializeField] private int coinDrop;

    [Header("SFX")]
    [SerializeField] private AudioClip takeDamageSound;

    private Animator anim;

    [Header("Components")]
    [SerializeField] private PlayerCurrency playerCurrency;
    [SerializeField] private MeleeEnemy meleeEnemy;
    [SerializeField] private EnemyPatrol enemyPatrol;

    private void Awake()
    {
        currentHealth = startingHealth;
        spriteRend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            StartCoroutine(Stun());
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("dead");
                dead = true;
                meleeEnemy.enabled = false;
                enemyPatrol.enabled = false;
                Destroy(gameObject, 0.40f);
                playerCurrency.GainCurrency(coinDrop);
                
            }
        }
        
    }

    private IEnumerator Stun()
    {
        GetComponent<EnemyPatrol>().enabled = false;
        for (int i = 0; i < noOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(stunDuration / (noOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(stunDuration / (noOfFlashes * 2));
        }
        GetComponent<EnemyPatrol>().enabled = true;
    }

    //public void AddHealth(float _value)
    //{
    //    currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    //}

    #region SFX
    private void TakeDamageSound()
    {
        SoundManager.instance.PlaySound(takeDamageSound);
    }
    #endregion
}

