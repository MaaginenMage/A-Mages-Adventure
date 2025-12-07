using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif

public class Npc_Speak : MonoBehaviour
{
    public DialogueSO[] conversation;
    private Transform player;
    private SpriteRenderer interact;

    private DialogueManager DialogueManager;

    private bool dialogueInitiated;

    private void Start()
    {
        DialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        interact = GetComponent<SpriteRenderer>();
        interact.enabled = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !dialogueInitiated)
        {
            interact.enabled = true;

            player = collision.gameObject.GetComponent<Transform>();

            // turn to player
            if(player.position.x > transform.position.x && transform.parent.localScale.x < 0)
            {
                Flip();
            }
            else if (player.position.x < transform.position.x && transform.parent.localScale.x > 0)
            {
                Flip();
            }

            DialogueManager.InitiateDialogue(this);
            dialogueInitiated = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            interact.enabled = false;

            DialogueManager.TurnOffDialogue();
            dialogueInitiated = false;
        }
    }

    private void Flip()
    {
        Vector3 currentScale = transform.parent.localScale;
        currentScale.x *= -1;
        transform.parent.localScale = currentScale;
    }
}
