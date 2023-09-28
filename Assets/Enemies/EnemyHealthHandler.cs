using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TargetBehaviour))]
public class EnemyHealthHandler : MonoBehaviour {
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private ObjectCollection enemiesAliveCollection;
    private int currentHealth;
    public UnityEvent OnKilled = new UnityEvent();
    public UnityEvent OnHit = new UnityEvent();
    private TargetBehaviour target;
    
    void Awake() {
        target = GetComponent<TargetBehaviour>();
        currentHealth = maxHealth;
    }

    private void OnEnable() {
        target.OnAttacked += Hit;
        enemiesAliveCollection.Register(gameObject);
    }

    private void OnDisable() {
        target.OnAttacked -= Hit;
        enemiesAliveCollection.Unregister(gameObject);
    }

    private void Hit(int damage) {
        OnHit.Invoke();
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
