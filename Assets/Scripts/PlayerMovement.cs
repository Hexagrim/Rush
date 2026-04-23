using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float movespeed;
    public float maxSpeed;
    public float jumpSpeed;

    public KeyCode Left, Right, Up, dashButton;

    public float apexGravity, upGravity, downGravity;

    bool isGrounded;

    public Transform groundCheck;
    public float groundCheckSize;
    public LayerMask groundLayer;

    public float apexSpeed;

    public float coyoteTimeCounter;
    private float coyoteTime = 0.15f;

    private float jumpBufferTime = 0.15f;
    public float jumpBufferCounter;

    private Animator Anim;

    public bool doubleJumpEnabled;
    public bool canDoubleJump;

    public GameObject doubleJumpParticle;


    public Transform wallCheck;
    bool isWalled;
    bool isSliding;
    public float wallSlidingSpeed;
    public LayerMask wallLayer;

    bool wallJumping;

    public Vector2 wallJumpForce;


    public float dashSpeed;
    public float dashTime;
    bool isDashing;
    bool canDash = true;
    bool dashReset;
    public float dashCooldown;

    public GameObject DashParticle;
    public ParticleSystem dashtrail;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
    }



    void Update()
    {
        if (isDashing) return;
        HandleCoyoteTime();
        HandleJumpBuffer();
        if (!isSliding)
        {
            HandleDoubleJump();
        }
        HandleJump();
        HandleGravity();
        HandleDamping();
        HandleJumpCut();
        if (!isSliding)
        {
            HandleApexSpeedClamp();
        }
        HandleWallCheck();
        WallSlide();
        WallJump();
        HandleDash();
    }


    // yes i had to do this :<

    private void FixedUpdate()
    {
        if (isDashing) return;
        HandleGroundCheck();

        if(wallJumping)
        {
            rb.AddForce(new Vector2(wallJumpForce.x * transform.localScale.x, wallJumpForce.y));
            //rb.linearVelocity = new Vector2(wallJumpForce.x * transform.localScale.x, wallJumpForce.y);
        }
        else
        {
            HandleMovement();
        }
    }



    void HandleCoyoteTime()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }




    void HandleJumpBuffer()
    {
        if (Input.GetKeyDown(Up))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }




    void HandleDoubleJump()
    {
        if (!isGrounded && canDoubleJump && Input.GetKeyDown(Up) && doubleJumpEnabled)
        {
            rb.linearVelocityY = jumpSpeed;
            Anim.SetTrigger("takeoff");
            if (FindFirstObjectByType<CamShake>() != null)
            {
                FindFirstObjectByType<CamShake>().ShakeCam(4, 2, 0.07f);
            }
            jumpBufferCounter = 0f;

            canDoubleJump = false;

            Instantiate(doubleJumpParticle, groundCheck.position, Quaternion.identity);
        }

    }



    void HandleJump()
    {
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            rb.linearVelocityY = jumpSpeed;
            Anim.SetTrigger("takeoff");
            canDoubleJump = true;
            jumpBufferCounter = 0f;
        }
        
    }



    void HandleGravity()
    {
        if (Mathf.Abs(rb.linearVelocityY) <= 1f && !isGrounded)
        {
            rb.gravityScale = apexGravity;
        }
        else if (rb.linearVelocityY < 0f)
        {
            rb.gravityScale = downGravity;
        }
        else
        {
            rb.gravityScale = upGravity;
        }
    }




    void HandleDamping()
    {
        if (isGrounded)
        {
            rb.linearDamping = 2f;
        }
        else
        {
            rb.linearDamping = 0f;
        }
    }




    void HandleJumpCut()
    {
        if (Input.GetKeyUp(Up) && rb.linearVelocityY > 0f)
        {
            rb.linearVelocityY *= 0.6f;
            coyoteTimeCounter = 0f;
        }
    }




    void HandleApexSpeedClamp()
    {
        if (Mathf.Abs(rb.linearVelocityY) <= 0.7f && !isGrounded)
        {
            rb.linearVelocityX = Mathf.Clamp(rb.linearVelocityX, -apexSpeed, +apexSpeed);
        }
        else
        {
            rb.linearVelocityX = Mathf.Clamp(rb.linearVelocityX, -maxSpeed, maxSpeed);
        }
    }




    void HandleMovement()
    {
        if (Input.GetKey(Left))
        {
            if (rb.linearVelocityX > 0f)
            {
                rb.linearVelocityX = 0f;
            }
            else rb.AddForceX(-movespeed);
            Anim.SetBool("isRunning", true);
            transform.localScale = new Vector2(-1, 1);
        }
        else if (Input.GetKey(Right))
        {
            if (rb.linearVelocityX < 0f)
            {
                rb.linearVelocityX = 0f;
            }
            else rb.AddForceX(movespeed);
            Anim.SetBool("isRunning", true);
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            rb.linearVelocityX *= 0.5f;
            Anim.SetBool("isRunning", false);
        }

    }


    void HandleGroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckSize, groundLayer);
        Anim.SetBool("isJumping", !isGrounded);
    }
    void HandleWallCheck()
    {
        isWalled = Physics2D.OverlapCircle(wallCheck.position, 0.1f, wallLayer);
    }
    //

    void WallSlide()
    {
        if(isWalled && !isGrounded && (Input.GetKey (Left) || Input.GetKey(Right)))
        {
            isSliding = true;
        }
        else
        {
            isSliding = false;
        }

        if(isSliding)
        {
            rb.linearVelocityY = Mathf.Clamp(rb.linearVelocityY,-wallSlidingSpeed,wallSlidingSpeed);
        }
        Anim.SetBool("isWallSliding", isSliding);
    }


    void WallJump()
    {
        if (!isGrounded && isSliding && Input.GetKeyDown(Up))
        {
            wallJumping = true;

            Invoke(nameof(StopWallJump), 0.15f);
        }
        
    }
    //yes yes very much, good code i write, yeah!. if you read this, get a life !

    void StopWallJump()
    {
        wallJumping = false;
        rb.linearVelocityY *= 0.4f;
        rb.gravityScale = downGravity;
        canDoubleJump = true;

    }

    void HandleDash()
    {
        if (Input.GetKeyDown(dashButton) && dashReset && canDash)
        {
            StartCoroutine(Dash());
        }

        if (isGrounded) //|| isSliding)
        {
            dashReset = true;

        }
        Anim.SetBool("isDashing", isDashing);

    }
    IEnumerator Dash()//yes this is dash
    {
        isDashing = true;
        dashReset = false;
        canDash = false;
        float normGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocityX = dashSpeed * Mathf.Sign(transform.localScale.x);
        rb.linearVelocityY  = 0f;
        dashtrail.Play(true);
        if(FindFirstObjectByType<CamShake>() != null)
        {
            FindFirstObjectByType<CamShake>().ShakeCam(7, 4, 0.2f);
        }
        
        Instantiate(DashParticle, transform.position , Quaternion.identity );
        yield return new WaitForSeconds(dashTime);
        dashtrail.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        isDashing = false;
        rb.gravityScale = normGravity;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;

    }


}
