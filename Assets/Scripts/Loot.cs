using UnityEngine;

public class Loot : MonoBehaviour
{
    private Mage player;
    [SerializeField] private CollectibleSO collectibleSO;
    public Animator anim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.GetComponent<Mage>();

        if (player == null)
        {
            return;
        }

        CollectItem();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = null;
        }
    }

    private void CollectItem()
    {
        anim.Play("Collect");
        collectibleSO.Collect(player);
        Destroy(gameObject, 1);
    }
}
