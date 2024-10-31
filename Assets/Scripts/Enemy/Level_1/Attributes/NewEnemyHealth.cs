using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemyHealth : MonoBehaviour
{
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

    [Header("Components")]
    [SerializeField] private BoxCollider2D swordCollider;

    private Animator anim;
    private GameObject enemy;

    private void Awake()
    {
        currentHealth = startingHealth;
        spriteRend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        enemy = gameObject;
    }

    //Takes health away from enemy health pool and stuns the enemy from attacking or moving
    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            swordCollider.enabled = false;
            anim.SetTrigger("hit");
            StartCoroutine(Stun());
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("dead");
                dead = true;
                Destroy(gameObject, 0.40f); //Despawns enemy
            }
        }

    }

    //Function that stuns the enemy, activates certain animations
    private IEnumerator Stun()
    {
        anim.SetBool("idle", true);
        for (int i = 0; i < noOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(stunDuration / (noOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(stunDuration / (noOfFlashes * 2));
        }
    }
}
