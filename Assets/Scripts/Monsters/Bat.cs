using UnityEngine;

public class Bat : MonoBehaviour
{
    public Health health;

    public Animator anim;
    public float speed;
    public bool chase = false;
    public Transform startingPoint;
    private GameObject player;
    public AudioClip hitDmg;

    [SerializeField] private GameObject[] deathParts;
    [SerializeField] private float force = 5;
    [SerializeField] private float torgue = 5;
    [SerializeField] private float lifetime = 2;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }
        if (chase == true)
        {
            Chase();
        } else
        {
            ReturnToStart();
        }
            Flip();
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
        anim.SetTrigger("isDamaged");
        AudioManager.instance.PlaySFX(hitDmg);
    }

    void HandleDeath()
    {
        AudioManager.instance.PlaySFX(hitDmg);
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

    private void Chase()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    private void ReturnToStart()
    {
        transform.position = Vector2.MoveTowards(transform.position, startingPoint.position, speed * Time.deltaTime);
    }

    private void Flip()
    {
        if (transform.position.x > player.transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        } else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
