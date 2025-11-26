using UnityEngine;

public class Flowers : MonoBehaviour
{
    private Mage playerMove;
    private Rigidbody2D rb;
    public GameObject mashText;
    private bool stuck;
    private float freetime;

    void Start()
    {
        playerMove = GameObject.Find("Mage").GetComponent<Mage>();
        mashText.SetActive(false);
        rb = playerMove.rb;
    }

    // Update is called once per frame
    void Update()
    {
        if (stuck)
        {
            mashText.SetActive(true);
            StopPlayer();
            if (playerMove.JumpPressed)
            {
                Debug.Log("Jump");
                freetime++;
                if (freetime >= 10)
                {
                    FreePlayer();
                }
            }
        }
        if (stuck && playerMove.isGrounded)
        {
            rb.constraints =
                RigidbodyConstraints2D.FreezeAll;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Mage")
        {
            stuck = true;
            freetime = 0;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Mage")
        {
            FreePlayer();
        }
    }


    void StopPlayer()
    {
        Debug.Log("Stop");
        playerMove.anim.SetBool("LookingDown", true);

        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        rb.constraints =
            RigidbodyConstraints2D.FreezePositionX |
            RigidbodyConstraints2D.FreezeRotation;
    }

    void FreePlayer()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        stuck = false;
        freetime = 0;
        mashText.SetActive(false);
        playerMove.anim.SetBool("LookingDown", false);
    }
}
