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
    [SerializeField] private GameObject portalPrefab;

    
    private void Awake() {
        weapons.Reset();
        foreach (var ammo in ammoTypes) {
            ammo.Reset();
        }
    }

    private void Start()
    {
        enemiesAlive.OnCollectionEmptyGetLastPos += LevelComplete;
        playerHealth.OnPlayerDied += GameOver;
        playerHealth.Reset();
    }

    private void OnDisable() {
        enemiesAlive.OnCollectionEmptyGetLastPos -= LevelComplete;
        playerHealth.OnPlayerDied -= GameOver;
    }

    private void GameOver() {
        OnPlayerDied.Invoke();
    }
    
    private void LevelComplete(Vector2 position) {
        OnAllEnemiesKilled.Invoke();
        Instantiate(portalPrefab, position, Quaternion.identity);
    }
}
