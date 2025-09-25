using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private DialogueSO currentConversation;
    private int section;
    private bool dialogueActivated;

    private GameObject speechBox;
    private TMP_Text npc;
    private Image icon;
    private TMP_Text dialogueText;

    private GameObject topBar;
    private GameObject bottomBar;
    private GameObject healthBar;
    private GameObject watch;
    private GameObject abilities;

    private string currentSpeaker;
    private Sprite currentIcon;

    public NpcSO[] npcSO;

    private Mage playerMove;

    [SerializeField]
    private float typingSpeed = 0.02f;
    private Coroutine typeWriterRoutine;
    private bool canContinueText = true;

    // Start is called before the first frame update
    void Start()
    {
        speechBox = GameObject.Find("Dialogue");
        npc = GameObject.Find("NpcName").GetComponent<TMP_Text>();
        icon = GameObject.Find("Icon").GetComponent<Image>();
        dialogueText = GameObject.Find("DialogueText").GetComponent<TMP_Text>();
        topBar = GameObject.Find("TopBar");
        bottomBar = GameObject.Find("BottomBar");
        healthBar = GameObject.Find("Healthbar");
        watch = GameObject.Find("Watch");
        abilities = GameObject.Find("Abilities");

        playerMove = GameObject.Find("Mage").GetComponent <Mage>();

        topBar.SetActive(false);
        bottomBar.SetActive(false);
        speechBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(dialogueActivated && Input.GetKeyDown(KeyCode.F) && canContinueText)
        {
            StopPlayer();
            if (section >= currentConversation.npcs.Length)
                TurnOffDialogue();
            else
                PlayDialogue();
        }
    }

    void StopPlayer()
    {
        playerMove.anim.SetBool("Walking", false);
        playerMove.anim.SetBool("Falling", false);
        playerMove.anim.SetBool("InAir", false);
        playerMove.rb.velocity = Vector2.zero;

        playerMove.enabled = false;
    }

    void FreePlayer()
    {
        playerMove.anim.SetBool("Walking", true);
        playerMove.anim.SetBool("Falling", true);
        playerMove.anim.SetBool("InAir", true);

        playerMove.enabled = true;
    }

    void PlayDialogue()
    {
        SetNpcInfo();

        npc.text = currentSpeaker;
        icon.sprite = currentIcon;

        if (typeWriterRoutine != null)
            StopCoroutine(typeWriterRoutine);
        typeWriterRoutine = StartCoroutine(TypeWriter(dialogueText.text = currentConversation.dialogue[section]));
        
        topBar.SetActive(true);
        bottomBar.SetActive(true);
        speechBox.SetActive(true);
        healthBar.SetActive(false);
        watch.SetActive(false);
        abilities.SetActive(false);

        section += 1;
    }

    void SetNpcInfo()
    {
        for (int i = 0; i < npcSO.Length; i++)
        {
            if (npcSO[i].name == currentConversation.npcs[section].ToString())
            {
                currentSpeaker = npcSO[i].npcName;
                currentIcon = npcSO[i].npcIcon;
            }
        }
    }

    private IEnumerator TypeWriter(string line)
    {
        dialogueText.text = "";
        canContinueText = false;
        yield return new WaitForSeconds(0.1f);
        foreach (char letter in line.ToCharArray())
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                dialogueText.text = line;
                break;
            }
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        canContinueText = true;
    }

    public void InitiateDialogue(Npc_Speak npcDialogue)
    {
        currentConversation = npcDialogue.conversation[0];

        dialogueActivated = true;
    }

    public void TurnOffDialogue()
    {
        section = 0;

        healthBar.SetActive(true);
        watch.SetActive(true);
        abilities.SetActive(true);
        topBar.SetActive(false);
        bottomBar.SetActive(false);
        dialogueActivated = false;
        speechBox.SetActive(false);

        playerMove.enabled = true;
    }
}

public enum DialogueNpcs
{
    Mystica,
    Fisher
};
