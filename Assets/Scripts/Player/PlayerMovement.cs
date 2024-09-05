using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private LayerMask playerLayer;

    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private Animator anim;

    private float horizontalInput;

    [Header("SFX")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip landSound;
    [SerializeField] private AudioClip runningSound;

    private void Awake()
    {
        //Grab references
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        //Moving left and right
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        if (horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(5,5,5);
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-5, 5, 5);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            Jump();
            anim.Play("jump");
            if (Input.GetKeyDown(KeyCode.Space) && (IsGrounded()||IsPlatformed()))
            {
                SoundManager.instance.PlaySound(jumpSound);
            }
        }
        
        if (Input.GetKey(KeyCode.Joystick1Button1))
        {
            Jump();
            anim.Play("jump");
            if (Input.GetKeyDown(KeyCode.Joystick1Button1) && (IsGrounded() || IsPlatformed()))
            {
                SoundManager.instance.PlaySound(jumpSound);
            }
        }

        //Set animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", IsGrounded());
        anim.SetBool("platformed", IsPlatformed());
        
    }

    private void Jump()
    {
        if (IsGrounded() || IsPlatformed())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
        }
    }

    
    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.size, 0, Vector2.down, 1f, groundLayer);
        return raycastHit.collider != null;
        
    }

    private bool IsPlatformed()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.size, 0, Vector2.down, 1f, platformLayer);
        return raycastHit.collider != null;
    }

    #region SFX
    private void RunningSound()
    {
        SoundManager.instance.PlaySound(runningSound);
    }
    #endregion
}
