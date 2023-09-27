using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TargetBehaviour))]
public class EnemyHealthHandler : MonoBehaviour {
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private int currentHealth;
    public UnityEvent OnKilled = new UnityEvent();
    private TargetBehaviour target;
    
    void Awake() {
        target = GetComponent<TargetBehaviour>();
        currentHealth = maxHealth;
    }

    private void OnEnable() {
        target.OnAttacked += Hit;
    }

    private void OnDisable() {
        target.OnAttacked -= Hit;
    }

    private void Hit(int damage) {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            Kill();
        }
    }
    
    private void Kill() {
        OnKilled.Invoke();
        Destroy(gameObject);
    }
    
}
