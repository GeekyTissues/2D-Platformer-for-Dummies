using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables and References
    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float airSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float jumpCooldown;

    [Header("Calculations")]
    [SerializeField] private float slopeCheckDistance;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playerLayer;

    [Header("Materials")]
    [SerializeField] PhysicsMaterial2D noFriction;
    [SerializeField] PhysicsMaterial2D fullFriction;

    
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
    private float jumpTimer;
    private float slopeSideAngle;
    private bool isJumping = false;

    //Vectors
    private Vector2 colliderSize;
    private Vector2 slopeNormalPerp;
    private Vector2 newVelocity;

    #endregion

    private void Awake()
    {
        //Grab references
        body = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();

        colliderSize = cc.size;
        jumpTimer = Mathf.Infinity;
    }

    private void Update()
    {
        jumpTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            Jump();
        }

        //Set animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", IsGrounded());
    }

    private void FixedUpdate()
    {
        SlopeCheck();
        ApplyMovement();
    }

    #region Movement
    private void ApplyMovement()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        //Moving left and right
        newVelocity.Set(horizontalInput * speed, body.velocity.y);
        body.velocity = newVelocity;

        //Facing which direction
        if (horizontalInput > 0 && !facingRight) { 
            Flip();
        }
        if (horizontalInput < 0 && facingRight)
        {
            Flip();
        }

        if ((IsGrounded() && !isOnSlope && !isJumping)) 
        {
            newVelocity.Set(speed * horizontalInput, 0.0f);
            body.velocity = newVelocity;
        }
        else if ((IsGrounded() && isOnSlope && !isJumping))
        {
            newVelocity.Set(speed * slopeNormalPerp.x * -horizontalInput, speed * slopeNormalPerp.y * -horizontalInput);
            body.velocity = newVelocity;    
        }
        else if (!IsGrounded())
        {
            newVelocity.Set(speed * horizontalInput, body.velocity.y);
            body.velocity = newVelocity;
        }

       
    }


    private bool CanJump()
    {
        if (IsGrounded())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Jump()
    {
        if (CanJump())
        {
            isJumping = true;
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.Play("jump");
            SoundManager.instance.PlaySound(jumpSound);
        }
    }


    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(cc.bounds.center, cc.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;

    }

    private void Flip()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;

        facingRight = !facingRight;
    }

    #endregion


    #region Slopes
    private void SlopeCheck()
    {
        Vector2 checkPos = transform.position - new Vector3(0.0f, colliderSize.y / 2);
        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, groundLayer);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, groundLayer);

        if (slopeHitFront)
        {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
        }
        else if (slopeHitBack)
        { 
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
        }
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

        if (isOnSlope && horizontalInput == 0.0f)
        {
            body.sharedMaterial = fullFriction;
        }
        else
        {
            body.sharedMaterial = noFriction;
        }
            
    }

    #endregion

    #region SFX
    private void RunningSound()
    {
        SoundManager.instance.PlaySound(runningSound);
    }
    #endregion
}
