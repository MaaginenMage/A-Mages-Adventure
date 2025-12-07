using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpellSlot : MonoBehaviour
{
    public Image iconImg;
    public GameObject highlight;

    public SpellSO AssignedSpell {  get; private set; }

    [SerializeField] private Color normalColor;
    [SerializeField] private Color highlightColor = Color.white;
    [SerializeField] private Color cooldownColor = Color.black;
    private Vector3 normalScale = Vector3.one;
    private Vector3 highlightScale = Vector3.one * 1.2f;

    private float cooldownTimer = 10f;

    public void SetSpell(SpellSO spellSO)
    {
        AssignedSpell = spellSO;

        if (spellSO != null)
        {
            iconImg.sprite = spellSO.icon;
            iconImg.gameObject.SetActive(true);
        } else
        {
            AssignedSpell = null;
            iconImg.sprite = null;
            iconImg.gameObject.SetActive(false);
        }

            SetHighlight(false);
    }


    public void SetHighlight(bool active)
    {
        highlight.SetActive(active);

        iconImg.color = active ? highlightColor : normalColor;
        iconImg.rectTransform.localScale = active ? highlightScale : normalScale;
    }

    public void StartCooldown(float duration)
    {
        cooldownTimer = duration;
        iconImg.color = cooldownColor;
    }

    public void UpdateCooldown()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                cooldownTimer = 0f;
                iconImg.color = normalColor;
            }
        }
    }
}
