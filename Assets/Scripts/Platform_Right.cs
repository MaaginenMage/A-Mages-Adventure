using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Right : MonoBehaviour
{
    public Detector detect;
    private Rigidbody2D rb;
    public Mage mage;

    public float MoveSpeed = 5f;
    public float MoveDir = 1f;

    private void Update()
    {
        if (mage.transform.position.x < 135)
        {
            transform.position = new Vector3(145, 10, 0);
        }

        if (detect.InArea == true)
        {
            if (transform.position.y > -11)
            {
                transform.position += new Vector3(0, MoveDir * MoveSpeed * -1) * Time.deltaTime;
            }
            if (transform.position.y <= -11 && transform.position.x <= 251)
            {
                transform.position += new Vector3(MoveDir * MoveSpeed * 3, 0) * Time.deltaTime;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.SetParent(transform);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
    }
}
