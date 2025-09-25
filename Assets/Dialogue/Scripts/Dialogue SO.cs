using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialogueSO : ScriptableObject
{
    public DialogueNpcs[] npcs;

    [Header("Dialogue")]
    [TextArea]
    public string[] dialogue;


}
