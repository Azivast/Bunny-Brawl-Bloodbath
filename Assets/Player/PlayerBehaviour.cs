using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBehaviour : MonoBehaviour {
    [SerializeField] private InputActionReference move, interract;
    [SerializeField] private int speed;
    private Rigidbody2D rigidbody;
    private Vector2 velocity;


    private void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        rigidbody.velocity = move.action.ReadValue<Vector2>() * speed;
    }
}
