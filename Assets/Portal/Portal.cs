using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Portal : MonoBehaviour
{
    [SerializeField] private UnityEvent onPlayerEnter;
    [SerializeField] private float rotationSpeed = 50;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            onPlayerEnter.Invoke();
        }
    }

    private void Update()
    {
        transform.Rotate(-Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
