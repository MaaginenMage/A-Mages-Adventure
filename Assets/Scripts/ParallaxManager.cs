using UnityEngine;

public class ParallaxBg : MonoBehaviour
{

    private float startPos, length;
    public GameObject cam;
    public float parallaxEffect;

    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float distance = cam.transform.position.x * parallaxEffect;
        float movement = cam.transform.position.x * (1 - parallaxEffect);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
    
        if (movement > startPos + length)
        {
            startPos += length;
        } 
        else if(movement < startPos - length)
        {
            startPos -= length;
        }
    }

    /* [System.Serializable]
    public class ParallaxLayer
    {
        public Transform layer;
        [Range(0, 1)] public float parallaxFactor;
    }

    public ParallaxLayer[] layers;

    public Transform camTransform;
    private Vector3 lastCameraPos;

    void Start()
    {
        lastCameraPos = camTransform.position;    
    }

    void FixedUpdate()
    {
        Vector3 cameraDelta = camTransform.position - lastCameraPos;

        foreach (ParallaxLayer layer in layers)
        {
            float moveX = cameraDelta.x * layer.parallaxFactor;
            float moveY = cameraDelta.y * layer.parallaxFactor;

            layer.layer.position += new Vector3(moveX, moveY, 0);
        }

        lastCameraPos = camTransform.position;
    } */
}
