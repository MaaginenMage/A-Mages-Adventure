using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public Mage mage;

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
            mage.MagesHealth.Health = 1;
            mage.Hit();
        }
        if (gameObject.tag == "D-Spike" && collision.CompareTag(detected))
        {
            mage.MagesHealth.Health = 1;
            mage.Hit();
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