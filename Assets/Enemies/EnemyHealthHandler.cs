using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TargetBehaviour))]
public class EnemyHealthHandler : MonoBehaviour {
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private ObjectCollection enemiesAliveCollection;
    [Tooltip("Material to switch to during the flash.")]
    [SerializeField] private Material flashMaterial;
    private int currentHealth;
    public UnityEvent OnKilled = new UnityEvent();
    public UnityEvent OnHit = new UnityEvent();
    private TargetBehaviour target;
    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    private Coroutine flashRoutine;
    
    void Awake() {
        target = GetComponent<TargetBehaviour>();
        enemiesAliveCollection.Register(gameObject);
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
    }

    private void OnEnable() {
        target.OnAttacked += Hit;
        currentHealth = maxHealth;
    }

    private void OnDisable() {
        target.OnAttacked -= Hit;
        enemiesAliveCollection.Unregister(gameObject);
    }

    private void Hit(int damage) {
        OnHit.Invoke();
        currentHealth -= damage;
        //HitStop.Stop(0.05f);
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }
        
        flashRoutine = StartCoroutine(FlashRoutine(0.1f));
        if (currentHealth <= 0) {
            Kill();
        }
    }
    
    private void Kill() {
        OnKilled.Invoke();
        Destroy(gameObject);
    }
    private IEnumerator FlashRoutine(float duration)
    {
        // Swap to the flashMaterial.
        spriteRenderer.material = flashMaterial;

        // Pause the execution of this function for "duration" seconds.
        yield return new WaitForSeconds(duration);

        // After the pause, swap back to the original material.
        spriteRenderer.material = originalMaterial;

        // Set the routine to null, signaling that it's finished.
        flashRoutine = null;
    }
}
