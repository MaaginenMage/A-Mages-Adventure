using UnityEngine;

public class MainBullet : MonoBehaviour
{
    public float speed = 5f;
    public float splitTime = 1f;
    public GameObject smallBulletPrefab;

    private Vector2 moveDirection;

    void Start()
    {
        // Choose a random diagonal direction downward-left
        float angle = Random.Range(200f, 280f); // Only down-left
        float rad = angle * Mathf.Deg2Rad;
        moveDirection = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

        // After some time, split
        Invoke(nameof(Split), splitTime);
    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    void Split()
    {
        // directions for four-bullet X-shape
        Vector2[] dirs =
        {
            new Vector2(1, 1).normalized,
            new Vector2(1, -1).normalized,
            new Vector2(-1, 1).normalized,
            new Vector2(-1, -1).normalized
        };

        foreach (var d in dirs)
        {
            GameObject bullet = Instantiate(smallBulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<SmallBullet>().Initialize(d);
        }

        Destroy(gameObject);
    }
}
