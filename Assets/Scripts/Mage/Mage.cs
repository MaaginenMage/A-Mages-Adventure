using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class Mage : MonoBehaviour
{
    public BaseState currentState;
    public IdleState idleState;
    public JumpState jumpState;
    public MoveState moveState;
    public AttackState attackState;
    public SpellState spellState;
    public FallState fallState;

    public AudioClip spell;
    public AudioClip hitDmg;

    [Header("Core Components")]
    public Combat combat;
    public Magic magic;

    [Header("Components")]
    public Rigidbody2D rb;
    public PlayerInput playerInput;
    private SpriteRenderer sr;
    public Animator anim;
    private TrailRenderer tr;
    public Mages_Health MagesHealth;
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
    private bool facingRight;

    [Header("Inputs")]
    public Vector2 MoveInput;
    public bool JumpPressed;
    public bool JumpReleased;
    public bool attackPressed;
    public bool spellPressed;

    public float fallMultiplier = 10f;
    public float lowJumpMultiplier = 8f;
    public bool isGrounded;
    public bool isAttacking = false;
    public bool canJump = true;

    [Header("Misc")]
    private bool stuck = false;
    private float freetime = 0;
    private bool invincible = false;
    public float knockbackForce = 10f;


    private void Awake()
    {
        idleState = new IdleState(this);
        jumpState = new JumpState(this);
        moveState = new MoveState(this);
        attackState = new AttackState(this);
        spellState = new SpellState(this);
        fallState = new FallState(this);

        ChangeState(idleState);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        tr = GetComponentInChildren<TrailRenderer>();
        ChangeState(idleState);
        tr.emitting = false; // start disabled
    }

    private void Update()
    {
        currentState.Update();
        if (currentState == null)
        {
            ChangeState(idleState);
        }
        /*if (IsDashing)
        {
            return;
        }*/

        Flip();
        //HandleAnimations();
    }

    void FixedUpdate()
    {
        currentState.FixedUpdate();
        /*if (IsDashing)
        {
            return;
        }*/
        CheckGrounded();
        AnimateAndMove();
    }

    public void ChangeState(BaseState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter();
    }

    public void AnimationFinish()
    {
        currentState.AnimationFinish();
    }
    public void SpellAnimationFinish()
    {
        currentState.SpellAnimationFinish();
    }


    private void AnimateAndMove()
    {
        // Look down
        //anim.SetBool("LookingDown", true);
    }

    void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, gCheckRad, groundLayer);
    }

    void Flip()
    {
        if (MoveInput.x > 0.1f && !facingRight && currentState != attackState)
        {
            facingRight = !facingRight;
            sr.flipX = false;
            float x = Mathf.Abs(combat.attackPoint.localPosition.x);
            combat.attackPoint.localPosition = new Vector3(facingRight ? x : -x, combat.attackPoint.localPosition.y, combat.attackPoint.localPosition.z);
            float x2 = Mathf.Abs(combat.hitFXloc.localPosition.x);
            combat.hitFXloc.localPosition = new Vector3(facingRight ? x2 : -x2, combat.hitFXloc.localPosition.y, combat.hitFXloc.localPosition.z);
        }
        else if (MoveInput.x < -0.1f && facingRight && currentState != attackState)
        {
            facingRight = !facingRight;
            sr.flipX = true;
            float x = Mathf.Abs(combat.attackPoint.localPosition.x);
            combat.attackPoint.localPosition = new Vector3(facingRight ? x : -x, combat.attackPoint.localPosition.y, combat.attackPoint.localPosition.z);
            float x2 = Mathf.Abs(combat.hitFXloc.localPosition.x);
            combat.hitFXloc.localPosition = new Vector3(facingRight ? x2 : -x2, combat.hitFXloc.localPosition.y, combat.hitFXloc.localPosition.z);
        }
    }

    public void ApplyVariableGravity()
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

    public void OnAttack(InputValue value)
    {
        attackPressed = value.isPressed;
    }

    public void OnSpell(InputValue value)
    {
        spellPressed = value.isPressed;
        AudioManager.instance.PlaySFX(spell);
    }

    public void OnPrevious(InputValue value)
    {
        if (value.isPressed)
        {
            magic.PreviousSpell();
        }
    }
    public void OnNext(InputValue value)
    {
        if (value.isPressed)
        {
            magic.NextSpell();
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
        AudioManager.instance.PlaySFX(hitDmg);
        anim.SetTrigger("IsHurt");
        MagesHealth.Health -= 1;
        if (MagesHealth.Health == 0)
        {
            StartCoroutine(Resurrect());
        }
        int dir;
        if (facingRight)
        {
            dir = -1;
        }
        else
        {
            dir = 1;
        }
        rb.linearVelocity = new Vector2(dir * knockbackForce, 12f);
        StartCoroutine(InvincibilityFrames());
    }
    IEnumerator InvincibilityFrames()
    {
        invincible = true;
        yield return new WaitForSeconds(1f);
        invincible = false;
    }
    public IEnumerator Resurrect()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        anim.SetBool("InAir", true);
        yield return new WaitForSeconds(1f);
        FadeManager.instance.FadeAndLoadScene(SceneManager.GetActiveScene().name);
    }

    /* private IEnumerator Dash()
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
    }*/
}