using UnityEngine;

public class MoveState : BaseState
{

    public MoveState(Mage player) : base(player) { }

    public override void Enter()
    {
        base.Enter();
        JumpPressed = false;
        player.attackPressed = false;
        anim.SetBool("Walking", true);
    }

    public override void Update()
    {
        base.Update();
        if (SpellPressed && magic.canCast && magic.availableSpells.Count > 0)
        {
            player.ChangeState(player.spellState);
        }
        else if (AttackPressed && combat.canAttack && !player.isAttacking)
        {
            player.ChangeState(player.attackState);
        }
        else if (JumpPressed && player.isGrounded && player.canJump)
        {
            player.ChangeState(player.jumpState);
        }
        if (rb.linearVelocity.y < -0.1f) // going down
        {
            player.ChangeState(player.fallState);
        }
        else if (Mathf.Abs(MoveInput.x) < 0.1f)
        {
            player.ChangeState(player.idleState);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        float HzMove = MoveInput.x * player.MoveSpeed;

        if (Mathf.Abs(MoveInput.x) < 0.1f && player.isGrounded)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(HzMove, rb.linearVelocity.y);
        }

        // animate walking
        if (HzMove != 0)
        {
            anim.SetBool("Walking", true);
        }
    }

    public override void Exit() 
    { 
        base.Exit();
        anim.SetBool("Walking", false);
    }
}
