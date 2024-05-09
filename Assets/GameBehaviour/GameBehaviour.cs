using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameBehaviour : MonoBehaviour {
    [SerializeField] private ObjectCollection enemiesAlive;
    [SerializeField] private PlayerHealthObject playerHealth;
    [SerializeField] private EquippedWeaponsObject weapons;
    [SerializeField] private AmmoType[] ammoTypes;
    [SerializeField] private UnityEvent OnAllEnemiesKilled;
    [SerializeField] private UnityEvent OnPlayerDied;

    
    private void Awake() {
        weapons.Reset();
        foreach (var ammo in ammoTypes) {
            ammo.Reset();
        }
    }

    private void Start()
    {
        enemiesAlive.OnCollectionEmpty += LevelComplete;
        playerHealth.OnPlayerDied += GameOver;
        playerHealth.Reset();
    }

    private void OnDisable() {
        enemiesAlive.OnCollectionEmpty -= LevelComplete;
        playerHealth.OnPlayerDied -= GameOver;
    }

    private void GameOver() {
        OnPlayerDied.Invoke();
    }
    
    private void LevelComplete() {
        OnAllEnemiesKilled.Invoke();
    }
}
