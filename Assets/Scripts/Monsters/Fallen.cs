using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Fallen : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public Detector detect;
    private Animator anim;

    public float MoveSpeed = 2f;
    public float MoveDir = -1f;
    public float JumpForce = 500f;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private Coroutine AbleToJump; // null = can jump

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
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
        Debug.DrawRay(transform.position + new Vector3(2.4f * MoveDir, 1.5f), Vector2.down * 2.8f, Color.red, 0f);
        Debug.DrawRay(transform.position + new Vector3(-2f, -1.5f), Vector2.right * 4f, Color.red, 0f);
        if (Physics2D.Raycast(transform.position + new Vector3(2.4f * MoveDir, 1.5f), Vector2.down, 2.8f, 1 << 8))
        {
            MoveDir *= -1;

        }
        rb.linearVelocity = new Vector2(MoveDir * MoveSpeed, rb.linearVelocity.y);

        if (detect.InArea == true)
        {
            Jump();
            if (rb.linearVelocity.y < 0)
            {
                rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.linearVelocity.y > 0)
            {
                rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }

        if (IsOnEdge())
        {
            MoveDir *= -1;
        }

        if (!IsGrounded())
        {
            anim.SetBool("Jumping", true); // In air
        }
        else
        {
            anim.SetBool("Jumping", false); // On ground
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            MoveDir *= -1;
        }
    }

    private void Jump()
    {
        if (AbleToJump == null && IsGrounded())
        {
           AbleToJump = StartCoroutine(JumpCooldown());
           rb.AddForce(Vector2.up * JumpForce);
        }
    }
    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(-2f, -1.5f), Vector2.right, 4f, 1 << 8);
        if (hit.collider == null)
        {
            return false;
        }
        return true;
    }

    bool IsOnEdge()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(2.5f * MoveDir, -1.5f), Vector2.right, 0.1f, 1 << 8);
        if (hit.collider == null && IsGrounded())
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
