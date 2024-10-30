using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rgb;
    private Animator animator;
    private bool isGrounded;
    private bool isFacingRight = true;

    public float jumpMultiplier = 2.5f;
    public float fallMultiplier = 1.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rgb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Move();
        UpdateAnimationStates();
        ApplyJumpAndFallMultipliers();

        //Debug Log
        Debug.Log("IsJumping: " + animator.GetBool("IsJumping"));
        Debug.Log("IsFalling: " + animator.GetBool("IsFalling"));
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rgb.linearVelocity = new Vector2(moveInput * moveSpeed, rgb.linearVelocity.y);

        //Define stage boundaries
        float minX = -6.5f;
        float maxX = 22f;
        float minY = -5f;
        float maxY = 6f;

        //Clamp player horizontal position within boundaries
        if (transform.position.x < minX)
        {
            transform.position = new Vector2(minX, transform.position.y);
        }
        else if (transform.position.x > maxX)
        {
            transform.position = new Vector2(maxX, transform.position.y);
        }

        //Clamp player vertical position within boundaries
        if (transform.position.y < minY)
        {
            transform.position = new Vector2(transform.position.x, minY);
        }
        else if (transform.position.y > maxY)
        {
            transform.position = new Vector2(transform.position.x, maxY);
        }


        if (moveInput > 0.01f && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < -0.01f && isFacingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight; //Toggle facing direction

        //Flip texture
        float rotationY = isFacingRight ? 0 : 180;
        transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
    }

    void Jump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rgb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }

        //Debug statement to check if isGrounded is updating correctly
        Debug.Log("IsGrounded: " + isGrounded);
    }

    void UpdateAnimationStates()
    {
        //Update grounded state
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        //Update animation state
        animator.SetFloat("Speed", Mathf.Abs(rgb.linearVelocity.x));
        animator.SetBool("IsJumping", !isGrounded && rgb.linearVelocity.y > 0); //True if in the air and moving up
        animator.SetBool("IsFalling", !isGrounded && rgb.linearVelocity.y < 0); //True if in the air and moving down
    }

    void ApplyJumpAndFallMultipliers()
    {
        //Apply jump multiplier when moving upwards
        if (rgb.linearVelocity.y > 0)
        {
            rgb.linearVelocity += Vector2.up * Physics2D.gravity.y * Time.deltaTime * fallMultiplier;
        }
        else if (rgb.linearVelocity.y < 0)
        {
            rgb.linearVelocity += Vector2.up * Physics2D.gravity.y * Time.deltaTime * fallMultiplier;
        }
    }
}

