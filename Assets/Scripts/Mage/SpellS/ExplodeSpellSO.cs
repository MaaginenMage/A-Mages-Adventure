using UnityEngine;

[CreateAssetMenu(menuName = "Spells/ExplodeSpell")]
public class ExplodeSpellSO : SpellSO
{
    [Header("Explode Settings")]
    public GameObject explodeFX;
    public int damage = 10;
    public float damageRadius = 10f;
    public LayerMask enemyLayer;

    public override void Cast(Mage player)
    {
        if (explodeFX != null)
        {
            GameObject newFx = Instantiate(explodeFX, player.transform.position, Quaternion.identity);
            Destroy(newFx, 2);
        }

        Collider2D[] enemies = Physics2D.OverlapCircleAll(player.transform.position, damageRadius, enemyLayer);

        foreach (Collider2D enemy in enemies)
        {
            Health health = enemy.GetComponent<Health>();
            if (health != null)
            {
                health.ChangeHealth(-damage);
            }
        }
    }
}
