using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TargetBehaviour))]
public class EnemyBehaviour : MonoBehaviour {
    public TargetBehaviour Target;
    private Rigidbody2D rigidBody;

    private void OnValidate() {
        if (Target is null)
            Debug.LogError("Target cannot be null");
    }
}