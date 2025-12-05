using UnityEngine;

public class ChaseControl : MonoBehaviour
{
    public Bat[] enemyArray;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (Bat bat in enemyArray)
            {
                bat.chase = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (Bat bat in enemyArray)
            {
                bat.chase = false;
            }
        }
    }
}
