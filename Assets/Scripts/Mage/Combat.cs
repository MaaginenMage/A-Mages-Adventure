using UnityEngine;

public class Combat : MonoBehaviour
{
    public Mage player;

    [Header("Attack")]
    public int damage;
    public float attackRadius = 0.7f;
    public float attackCooldown = 1f;
    public Transform attackPoint;
    public LayerMask enemyLayer;
    public Animator hitFX;
    public Transform hitFXloc;

    private float nextAttackTime;
    public bool canAttack => Time.time > nextAttackTime;

    public void AnimationFinish()
    {
        player.AnimationFinish();
    }

    public void Attack()
    {
        if (!canAttack)
        {
            return;
        }

        nextAttackTime = Time.time + attackCooldown;

        Collider2D enemy = Physics2D.OverlapCircle(attackPoint.position, attackRadius, enemyLayer);
        if (enemy != null)
        {
            hitFX.Play("Hit");
            enemy.gameObject.GetComponent<Health>().ChangeHealth(-damage);
        }
    }
}