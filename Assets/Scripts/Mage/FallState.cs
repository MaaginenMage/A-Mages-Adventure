using UnityEngine;

public class FallState : BaseState
{
    public FallState(Mage player) : base(player) { }

    public override void Enter()
    {
        base.Enter();
        JumpPressed = false;
        player.attackPressed = false;
        anim.SetBool("Falling", true);
        player.canJump = false;
    }

    public override void Update()
    {
        base.Update();

        if (AttackPressed && combat.canAttack)
        {
            player.ChangeState(player.attackState);
        }
        else if (player.isGrounded)
        {
            if (Mathf.Abs(player.MoveInput.x) > 0.1f)
                player.ChangeState(player.moveState);
            else
                player.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        anim.SetBool("Falling", false);
        player.canJump = true;
    }
}
