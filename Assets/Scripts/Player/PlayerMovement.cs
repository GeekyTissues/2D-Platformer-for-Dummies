using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float airSpeed;
    [SerializeField] private float jumpPower;

    [Header("Calculations")]
    [SerializeField] private float slopeCheckDistance;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private LayerMask playerLayer;

    
    [Header("SFX")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip landSound;
    [SerializeField] private AudioClip runningSound;

    //Components
    private Rigidbody2D body;
    private CapsuleCollider2D cc;
    private Animator anim;
    
    //Primitives
    private float horizontalInput;
    private float slopeDownAngle;
    private bool isOnSlope;
    private float slopeDownAngleOld;
    private bool facingRight = true;

    //Vectors
    private Vector2 colliderSize;
    private Vector2 slopeNormalPerp;


    private void Awake()
    {
        //Grab references
        body = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();

        colliderSize = cc.size;
    }

    private void Update()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            Jump();
            anim.Play("jump");
            if (Input.GetKeyDown(KeyCode.Space) && (IsGrounded() || IsPlatformed()))
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

    private void FixedUpdate()
    {
        SlopeCheck();
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        //Moving left and right
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        if (horizontalInput > 0 && !facingRight) { 
            Flip();
        }
        if (horizontalInput <0 && facingRight)
        {
            Flip();
        }

        if ((IsGrounded() && !isOnSlope) || (IsPlatformed() && !isOnSlope)) { 

        }
        else if ((IsGrounded() && isOnSlope) || (IsPlatformed() && isOnSlope))
        {

        }
        else if (!IsGrounded())
        {
            
        }
    }

    private void Flip()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;

        facingRight = !facingRight;
    }

    private void SlopeCheck()
    {
        Vector2 checkPos = transform.position - new Vector3(0.0f, colliderSize.y / 2);

        SlopeCheckVertical(checkPos);
    }

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {

    }

    private void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, groundLayer);
        //RaycastHit2D hit = Physics2D.BoxCast(checkPos, colliderSize, 0, Vector2.down, slopeCheckDistance, groundLayer);
        if (hit)
        {
            slopeNormalPerp = Vector2.Perpendicular(hit.normal);

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != slopeDownAngleOld)
            {
                isOnSlope = true;
            }

            slopeDownAngleOld = slopeDownAngle;

            Debug.DrawRay(hit.point, slopeNormalPerp, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
        }
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
        RaycastHit2D raycastHit = Physics2D.BoxCast(cc.bounds.center, cc.size, 0, Vector2.down, 1f, groundLayer);
        return raycastHit.collider != null;
        
    }

    private bool IsPlatformed()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(cc.bounds.center, cc.size, 0, Vector2.down, 1f, platformLayer);
        return raycastHit.collider != null;
    }

    #region SFX
    private void RunningSound()
    {
        SoundManager.instance.PlaySound(runningSound);
    }
    #endregion
}
