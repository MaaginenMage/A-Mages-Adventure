using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Fallen : MonoBehaviour
{
    [Header("Components")]
    public Health health;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public GameObject detect;
    private Animator anim;

    [Header("Function")]
    public Transform groundCheck;
    public float gCheckRad;
    public LayerMask groundLayer;
    private bool isGrounded;
    public float JumpForce = 20;
    public float fallMultiplier = 10f;

    public float MoveSpeed = 2f;
    public float MoveDir = -1f;

    [Header("Death Fx")]
    [SerializeField] private GameObject[] deathParts;
    [SerializeField] private float force = 5;
    [SerializeField] private float torgue = 5;
    [SerializeField] private float lifetime = 2;

    private Coroutine AbleToJump; // null = can jump

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        health.OnDamaged += HandleDamage;
        health.OnDeath += HandleDeath;
    }
    private void OnDisable()
    {
        health.OnDamaged -= HandleDamage;
        health.OnDeath -= HandleDeath;
    }
    void HandleDamage()
    {
        anim.SetTrigger("isDamaged");
    }

    void HandleDeath()
    {
        foreach (GameObject prefab in deathParts)
        {
            Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(0.5f, 1f)).normalized;
            GameObject part = Instantiate(prefab, transform.position, rotation);

            Rigidbody2D rb = part.GetComponent<Rigidbody2D>();
            Vector2 randomDir = new Vector2(Random.Range(-1, 1), Random.Range(0.5f, 1)).normalized;
            rb.linearVelocity = randomDir * force;
            rb.AddTorque(Random.Range(-torgue, torgue), ForceMode2D.Impulse);

            Destroy(part, lifetime);
        }

        Destroy(gameObject);
    }

    private void Update()
    {
        Debug.Log("Grounded: " + isGrounded);

        if (MoveDir == -1f)
        {
            sr.flipX = false;
        }
        if (MoveDir == 1f)
        {
            sr.flipX = true;
        }
        if (Physics2D.Raycast(transform.position + new Vector3(2.4f * MoveDir, 1.5f), Vector2.down, 2.8f, 1 << 8))
        {
            MoveDir *= -1;

        }
        rb.linearVelocity = new Vector2(MoveDir * MoveSpeed, rb.linearVelocity.y);

        if (IsOnEdge())
        {
            MoveDir *= -1;
        }

        /*if (!isGrounded)
        {
            anim.SetBool("Jumping", true); // In air
        }
        else if (isGrounded)
        {
            anim.SetBool("Jumping", false); // On ground
        }*/
    }

    void FixedUpdate()
    {
        CheckGrounded();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyHitbox")
        {
            MoveDir *= -1;
        }
        if (collision.gameObject.tag == "Player")
        {
            Jump();
            if (rb.linearVelocity.y < 0)
            {
                rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
        }
    }

    private void Jump()
    {
        if (AbleToJump == null && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpForce);
            AbleToJump = StartCoroutine(JumpCooldown());
        }
    }
    void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, gCheckRad, groundLayer);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, gCheckRad);
    }

    bool IsOnEdge()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(2.5f * MoveDir, -1.5f), Vector2.right, 0.1f, 1 << 8);
        if (hit.collider == null && isGrounded)
        {
            return true;
        }
        return false;
    }
    IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(3f);
        AbleToJump = null;
    }
}
