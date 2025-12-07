using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Components")]
    public BossTrigger bossTrigger;
    public CameraController camController;
    public Health health;
    public Animator anim;
    public GameObject Head;

    public bool attacking = false;
    public AudioClip hitDmg;
    public AudioClip whip;
    public AudioClip roar;
    public AudioClip shoot;

    [Header("Tail")]
    public GameObject tailHit;
    public float tailCooldown = 2f;
    private bool canWhip = true;
    public float tailDelay = 1.5f;
    public float tailActiveTime = 0.5f;

    [Header("Shoot")]
    public Transform mouth;
    public GameObject bulletPrefab;

    [Header("Spikes")]
    public Transform[] spikePos;
    public GameObject warning;
    public GameObject spikePrefab;
    public float warningDuration = 1f;
    public int spikeCount = 5;
    


    [Header("Death Fx")]
    [SerializeField] private GameObject[] deathParts;
    [SerializeField] private float force = 30;
    [SerializeField] private float torgue = 5;
    [SerializeField] private float lifetime = 3;

    private void Start()
    {
        tailHit.SetActive(false);
    }
    public void StartFight()
    {
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
        AudioManager.instance.PlaySFX(hitDmg);
    }

    void HandleDeath()
    {
        bossTrigger.arena.SetActive(false);
        camController.camObj.SetActive(false);
        bossTrigger.enabled = false;
        camController.enabled = false;
        AudioManager.instance.PlaySFX(roar);
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
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2.2f, 3.2f));

            int attack = Random.Range(0, 4);

            switch (attack)
            {
                case 0:
                    TailWhip();
                    break;
                case 1:
                    FireBullet();
                    break;
                case 2:
                    SpikeAttack();
                    break;
            }
        }
    }

    public void TailWhip()
    {
        if (!canWhip) return;
        StartCoroutine(TailWhipRoutine());
    }
    IEnumerator TailWhipRoutine()
    {
        canWhip = false;
        anim.SetTrigger("TailWhip");

        yield return new WaitForSeconds(tailDelay);
        tailHit.SetActive(true);
        AudioManager.instance.PlaySFX(whip);

        yield return new WaitForSeconds(tailActiveTime);
        tailHit.SetActive(false);

        yield return new WaitForSeconds(tailCooldown);
        canWhip = true;
    }

    public void FireBullet()
    {
        StartCoroutine(FireRoutine());
    }
    private IEnumerator FireRoutine()
    {
        anim.SetTrigger("Fire");
        yield return new WaitForSeconds(0.5f);  // Wait during mouth-open animation
        AudioManager.instance.PlaySFX(shoot);
        Instantiate(bulletPrefab, mouth.position, Quaternion.identity);
    }

    public void SpikeAttack()
    {
        StartCoroutine(SpikeRoutine());
    }
    IEnumerator SpikeRoutine()
    {
        anim.SetTrigger("Warn"); // tongue flick animation

        // Wait for animation (or use animation event)
        yield return new WaitForSeconds(0.4f);

        // Choose random spike spots
        List<Transform> chosen = new List<Transform>();
        for (int i = 0; i < spikeCount; i++)
        {
            chosen.Add(spikePos[Random.Range(0, spikePos.Length)]);
        }

        // Show warnings
        List<GameObject> warnings = new List<GameObject>();
        foreach (Transform t in chosen)
        {
            GameObject w = Instantiate(warning, t.position + Vector3.down * 10f, Quaternion.Euler(0f, 0f, 180f));
            warnings.Add(w);
        }

        // Wait while warnings blink
        yield return new WaitForSeconds(warningDuration);

        // Spawn actual spikes
        foreach (Transform t in chosen)
        {
            Instantiate(spikePrefab, t.position, Quaternion.Euler(0f, 0f, 180f));
        }

        // Remove warnings
        foreach (GameObject w in warnings)
            Destroy(w);
    }

}
