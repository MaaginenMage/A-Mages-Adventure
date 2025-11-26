using UnityEngine;

public class SpellState : BaseState
{
    public SpellState(Mage player) : base(player) { }

    public override void Enter()
    {
        base.Enter();
        anim.SetBool("Spell", true);
    }

    public override void AnimationFinish()
    {
        base.AnimationFinish();

        if (Mathf.Abs(MoveInput.x) > 0.1f)
        {
            player.ChangeState(player.moveState);
        } else
        {
            player.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Enter();
        anim.SetBool("Spell", false);
    }
}
