using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;         // Assign Mage here in Inspector
    public float followSpeed = 10f;

    public float normalYOffset = 1.5f;     // Normal camera height offset
    public float lookDownYOffset = 0.5f;   // Lower camera offset when looking down

    private float currentYOffset;

    void Start()
    {
        currentYOffset = normalYOffset;
    }

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("CameraFollow: Target not assigned!");
            return;
        }

        if (Input.GetKey(KeyCode.S))
        {
            Debug.Log("S key held - moving camera down");
            currentYOffset = Mathf.Lerp(currentYOffset, lookDownYOffset, Time.deltaTime * 5f);
        }
        else
        {
            currentYOffset = Mathf.Lerp(currentYOffset, normalYOffset, Time.deltaTime * 5f);
        }

        Vector3 targetPosition = new Vector3(target.position.x, target.position.y + currentYOffset, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
    }
}

