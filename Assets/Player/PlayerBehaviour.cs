using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBehaviour : MonoBehaviour {
    [SerializeField] private InputActionReference move, look, fire;
    [SerializeField] private int speed;
    private Rigidbody2D rigidbody;
    private Vector2 velocity;


    private void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable() {
        fire.action.Enable(); // TODO: is this needed?
        move.action.Enable();
        fire.action.performed += OnFire;
        //move.action.performed += OnMove;
    }
    
    private void OnDisable() {
        fire.action.Disable(); // TODO: is this needed?
        move.action.Disable();
        fire.action.performed -= OnFire;
        //move.action.performed -= OnMove;
    }

    private void OnFire(InputAction.CallbackContext context) {
        Debug.Log("Fired");
    }
    
    private void OnMove(InputAction.CallbackContext context) {
        Debug.Log("Moved");
        // rigidbody.velocity = context.ReadValue<Vector2>() * speed;
    }

    private void FixedUpdate() {
        rigidbody.velocity = move.action.ReadValue<Vector2>() * speed;
    }
}
