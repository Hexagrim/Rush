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

    private float coyoteTimeCounter;
    private float coyoteTime = 0.15f;

    private float jumpBufferTime = 0.15f;
    private float jumpBufferCounter;

    private Animator Anim;

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(Up))
        {

            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }


        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            rb.linearVelocityY = jumpSpeed;
            Anim.SetTrigger("takeoff");
            jumpBufferCounter = 0f;
        }
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

        isGrounded = Physics2D.OverlapBox(groundCheck.position, Vector2.one * groundCheckSize, 0f, groundLayer);

        if(isGrounded)
        {
            rb.linearDamping = 2f;
        }
        else//
        {
            rb.linearDamping = 0f;
        }

        if(Input.GetKeyUp(Up) && rb.linearVelocityY > 0f)
        {
            rb.linearVelocityY *= 0.75f;
            coyoteTimeCounter = 0f;
        }

        if(Mathf.Abs(rb.linearVelocityY) <= 0.5f && !isGrounded)
        {
            rb.linearVelocityX = Mathf.Clamp(rb.linearVelocityX, -apexSpeed, +apexSpeed);
        }
        else
        {
            rb.linearVelocityX = Mathf.Clamp(rb.linearVelocityX, -maxSpeed, maxSpeed);

        }
    }
    private void FixedUpdate()
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


        Anim.SetBool("isJumping", !isGrounded);


    }
}
