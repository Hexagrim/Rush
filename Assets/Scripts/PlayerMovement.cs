using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float movespeed;
    public float maxSpeed;
    public float jumpSpeed;

    public KeyCode Left, Right, Up;

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




    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
    }



    void Update()
    {
        HandleCoyoteTime();
        HandleJumpBuffer();
        HandleDoubleJump();
        HandleJump();
        HandleGravity();
        HandleDamping();
        HandleJumpCut();
        HandleApexSpeedClamp();
    }


    // yes i had to do this :<

    private void FixedUpdate()
    {
        HandleMovement();
        HandleGroundCheck();
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
        if (Mathf.Abs(rb.linearVelocityY) <= 0.5f && !isGrounded)
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
        if (Mathf.Abs(rb.linearVelocityY) <= 0.5f && !isGrounded)
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








}
