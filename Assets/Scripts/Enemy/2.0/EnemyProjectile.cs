using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Damage
{
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
        if(Mathf.Sign(localScaleX) != _direction)
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
