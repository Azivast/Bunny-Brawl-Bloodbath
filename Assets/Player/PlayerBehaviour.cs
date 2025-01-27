using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBehaviour : MonoBehaviour {
    public UnityEvent OnHit = new UnityEvent();
    public UnityEvent OnDash = new UnityEvent();
    public UnityEvent OnDeath = new UnityEvent();
    [SerializeField] private InputActionReference move, dash, interact;
    [SerializeField] private int speed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;
    [SerializeField] private PlayerHealthObject health;
    [SerializeField] private InteractHandlerObject interactHandler;
    [Tooltip("Material to switch to during the flash.")]
    [SerializeField] private Material flashMaterial;

    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 velocity;
    private TargetBehaviour targetBehaviour;
    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    private Coroutine flashRoutine;
    private bool dashing;
    private Vector2 dashDirection;
    private float dashTimer, dashCooldownTimer;
    private PlayerWeaponHandler weaponHandler;
    
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        targetBehaviour = GetComponent<TargetBehaviour>();
        weaponHandler = GetComponent<PlayerWeaponHandler>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
    }

    private void OnEnable() {
        targetBehaviour.OnAttacked += TakeDamage;
        interact.action.Enable();
        interact.action.performed += Interact;
        dash.action.Enable();
        dash.action.performed += Dash;
        health.OnPlayerDied += Die;
    }

    private void OnDisable() {
        targetBehaviour.OnAttacked -= TakeDamage;
        interact.action.Disable();
        interact.action.performed -= Interact;
        dash.action.Disable();
        dash.action.performed -= Dash;
        health.OnPlayerDied -= Die;
    }

    private void TakeDamage(int amount) {
        health.Damage(amount);
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }
        
        flashRoutine = StartCoroutine(FlashRoutine(0.1f));
        OnHit.Invoke();
    }

    private void Interact(InputAction.CallbackContext context) {
        interactHandler.Interact();
    }

    private void Dash(InputAction.CallbackContext context)
    {
        if (dashCooldownTimer <= 0)
        {
            dashCooldownTimer = dashCooldown;
            dashing = true;
            dashDirection = move.action.ReadValue<Vector2>();
            OnDash.Invoke();
        }
    }

    private void Die()
    {
        OnDeath.Invoke();
        weaponHandler.enabled = false;
        this.enabled = false;
    }

    private void FixedUpdate() {
        if (dashing)
        {
            dashTimer += Time.fixedDeltaTime;

            if (dashTimer >= dashDuration)
            {
                dashing = false;
                dashTimer = 0;
                rb.velocity = move.action.ReadValue<Vector2>() * speed;
            }

            else
            {
                rb.velocity = dashDirection * dashSpeed;
            }
        }
        else
        {
            if (dashCooldownTimer > 0) dashCooldownTimer -= Time.deltaTime;

            rb.velocity = move.action.ReadValue<Vector2>() * speed;
        }
        anim.SetFloat("Velocity", rb.velocity != Vector2.zero ? 0.1f : 0.0f);
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
