using UnityEngine;

public class JumpState : BaseState
{
    public JumpState(Mage player) : base(player) { }

    public override void Enter()
    {
        base.Enter();
        anim.SetBool("InAir", true);

        rb.linearVelocity = new Vector2 (rb.linearVelocity.x, player.JumpForce);

        JumpPressed = false;
        JumpReleased = false;
    }

    public override void Update()
    {
        base.Update();
        if (player.isGrounded && rb.linearVelocity.y <= 0)
        {
            player.ChangeState(player.idleState);
        }

        if (!player.isGrounded)
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
        if (player.isGrounded)
        {
            anim.SetBool("InAir", false);
            anim.SetBool("Falling", false);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        player.ApplyVariableGravity();

        if (JumpReleased && rb.linearVelocityY > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * player.JumpCut);
            JumpReleased = false;
        }
        if (MoveInput.x > 0.1f)
        {
            rb.linearVelocity = new Vector2(player.MoveSpeed, rb.linearVelocityY);
        }
        else if (MoveInput.x < -0.1f)
        {
            rb.linearVelocity = new Vector2(player.MoveSpeed * -1, rb.linearVelocityY);
        }
    }

    public override void Exit()
    {
        base.Exit();

        anim.SetBool("InAir", false);
    }
}