using UnityEngine;

public abstract class SpellSO : CollectibleSO
{
    [Header("General")]
    public Sprite icon;
    public float cooldown;

    public override void Collect(Mage player)
    {
        player.magic.LearnSpell(this);
    }

    public abstract void Cast(Mage player);
}