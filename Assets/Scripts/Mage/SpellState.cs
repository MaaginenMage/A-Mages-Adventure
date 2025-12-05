using UnityEngine;

public class SpellState : BaseState
{
    public SpellState(Mage player) : base(player) { }

    public override void Enter()
    {
        base.Enter();
        player.spellPressed = false;
        anim.SetBool("Spell", true);
    }

    public override void SpellAnimationFinish()
    {
        base.AnimationFinish();

        if (!player.isGrounded)
        {
            player.ChangeState(player.fallState);
        } else if (Mathf.Abs(MoveInput.x) > 0.1f)
        {
            player.ChangeState(player.moveState);
        } else
        {
            player.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        anim.SetBool("Spell", false);
    }
}
