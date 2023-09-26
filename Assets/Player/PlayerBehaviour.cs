using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBehaviour : MonoBehaviour {
    [SerializeField] private InputActionReference move, interract;
    [SerializeField] private int speed;
    [SerializeField] private PlayerHealthObject health;
    
    private Rigidbody2D rigidbody;
    private Vector2 velocity;
    private TargetBehaviour targetBehaviour;
    
    private void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
        targetBehaviour = GetComponent<TargetBehaviour>();
    }

    private void OnEnable() {
        targetBehaviour.OnAttacked += TakeDamage;
    }

    private void OnDisable() {
        targetBehaviour.OnAttacked -= TakeDamage;
    }

    private void TakeDamage(int amount) {
        health.Damage(amount);
    }

    private void FixedUpdate() {
        rigidbody.velocity = move.action.ReadValue<Vector2>() * speed;
    }
}
