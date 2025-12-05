using Unity.VisualScripting;
using UnityEngine;

public class AttackState : BaseState
{
    public AttackState(Mage player) : base(player) { }

    public override void Enter()
    {
        base.Enter();

        JumpPressed = false;
        player.attackPressed = false;
        anim.SetBool("Attacking", true);
        if (player.isGrounded)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
        player.isAttacking = true;
    }

    public override void AnimationFinish()
    {
        if (!player.isGrounded)
        {
            player.ChangeState(player.fallState);
        }
        else if (Mathf.Abs(MoveInput.x) > 0.1f && player.isGrounded)
        {
            player.ChangeState(player.moveState);
        }
        else
        {
            player.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        anim.SetBool("Attacking", false);
        player.isAttacking = false;
    }
}
