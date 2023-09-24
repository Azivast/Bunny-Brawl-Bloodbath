using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EnemyBehaviour))]
public class EnemyHealthHandler : MonoBehaviour {
    [SerializeField]private EnemyBehaviour enemyBehaviour;
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private int currentHealth;
    public UnityEvent OnKilled = new UnityEvent();
    void Start() {
        enemyBehaviour = GetComponent<EnemyBehaviour>();
        currentHealth = maxHealth;
    }

    private void OnEnable() {
        enemyBehaviour.Target.OnAttacked += Hit;
    }

    private void OnDisable() {
        enemyBehaviour.Target.OnAttacked -= Hit;
    }

    private void Hit(TargetBehaviour target, int damage) {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            Kill();
        }
    }
    
    private void Kill() {
        Destroy(gameObject);
        OnKilled.Invoke();
    }
    
}
