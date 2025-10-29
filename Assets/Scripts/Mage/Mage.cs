using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Mage : MonoBehaviour
{
    public Rigidbody2D rb;
    public PlayerInput playerInput;
    private SpriteRenderer sr;
    public Animator anim;
    private TrailRenderer tr;
    public Mages_Health MagesHealth;
    public Abilities MagesAbilities;
    public Transform groundCheck;
    public float gCheckRad;
    public LayerMask groundLayer;

    [Header("Movement")]
    public float MoveSpeed;
    public float JumpForce;
    public float JumpCut;
    public float Gravity;
    public float FallGravity;
    public float JumpGravity;
    public float DashForce = 25f;

    [Header("Inputs")]
    private Vector2 MoveInput;
    public bool JumpPressed;
    private bool JumpReleased;

    public float fallMultiplier = 10f;
    public float lowJumpMultiplier = 8f;
    private bool isGrounded;

    [Header("abilities")]
    // star
    public bool AbletoShoot;
    public float ShootCooldown = 7f;
    // Dash
    private bool IsDashing;
    public bool AbleToDash;
    public float DashCooldown = 0.5f;
    private float DashTimer = 0.2f;
    private Coroutine DashCoroutine;
    // ult
    public bool AbleToUlt;
    public float UltCooldown = 100f;

    [Header("Misc")]
    // flowers
    private bool stuck = false;
    private float freetime = 0;
    private bool invincible = false;



    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        tr = GetComponentInChildren<TrailRenderer>();
        tr.emitting = false; // start disabled

        DashCoroutine = null;
    }

    private void Update()
    {
        if (IsDashing)
        {
            return;
        }

        Flip();
        HandleAnimations();

        if (AbleToDash == false && isGrounded && DashCoroutine == null)
        {
            DashCoroutine = StartCoroutine(WaitForDash());
        }

        if (MagesAbilities.DashUnlocked == true)
        {
            if (Input.GetKey(KeyCode.LeftShift) && AbleToDash && DashCoroutine == null)
            {
                StartCoroutine(Dash());
            }
        }
    }

    void FixedUpdate()
    {
        if (IsDashing)
        {
            return;
        }

        CheckGrounded();
        ApplyVariableGravity();
        AnimateAndMove();
        HandleJump();
    }


    private void AnimateAndMove()
    {
        float HzMove = MoveInput.x * MoveSpeed;
        rb.linearVelocity = new Vector2(HzMove, rb.linearVelocity.y);
        
        // animate walking
        if (HzMove != 0)
        {
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
        }

        // Look down
        //anim.SetBool("LookingDown", true);
    }

    private void HandleJump()
    {
        if (JumpPressed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpForce);
            JumpReleased = false;
        }
        JumpPressed = false;
        if (JumpReleased)
        {
            if (rb.linearVelocityY > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * JumpCut);
            }
            JumpReleased = false;
        }
    }

    void HandleAnimations()
    {
        if (!isGrounded)
        {
            anim.SetBool("InAir", true);

            if (rb.linearVelocity.y < -0.1f) // going down
            {
                anim.SetBool("Falling", true);
            }
            else // going up
            {
                anim.SetBool("Falling", false);
            }
        }
        if (isGrounded)
        {
            anim.SetBool("InAir", false);
            anim.SetBool("Falling", false);
        }
    }


    void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, gCheckRad, groundLayer);
    }

    void Flip()
    {
        if (MoveInput.x > 0.1f)
        {
            sr.flipX = false;
        }
        else if (MoveInput.x < -0.1f)
        {
            sr.flipX = true;
        }
    }

    void ApplyVariableGravity()
    {
        if (rb.linearVelocity.y < -0.1f)
        {
            rb.gravityScale = FallGravity;
        }
        else if (rb.linearVelocity.y > 0.1f)
        {
            rb.gravityScale = JumpGravity;
        }
        else
        {
            rb.gravityScale = Gravity;
        }
    }

    public void OnMove(InputValue inValue)
    {
        MoveInput = inValue.Get<Vector2>();
    }

    public void OnJump(InputValue inValue)
    {
        if (inValue.isPressed)
        {
            JumpPressed = true;
            JumpReleased = false;
        }
        else
        {
            JumpReleased = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, gCheckRad);
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Flowers
        if (collision.gameObject.name == "HealFlowerTrigger")
        {
            stuck = true;
            MagesHealth.Healing = true;
            rb.gravityScale = 3f;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            if (isGrounded)
            {
                rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            }
        }
        if (collision.gameObject.tag == "Flower")
        {
            stuck = true;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            if (isGrounded)
            {
                rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            }
        }

        if (collision.gameObject.name == "SkillBook-1")
        {
            MagesAbilities.StarUnlocked = true;
        }
        if (collision.gameObject.name == "SkillBook-2")
        {
            MagesAbilities.DashUnlocked = true;
        }
        if (collision.gameObject.name == "SkillBook-3")
        {
            MagesAbilities.UltUnlocked = true;
        }

        if (collision.gameObject.tag == "Spike")
        {
            Hit();
        }

        if (collision.gameObject.tag == "EnemyHitbox")
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            Hit();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (stuck && collision.gameObject.name == "HealFlowerTrigger" || collision.gameObject.tag == "Flower")
        {
            if (isGrounded)
            {
                rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            }
            if (JumpPressed)
            {
                freetime++;
                if (freetime >= 3)
                {
                    stuck = false;
                    freetime = 0;
                    MagesHealth.Healing = false;
                    rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
                }
            }
        }

        if (collision.gameObject.tag == "Spike" || collision.gameObject.tag == "EnemyHitbox")
        {
            Hit();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "HealFlowerTrigger" || collision.gameObject.tag == "Flower")
        {
            stuck = false;
            freetime = 0;
            MagesHealth.Healing = false;
            rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public void Hit()
    {
        if (invincible) return;
        MagesHealth.Health -= 1;
        if (MagesHealth.Health == 0)
        {
            StartCoroutine(Resurrect());
        }
        StartCoroutine(InvincibilityFrames());
    }
    IEnumerator InvincibilityFrames()
    {
        invincible = true;
        yield return new WaitForSeconds(1f);
        invincible = false;
    }
    IEnumerator Resurrect()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(1f);
        transform.position = new Vector3(-5, -2.48f, 0);
        MagesHealth.Health = 7;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private IEnumerator Dash()
    {
        IsDashing = true;
        AbleToDash = false;
        tr.emitting = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        float direction = Input.GetAxisRaw("Horizontal");
        if (direction == 0)
            direction = transform.localScale.x; // dash in facing direction if no input

        rb.linearVelocity = new Vector2(direction * DashForce, 0f);

        anim.SetBool("Dashing", true);

        yield return new WaitForSeconds(DashTimer);

        rb.linearVelocity = new Vector2(direction * (DashForce / 2f), rb.linearVelocity.y); // soften exit
        rb.gravityScale = originalGravity;
        tr.emitting = false;
        IsDashing = false;

        transform.localScale = new Vector3(2, 3, 1);
        anim.SetBool("Dashing", false);

        yield return new WaitForSeconds(DashCooldown);
        AbleToDash = true;
        DashCoroutine = null;
    }

    IEnumerator WaitForDash()
    {
        yield return new WaitForSeconds(DashCooldown);
        DashCoroutine = null;
        AbleToDash = true;
    }
}
