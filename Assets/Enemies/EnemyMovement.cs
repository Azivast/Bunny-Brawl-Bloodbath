using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour {
    [SerializeField] private float speed = 1;
    [SerializeField] private float chanceOfNewDir = 0.1f;
    private Rigidbody2D rb;
    private Random rand;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        rand = new Random();
    }

    private void MoveRandom() {
        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle;
        rb.velocity = randomDirection * speed;
    }

    private void FixedUpdate() {

        if (rand.NextDouble() > chanceOfNewDir) {
            MoveRandom();
        }
    }
}
