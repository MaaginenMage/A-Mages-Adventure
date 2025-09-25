using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public Mage mage;
    public Abilities abilitys;

    public bool InArea = false;
    private string detected = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(detected))
        {
            InArea = true;
        }
        if (gameObject.tag == "Fall" && collision.CompareTag(detected))
        {
            mage.transform.position = new Vector3(-5, -2.48f, 0);
        }
        if (gameObject.tag == "D-Spike" && collision.CompareTag(detected))
        {
            mage.transform.position = new Vector3(-5, -2.48f, 0);
        }
        if (gameObject.name == "SpellBook-1" && collision.CompareTag(detected))
        {
            abilitys.StarUnlocked = true;
            Destroy(gameObject);
        }
        if (gameObject.name == "SpellBook-2" && collision.CompareTag(detected))
        {
            abilitys.DashUnlocked = true;
            Destroy(gameObject);
        } 
        if (gameObject.name == "SpellBook-3" && collision.CompareTag(detected))
        {
            abilitys.UltUnlocked = true;
            Destroy(gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(detected))
        {
            InArea = false;
        }
    }
}