using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {
    [SerializeField] private int maxhealth = 10;
    [SerializeField] private TargetBehaviour target;

    private Rigidbody2D rigidBody;
    private int currentHealth;

    private void Start() {
        currentHealth = maxhealth;
    }
}
