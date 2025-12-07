using UnityEngine;
using System.Collections.Generic;

public class SpellUiManager : MonoBehaviour
{
    [SerializeField] private List<SpellSlot> slots = new List<SpellSlot>();

    public void ShowSpells(List<SpellSO> spells)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if(i < spells.Count)
            {
                slots[i].SetSpell(spells[i]);
            }
            else
            {
                slots[i].SetSpell(null);
            }
        }
    }

    private void Update()
    {
        foreach (SpellSlot slot in slots)
        {
            slot.UpdateCooldown();
        }
    }


    public void HighlightSpell(SpellSO active)
    {
        foreach (SpellSlot slot in slots)
        {
            slot.SetHighlight(slot.AssignedSpell == active);
        }
    }
}
