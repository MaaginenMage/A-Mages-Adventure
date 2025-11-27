using UnityEngine;

public class Magic : MonoBehaviour
{
    public Mage player;

    [Header("Star")]
    public float spellRange;
    public float spellCooldown;

    [Header("Explode")]
    public GameObject explodeFX;
    public int damage = 10;
    public float damageRadius = 10f;
    public LayerMask enemyLayer;

    public bool canCast => Time.time > nextCast;
    private float nextCast;

    public void SpellAnimationFinish()
    {
        player.SpellAnimationFinish();
        CastSpell();
    }

    private void CastSpell()
    {
        //Star();
        //Dash();
        Explode();
    }

    private void Star()
    {
        if (!canCast)
        {
            return;
        }
        nextCast = Time.time + spellCooldown;
    }

    private void Dash()
    {
        if (!canCast)
        {
            return;
        }
        nextCast = Time.time + spellCooldown;
    }

    private void Explode()
    {
        if (!canCast)
        {
            return;
        }

        if (explodeFX != null)
        {
            GameObject newFx = Instantiate(explodeFX, player.transform.position, Quaternion.identity);
            Destroy(newFx, 2);
        }

        Collider2D[] enemies = Physics2D.OverlapCircleAll(player.transform.position, damageRadius, enemyLayer);

        foreach (Collider2D enemy in enemies)
        {
            Health health = enemy.GetComponent<Health>();
            if(health != null)
            {
                health.ChangeHealth(-damage);
            }
        }

        nextCast = Time.time + spellCooldown;
    }
}
