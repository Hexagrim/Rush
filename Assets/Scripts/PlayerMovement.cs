using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float movespeed;
    public float maxSpeed;
    public float jumpSpeed;

    public KeyCode Left, Right, Up, dashButton, Down, glide;

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
    public ParticleSystem dashtrail, slamtrail, glideTrail;

    bool isSlamming;
    bool canSlam = true;
    public GameObject slamParticle;
    public float slamSpeed;
    public float slamCooldown;


    bool isGliding = false;
    public bool canGlide = true;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
    }



    void Update()
    {
        HandleGroundCheck();

        if (isDashing || isSlamming) return;
        HandleCoyoteTime();
        HandleJumpBuffer();
        if (!isSliding)
        {
            HandleDoubleJump();
        }
        HandleJump();
        if (!isGliding)
        {
            HandleGravity();
        }
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
        HandleSlam();
        HandleGlide();
    }


    // yes i had to do this :<

    private void FixedUpdate()
    {

        if (isDashing || isSlamming) return;


        if (wallJumping)
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
        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckSize, groundLayer);
        isGrounded = Physics2D.OverlapBox(groundCheck.position, Vector2.one * groundCheckSize, 0f, groundLayer);
        Anim.SetBool("isJumping", !isGrounded);
    }
    void HandleWallCheck()
    {
        isWalled = Physics2D.OverlapCircle(wallCheck.position, 0.1f, wallLayer);
    }
    //

    void WallSlide()
    {
        if (isWalled && !isGrounded && (Input.GetKey(Left) || Input.GetKey(Right)) && rb.linearVelocityY <= 5f)
        {
            isSliding = true;
        }
        else
        {
            isSliding = false;
        }

        if (isSliding)
        {
            rb.linearVelocityY = Mathf.Clamp(rb.linearVelocityY, -wallSlidingSpeed, wallSlidingSpeed);
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
        //canDoubleJump = true;
        //dashReset = true;

    }

    void HandleDash()
    {
        if (Input.GetKeyDown(dashButton) && dashReset && canDash) // yeah eyah biqach i dont fucking lke to
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
        rb.linearVelocityY = 0f;
        dashtrail.Play(true);
        if (FindFirstObjectByType<CamShake>() != null)
        {
            FindFirstObjectByType<CamShake>().ShakeCam(7, 4, 0.2f);
        }

        Instantiate(DashParticle, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(dashTime);
        dashtrail.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        isDashing = false;
        rb.gravityScale = normGravity;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;

    }

    void HandleSlam()
    {

        if (Input.GetKeyDown(Down) && canSlam && !isGrounded)
        {

            StartCoroutine(DownSmash());
        }
    }

    IEnumerator DownSmash()
    {
        isSlamming = true;
        canSlam = false;

        float slamStartTime = Time.time;
        float minLockTime = 0.1f;
        bool cancelUsed = false;
        rb.linearVelocity = new Vector2(0, -slamSpeed);
        Anim.SetTrigger("groundSlam");
        FindFirstObjectByType<CamShake>().ShakeCam(2, 2, 0.5f);
        Instantiate(DashParticle, transform.position, Quaternion.identity);
        slamtrail.Play();
        while (!isGrounded && isSlamming)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            if (!cancelUsed && Time.time > slamStartTime + minLockTime)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    cancelUsed = true;
                    Anim.SetTrigger("stopSmash");
                    rb.linearVelocity = new Vector2(
                        rb.linearVelocity.x * 0.5f,
                        rb.linearVelocity.y * 0.25f
                    );
                    rb.linearVelocity += Vector2.up * 1.5f;
                    break;
                }
            }
            yield return null;
        }
        if (isGrounded)
        {
            FindFirstObjectByType<CamShake>().ShakeCam(10, 5, 0.15f);
            Instantiate(slamParticle, transform.position, Quaternion.identity);
        }
        slamtrail.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        isSlamming = false;
        yield return new WaitForSeconds(slamCooldown);
        canSlam = true;
    }


    void HandleGlide()
    {
        if (Input.GetKey(glide) && canGlide && rb.linearVelocityY <= 0f && !isGrounded)
        {
            isGliding = true;
            rb.gravityScale = 1f;
            //rb.linearVelocityY = Mathf.Clamp(rb.linearVelocityY, -10, float.MaxValue);
            if (!glideTrail.isPlaying)
            {
                glideTrail.Play();
            }
        }
        else
        {
            if (glideTrail.isPlaying)
            {
                glideTrail.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
            isGliding = false;
        }

        Anim.SetBool("isGliding", isGliding);
    }

    //DEBUG SHIT AHH

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.green;

        Vector3 pos = groundCheck.position;
        Vector3 size = new Vector3(groundCheckSize, groundCheckSize, 0f);

        Gizmos.DrawWireCube(pos, size);
    }
}
