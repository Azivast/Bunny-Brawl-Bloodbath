using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Casing : MonoBehaviour
{
    [SerializeField] LayerMask layersToIgnore;
    [SerializeField] float ejectForce;
    [SerializeField] float ejectTorque;

    private void Start()
    {
        System.Random rand = new();
        var rb = GetComponent<Rigidbody2D>();
        rb.excludeLayers = layersToIgnore;
        rb.AddForce(-transform.up*ejectForce);
        rb.AddTorque(ejectTorque * UnityEngine.Random.value);
    }
}
