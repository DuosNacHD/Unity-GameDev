using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    public Vector3 Spawnpoint;

    [SerializeField] int Health;
    [SerializeField] float speed, jump, coyoteTime;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;

    Rigidbody2D rb;
    Animator anim;
    float direction, coyoteTimer;
    bool reJump, reUnJump;
    void Start()
    {
        Spawnpoint = transform.position;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {     
        Move();
        DeathF();
        JumpF();
        Flip();
        Animations();
    }

    private void OnTriggerEnter2D(Collider2D collision)
{
        
            IInteractable interact = collision.gameObject.GetComponent<IInteractable>();
            if (interact != null)
            {
                interact.Interact();
            }
        
    }
    #region Animations
    void Animations()
    {
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        anim.SetBool("Grounded", Grounded());
    }
    void Flip()
    {
        if (direction < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (direction > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }
    #endregion
    #region Jump
    void JumpF()
    {
        if (Grounded())
        {
            coyoteTimer = Time.time + coyoteTime;
        }
        if (reJump)
        {
            Jump();
        }
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        if (Input.GetButtonUp("Jump"))
        {
            UnJump();
        }
    }
    public void Jump()
    {
        if (coyoteTimer >= Time.time)
        {
            if (reJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jump * 0.9f);
                reJump = false;
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, jump);
                reUnJump = true;
            }
            coyoteTimer = 0;
        }
        else
        {
            if (rb.velocity.y < 0)
            {
                reJump = true;
            }
        }
    }
    public void UnJump()
    {
        if (rb.velocity.y > 0 && reUnJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);
            coyoteTimer = 0;
            reUnJump = false;
        }
    }
    bool Grounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
    #endregion
    #region Move
    void Move()
    {        
            direction = Input.GetAxis("Horizontal");       
    }
    void FixedUpdate()
    {
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
    }
    #endregion
    #region Live
    void DeathF()
    {
        if (transform.position.y <= -7)
        {
            death();
        }
        if (Health <= 0)
        {
            death();
        }
    }
    void death()
    {
        transform.position = Spawnpoint;
        rb.velocity = Vector2.zero;
        Health = 5;
    }
    public void Hurt(int Damage)
    {
        Health -= Damage;
        if (Health > 0)
        {
            anim.SetTrigger("Hurt");
        }
    }
    #endregion
}
