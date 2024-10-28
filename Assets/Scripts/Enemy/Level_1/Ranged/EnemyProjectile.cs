using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Damage
{
    /// <summary>
    /// Script for the enemy projectiles. Functions for activating the projectile when in use and move the projectile across the screen. 
    /// Checks if the projectile has collider with anything and deactivates if true.
    /// </summary>

    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private float lifetime;
    private float direction;

    public void ActivateProjectile(float _direction)
    {
        direction = _direction;
        lifetime = 0;
        gameObject.SetActive(true);
        float localScaleX = transform.localScale.x;

        //If the arrow is not facing the same direction as the parent gameobject, switch the direction
        if(Mathf.Sign(localScaleX) != Mathf.Sign(_direction))
        {
            localScaleX = -localScaleX;
        }
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Update()
    {
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
        {
            gameObject.SetActive(false);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision); //Execute logic from parent script first
        if (GetComponent<Collider2D>().gameObject.layer == 9)
        {
            gameObject.SetActive(false); //When this hits any object, deactivate arrow
        }
        
    }
}
