using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class OneTimeDamage : MonoBehaviour
{
    [SerializeField] private int amount = 1;

    private int framesAlive = 0;
    
    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.TryGetComponent(out TargetBehaviour target)) {
            target.Attack(amount);
            Destroy(this);
        }
    }

    private void FixedUpdate() {
        if (framesAlive++ > 0) Destroy(gameObject);
    }
}
