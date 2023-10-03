using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class RabbitMovement : MonoBehaviour {
    [SerializeField] private float speed = 1;
    [SerializeField] private float range = 5;
    private Animator animator;
    private Rigidbody2D rb;
    private float moveTimer;


    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Move(Vector3 target) {
        rb.velocity = (target - transform.position).normalized * speed;
    }
    
    private bool TryFindPlayer(out Vector3 playerPosition) {
        int mask = LayerMask.GetMask("Player");
        Collider2D col = Physics2D.OverlapCircle(transform.position, range, mask);

        if (col == null) {
            playerPosition = Vector3.zero;
            return false;
        }
        else {
            playerPosition = col.transform.position;
            return true;
        }
    }

    private void FixedUpdate() {
        if (TryFindPlayer(out Vector3 target))
        Move(target);
    }
    
    private void Update() {
        animator.SetFloat("Velocity", rb.velocity.magnitude);
    }
}
