using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private bool dead;

    [Header("Stun")]
    [SerializeField] private float stunDuration;
    [SerializeField] private int noOfFlashes;
    private SpriteRenderer spriteRend;

    [Header("SFX")]
    [SerializeField] private AudioClip takeDamageSound;

    private Animator anim;

    private float lifetime = 3;

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
                Destroy(gameObject, 3f);
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

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    #region SFX
    private void TakeDamageSound()
    {
        SoundManager.instance.PlaySound(takeDamageSound);
    }
    #endregion
}

