using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(Mage player) : base(player) { }

    public override void Enter()
    {
        base.Enter();
        JumpPressed = false;
        player.attackPressed = false;
        rb.linearVelocity = new Vector2 (0, rb.linearVelocity.y);
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
        else if (rb.linearVelocity.y < -0.1f) // going down
        {
            player.ChangeState(player.fallState);
        }
        else if (JumpPressed && player.isGrounded && player.canJump)
        {
            JumpPressed = false;
            player.ChangeState(player.jumpState);
        }
        else if (Mathf.Abs(player.MoveInput.x) > 0.1f)
        {
            player.ChangeState(player.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
