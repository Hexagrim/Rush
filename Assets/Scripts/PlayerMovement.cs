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
    bool isWalled;
    public Transform wallCheck;
    public LayerMask wallLayer;
    public float wallCheckSize;
    public float wallSlidingSpeed;
    bool isWallSliding;




    public bool isWallJumping;
    public float wallJumpingDirection;
    float wallJumpingTime = 0.2f;
    public float wallJumpingCounter;
    float wallJumpingDuration = 0.4f;
    public Vector2 wallJumpPower;
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



        //DOUBLE JUMP________________________________________________
        //_____________________________________________________________
        //_______________________________________:>__________________


        if(!isGrounded && canDoubleJump && Input.GetKeyDown(Up) && doubleJumpEnabled && !isWallSliding && !isWallJumping)
        {
            rb.linearVelocityY = jumpSpeed;
            Anim.SetTrigger("takeoff");
            jumpBufferCounter = 0f;
            canDoubleJump = false;
            Instantiate(doubleJumpParticle,groundCheck.position,Quaternion.identity);
        }


        //yeah idk, im just scared i will lose this code so im encasing it with comments!
        //________________________________________________________________________________




        //NOW WALL JUMP/SLIDEEE

        if(isWalled && !isGrounded && (Input.GetKey(Left) || Input.GetKey(Right)))
        {
            isWallSliding = true;
            rb.linearVelocityY = Mathf.Clamp(rb.linearVelocityY, -wallSlidingSpeed, float.MaxValue);
        }
        else
        {
            isWallSliding = false;
        }
        
        if(Input.GetKeyDown(Up) && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.linearVelocityX = wallJumpingDirection * wallJumpPower.x;
            rb.linearVelocityY = wallJumpPower.y;
            wallJumpingCounter = 0f;
            if(transform.localScale.x != wallJumpingDirection)
            {
                transform.localScale = new Vector2(wallJumpingDirection, 1);
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }

        Anim.SetBool("isWallSliding", isWallSliding);


        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;
            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }




        //WALLL SHIT IS ABOVESSS__________________________________
        //___________________________________________________________
        //____________________________________________________




        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            rb.linearVelocityY = jumpSpeed;
            Anim.SetTrigger("takeoff");
            canDoubleJump = true;
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
        if (!isWallJumping)
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
        isWalled = Physics2D.OverlapCircle(wallCheck.position, wallCheckSize, wallLayer);
    }
    private void FixedUpdate()
    {
        if (isWallJumping)
        {
            return;
        }
        
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

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckSize, groundLayer);
        Anim.SetBool("isJumping", !isGrounded);


    }

    void StopWallJumping()
    {
        isWallJumping = false;
    }
}
