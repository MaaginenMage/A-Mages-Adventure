using UnityEngine;

public class SmallBullet : MonoBehaviour
{
    public float speed = 6f;
    private Vector2 direction;

    public void Initialize(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Start() 
    { 
        Destroy(gameObject, 4f); 
    }


    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
