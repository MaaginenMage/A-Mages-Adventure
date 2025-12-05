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
    public float gCheckRad = 0.5f;
    public LayerMask groundLayer;
    public bool isGrounded;
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
        if (MoveDir == -1f)
        {
            sr.flipX = false;
        }
        if (MoveDir == 1f)
        {
            sr.flipX = true;
        }
        rb.linearVelocity = new Vector2(MoveDir * MoveSpeed, rb.linearVelocity.y);

        if (!isGrounded)
        {
            anim.SetBool("Jumping", true); // In air
        }
        else
        {
            anim.SetBool("Jumping", false); // On ground
        }
    }

    void FixedUpdate()
    {
        if (IsOnEdge() && isGrounded)
        {
            MoveDir *= -1;
        }
        else if (IsHittingWall())
        {
            MoveDir *= -1;

        }
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
        Collider2D hit = Physics2D.OverlapCircle(groundCheck.position, gCheckRad, groundLayer);
        isGrounded = hit != null;

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, gCheckRad);
    }

    bool IsHittingWall()
    {
        Vector2 origin = (Vector2)transform.position + new Vector2(MoveDir * 2f, -0.5f);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.right * MoveDir, 0.2f, groundLayer);

        Debug.DrawRay(origin, Vector2.right * MoveDir * 0.2f, Color.red);

        return hit.collider != null;
    }

    bool IsOnEdge()
    {
        Vector2 origin = (Vector2)transform.position + new Vector2(MoveDir * 2f, -1.3f);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 0.3f, groundLayer);

        Debug.DrawRay(origin, Vector2.down * 0.3f, Color.green);

        return hit.collider == null;   // no ground = edge
    }
    IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(3f);
        AbleToJump = null;
    }
}
