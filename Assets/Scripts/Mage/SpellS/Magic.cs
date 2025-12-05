using UnityEngine;
using System.Collections.Generic;

public class Magic : MonoBehaviour
{
    public Mage player;
    public SpellUiManager spellUiManager;

    public List<SpellSO> availableSpells = new List<SpellSO>();
    [SerializeField] private int currentIndex = 0;
    public SpellSO currentSpell => availableSpells.Count > 0 ? availableSpells[currentIndex] : null;

    public bool canCast => Time.time > nextCast;
    private float nextCast;

    private void Start()
    {
        spellUiManager.ShowSpells(availableSpells);
        HighlightCurrentSpell();
    }

    public void LearnSpell(SpellSO spell)
    {
        if (!availableSpells.Contains(spell))
        {
            availableSpells.Add(spell);
        }
        currentIndex = Mathf.Clamp(currentIndex, 0, availableSpells.Count - 1);

        spellUiManager.ShowSpells(availableSpells);

        if (availableSpells.Count > 0)
        {
            HighlightCurrentSpell();
        }
    }

    public void NextSpell()
    {
        if(availableSpells.Count == 0)
        {
            return;
        }

        currentIndex = (currentIndex + 1) % availableSpells.Count;
        HighlightCurrentSpell();
    }

    public void PreviousSpell()
    {
        if (availableSpells.Count == 0)
        {
            return;
        }

        currentIndex = (currentIndex - 1 + availableSpells.Count) % availableSpells.Count;
        HighlightCurrentSpell();
    }

    private void HighlightCurrentSpell()
    {
        if(currentSpell != null)
        {
            spellUiManager.HighlightSpell(currentSpell);
        }
    }

    public void SpellAnimationFinish()
    {
        player.SpellAnimationFinish();
        CastSpell();
    }

    private void CastSpell()
    {
        if (!canCast || currentSpell == null)
        {
            return;
        }
            currentSpell.Cast(player); 

        nextCast = Time.time + currentSpell.cooldown;
    }
}
