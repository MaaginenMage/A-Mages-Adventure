using UnityEngine;

public class JumpState : BaseState
{
    public JumpState(Mage player) : base(player) { }

    public override void Enter()
    {
        base.Enter();
        anim.SetBool("InAir", true);
        player.canJump = false;

        rb.linearVelocity = new Vector2 (rb.linearVelocity.x, player.JumpForce);

        JumpPressed = false;
        JumpReleased = false;
        player.attackPressed = false;
    }

    public override void Update()
    {
        base.Update();

        if (rb.linearVelocity.y < -0.1f && !player.isGrounded) // going down
        {
            player.ChangeState(player.fallState);
        }
        if (AttackPressed && combat.canAttack)
        {
            player.ChangeState(player.attackState);
        }
        if (SpellPressed && magic.canCast && magic.availableSpells.Count > 0)
        {
            player.ChangeState(player.spellState);
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
        player.canJump = true;
    }
}