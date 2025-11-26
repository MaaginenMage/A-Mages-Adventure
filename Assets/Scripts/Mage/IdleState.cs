using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(Mage player) : base(player) { }

    public override void Enter()
    {
        base.Enter();
        rb.linearVelocity = new Vector2 (0, rb.linearVelocity.y);
    }

    public override void Update()
    {
        base.Update();
        if (AttackPressed && combat.canAttack)
        {
            player.ChangeState(player.attackState);
        }
        else if (JumpPressed)
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
