using Unity.VisualScripting;
using UnityEngine;

public class AttackState : BaseState
{
    public AttackState(Mage player) : base(player) { }

    public override void Enter()
    {
        base.Enter();

        player.attackPressed = false;
        anim.SetBool("Attacking", true);
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    public override void AnimationFinish()
    {
        if (Mathf.Abs(MoveInput.x) > 0.1f)
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
    }
}
