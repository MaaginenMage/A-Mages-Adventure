using UnityEngine;

public class Flowers : MonoBehaviour
{
    private Mage playerMove;
    private bool stuck = false;
    private int freetime = 0;

    void Start()
    {
        playerMove = GameObject.Find("Mage").GetComponent<Mage>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stuck)
        {
            StopPlayer();
            if (playerMove.JumpPressed)
            {
                freetime++;
                if (freetime >= 5)
                {
                    FreePlayer();
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Mage")
        {
            playerInside = true;
            mashCount = 0;
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
        playerMove.anim.SetBool("Walking", false);
        playerMove.anim.SetBool("Falling", false);
        playerMove.anim.SetBool("InAir", false);
        playerMove.rb.linearVelocity = Vector2.zero;

        playerMove.enabled = false;
    }

    void FreePlayer()
    {
        playerMove.anim.SetBool("Walking", true);
        playerMove.anim.SetBool("Falling", true);
        playerMove.anim.SetBool("InAir", true);

        playerMove.enabled = true;
    }
}
