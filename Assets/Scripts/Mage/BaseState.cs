using UnityEngine;

public abstract class BaseState
{
    protected Mage player;
    protected Animator anim;
    protected Rigidbody2D rb;
    protected Combat combat;

    protected bool JumpPressed {get => player.JumpPressed; set => player.JumpPressed = value;}
    protected bool JumpReleased {get => player.JumpReleased; set => player.JumpReleased = value;}
    protected Vector2 MoveInput => player.MoveInput;
    protected bool AttackPressed => player.attackPressed;

    public BaseState (Mage player)
    {
        this.player = player;
        this.anim = player.anim;
        this.rb = player.rb;
        this.combat = player.combat;
    }

    public virtual void Enter(){}
    public virtual void Exit(){}
    public virtual void Update(){}
    public virtual void FixedUpdate(){}
    public virtual void AnimationFinish(){}
}
