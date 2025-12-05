using UnityEngine;

public class WavyStarProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float waveAmplitude = 1f;
    public float waveFrequency = 5f;
    public GameObject bossHead;
    public int damage = 5;
    public float lifeTime = 4f;
    public int direction = 1;

    private float timeAlive = 0f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        timeAlive += Time.deltaTime;

        // Horizontal movement
        float x = direction * speed * timeAlive;

        // Vertical wave movement
        float y = Mathf.Sin(timeAlive * waveFrequency) * waveAmplitude;

        transform.position = startPos + new Vector3(x, y, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == bossHead)
        {
            damage *= 3;
        }
        Health h = other.GetComponent<Health>();
        if (h != null)
        {
            h.ChangeHealth(-damage);
            Destroy(gameObject);
        }
        damage = 5;
    }
}
