using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Mage : MonoBehaviour
{
    public Rigidbody2D rb;
    private SpriteRenderer sr;
    public Animator anim;
    private TrailRenderer tr;
    public Mages_Health MagesHealth;
    public Abilities MagesAbilities;

    //public TextMesh KillCount;
    //public TextMesh DeathCount;

    //public Transform Camera;

    public float MoveSpeed;
    public float JumpForce = 800f;
    public float DashForce = 25f;

    public float fallMultiplier = 10f;
    public float lowJumpMultiplier = 8f;
    public LayerMask groundLayer;

    public bool AbletoShoot = true;
    public float ShootCooldown = 7f;

    private bool IsDashing;
    public bool AbleToDash = true;
    public float DashCooldown = 0.5f;
    private float DashTimer = 0.2f;

    public bool AbleToUlt = true;
    public float UltCooldown = 100f;

    private int freetime = 0;
    private bool invincible = false;

    private Coroutine AbleToJump; // null = can jump
    private Coroutine DashCoroutine;

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

        AnimateAndMove();

        Jump();
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }


        if (AbleToDash == false && IsGrounded() && DashCoroutine == null)
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

    private void FixedUpdate()
    {
        if (IsDashing)
        {
            return;
        }

        float HzMove = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(HzMove * MoveSpeed, rb.linearVelocity.y);
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "EnemyHitbox")
        {
            Hit();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "HealFlowerTrigger")
        {
            MagesHealth.Healing = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        if (collision.gameObject.tag == "Flower")
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
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
        if (collision.gameObject.name == "HealFlowerTrigger" || collision.gameObject.tag == "Flower")
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                freetime++;
                if (freetime == 3)
                {
                    rb.constraints = RigidbodyConstraints2D.None;
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                }
            }
        }

        if (collision.gameObject.tag == "Spike")
        {
            Hit();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "HealFlowerTrigger" || collision.gameObject.tag == "Flower")
        {
            MagesHealth.Healing = false;
            freetime = 0;
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


    public void Jump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (AbleToJump == null)
            {
                if (IsGrounded())
                {
                    AbleToJump = StartCoroutine(JumpCooldown());
                    rb.AddForce(Vector2.up * JumpForce);
                }
            }
        }
        if (!IsGrounded())
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
        if (IsGrounded() && AbleToJump == null)
        {
            anim.SetBool("InAir", false);
            anim.SetBool("Falling", false);
        }
    }

    public bool IsGrounded()
    {
        Vector2 startPoint = new Vector2(transform.position.x - 0.3f, transform.position.y - 1.55f);
        Vector2 endPoint = new Vector2(transform.position.x + 0.3f, transform.position.y - 1.55f);

        RaycastHit2D hit = Physics2D.Linecast(startPoint, endPoint, groundLayer);
        return hit.collider != null;
    }

    IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        AbleToJump = null;
    }

    private void AnimateAndMove()
    {
        // Move left & right
        float HzMove = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(HzMove * MoveSpeed, rb.linearVelocity.y);

        // Animate walking
        if (HzMove != 0)
        {
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
        }
        // Flip sprite
        if (HzMove > 0)
        {
            sr.flipX = false;
        }
        if (HzMove < 0)
        {
            sr.flipX = true;
        }

        // Look down
        anim.SetBool("LookingDown", Input.GetKey(KeyCode.S));
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
