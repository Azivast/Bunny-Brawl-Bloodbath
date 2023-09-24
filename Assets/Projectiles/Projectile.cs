using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour {
    [SerializeField] private int speed = 5;
    [SerializeField] private int damage = 1;

    private Rigidbody2D rb;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.TryGetComponent(out TargetBehaviour target)) {
            target.Attack(damage);
        }
        Destroy(gameObject);
    }
}
