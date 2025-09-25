using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Abilities : MonoBehaviour
{
    public Mage mage;
    public Image SkillOne;
    public Image SkillTwo;
    public Image SkillThree;
    public TextMeshProUGUI SkillText1;
    public TextMeshProUGUI SkillText2;
    public TextMeshProUGUI SkillText3;

    public bool StarUnlocked = false;
    public bool DashUnlocked = false;
    public bool UltUnlocked = false;

    private void Update()
    {
        if (StarUnlocked == true)
        {
            SkillOne.enabled = true;
            SkillText1.enabled = true;
            if (mage.AbletoShoot == false)
            {
                SkillOne.color = Color.gray;
            } else
            {
                SkillOne.color = Color.white;
            }
        }
        else
        {
            SkillOne.enabled = false;
            SkillText1.enabled = false; 
        }
        
        if (DashUnlocked == true)
        {
            SkillTwo.enabled = true;
            SkillText2.enabled = true;
            if (mage.AbleToDash == false)
            {
                SkillTwo.color = Color.gray;
            } else
            {
                SkillTwo.color = Color.white;
            }
        }
        else
        {
            SkillTwo.enabled = false;
            SkillText2.enabled = false;
        }

        if (UltUnlocked == true)
        {
            SkillThree.enabled = true;
            SkillText3.enabled = true;
            if (mage.AbleToUlt == false)
            {
                SkillThree.color = Color.gray;
            } else
            {
                SkillThree.color = Color.white;
            }
        }
        else
        {
            SkillThree.enabled = false;
            SkillText3.enabled = false;
        }
    }
}
