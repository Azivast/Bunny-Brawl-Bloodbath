using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameBehaviour : MonoBehaviour {
    [SerializeField] private ObjectCollection enemiesAlive;

    private void OnEnable() {
        enemiesAlive.OnCollectionEmpty += GameOver;
    }

    private void OnDisable() {
        enemiesAlive.OnCollectionEmpty -= GameOver;
    }

    private void GameOver() {
        throw new NotImplementedException();
    }
}
