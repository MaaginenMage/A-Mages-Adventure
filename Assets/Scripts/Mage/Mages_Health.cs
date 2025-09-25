using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mages_Health : MonoBehaviour
{
    public Mage mage;
    private Animator anim;
    public int Health;
    public bool Healing;
    private void Start()
    {
        anim = GetComponent<Animator>();
        Health = anim.GetInteger("Health");
        Healing = anim.GetBool("Healing");
    }

    private void Update()
    {
        anim.SetInteger("Health", Health);
        anim.SetBool("Healing", Healing);
        if (Healing == true)
        {
            Health = 7;
        }
    }
}
