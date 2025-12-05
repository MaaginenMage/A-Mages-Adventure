using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Health health;
    public Animator anim;
    public GameObject Head;

    [Header("Death Fx")]
    [SerializeField] private GameObject[] deathParts;
    [SerializeField] private float force = 5;
    [SerializeField] private float torgue = 5;
    [SerializeField] private float lifetime = 2;

    public void StartFight()
    {
        // enable AI, attacks, etc.
        // typically:
        StartCoroutine(AttackLoop());
    }

    private void OnEnable()
    {
        health.OnDamaged += HandleDamage;
        health.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        health.OnDamaged -= HandleDamage;
        health.OnDeath -= HandleDeath;
    }

    void HandleDamage()
    {
        anim.SetTrigger("IsDamaged");
    }

    void HandleDeath()
    {
        foreach (GameObject prefab in deathParts)
        {
            Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(0.5f, 1f)).normalized;
            GameObject part = Instantiate(prefab, transform.position, rotation);

            Rigidbody2D rb = part.GetComponent<Rigidbody2D>();
            Vector2 randomDir = new Vector2(Random.Range(-1, 1), Random.Range(0.5f, 1)).normalized;
            rb.linearVelocity = randomDir * force;
            rb.AddTorque(Random.Range(-torgue, torgue), ForceMode2D.Impulse);

            Destroy(part, lifetime);
        }

        Destroy(gameObject);
    }

    IEnumerator AttackLoop()
    {
        yield return new WaitForSeconds(0.6f);
    }
}
