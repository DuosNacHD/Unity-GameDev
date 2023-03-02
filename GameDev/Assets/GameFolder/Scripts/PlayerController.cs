using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerController : MonoBehaviour
{
    public float Money;
    public Vector3 Spawnpoint;

    [SerializeField] TMP_Text textE;
    [SerializeField] int Health;
    [SerializeField] float speed, jump, coyoteTime;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;

    IInteractable interactable;
    Rigidbody2D rb;
    Animator anim;
    float direction, coyoteTimer;
    bool reJump, reUnJump,Interacting;
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
        Interact();
    }
    #region InteractableControl
    void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E) && Interacting)
        {
            interactable.Interactable();
            Interacting = false;
            textE.enabled = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

         interactable = collision.gameObject.GetComponent<IInteractable>();
        if (interactable != null) 
        {
            Interacting = true;
            textE.enabled = true;
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
         interactable = collision.gameObject.GetComponent<IInteractable>();
        if (interactable != null)
        {
            Interacting = false;
            textE.enabled = false;
        }
    }
    #endregion
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
