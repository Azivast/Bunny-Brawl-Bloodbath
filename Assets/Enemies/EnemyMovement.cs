using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour {
    [SerializeField] private float speed = 1;
    [SerializeField] private int chanceOfNewDir = 50;
    [SerializeField] private float newMoveTime = 2;
    private Rigidbody2D rb;
    private Random rand;
    private float moveTimer;


    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        rand = new Random();
    }

    private void MoveRandom() {
        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle;
        rb.velocity = randomDirection * speed;
    }

    private void FixedUpdate() {
        moveTimer -= Time.fixedDeltaTime;

        if (moveTimer <= 0) {
            if (rand.Next(100) > chanceOfNewDir) {
                moveTimer = newMoveTime;
                MoveRandom();
            }
        }

    }
}
