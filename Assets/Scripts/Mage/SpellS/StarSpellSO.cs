using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/StarSpell")]
public class StarSpellSO : SpellSO
{
    [Header("Explode Settings")]
    public GameObject starPrefab;
    public LayerMask enemyLayer;
    public LayerMask obstacle;
    public float speed = 25f;
    public float waveAmplitude = 3f;
    public float waveFrequency = 7f;
    public int damage = 5;

    public override void Cast(Mage player)
    {

        GameObject star = Instantiate(starPrefab, player.transform.position, Quaternion.identity);

        WavyStarProjectile proj = star.GetComponent<WavyStarProjectile>();
        proj.speed = speed;
        proj.waveAmplitude = waveAmplitude;
        proj.waveFrequency = waveFrequency;
        proj.damage = damage;
    }
}
