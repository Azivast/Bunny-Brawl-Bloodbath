using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameBehaviour : MonoBehaviour {
    [SerializeField] private ObjectCollection enemiesAlive;
    [SerializeField] private PlayerHealthObject playerHealth;
    [SerializeField] private UnityEvent OnAllEnemiesKilled;
    [SerializeField] private UnityEvent OnPlayerDied;

    private void OnEnable() {
        enemiesAlive.OnCollectionEmpty += LevelComplete;
        playerHealth.OnPlayerDied += GameOver;
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
