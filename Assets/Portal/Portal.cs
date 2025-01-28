using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Portal : MonoBehaviour
{
    [SerializeField] private UnityEvent onPlayerEnter;
    [SerializeField] private float rotationSpeed = 50;

    private Transform playerTransform;
    private bool playerFound = false;
    private Vector2 playerVelocity;
    private Vector2 playerScaleVelocity;
    private Camera playerCamera;
    private float playerRotationSpeed = 1;
    private SpriteRenderer playerSprite;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            playerFound = true;
            playerTransform.GetComponent<Rigidbody2D>().simulated = false;
            playerTransform.GetComponent<PlayerBehaviour>().enabled = false;
            playerCamera = playerTransform.GetComponentInChildren<Camera>();
            playerCamera.transform.SetParent(null);
            playerSprite = playerTransform.GetComponent<SpriteRenderer>();
            StartCoroutine(Wait());
        }
    }

    private void Update()
    {
        transform.Rotate(-Vector3.forward, rotationSpeed * Time.deltaTime);
        if (playerFound)
        {
            playerTransform.position = Vector2.SmoothDamp(playerTransform.position, transform.position, ref playerVelocity, 0.1f);
            playerTransform.localScale = Vector2.SmoothDamp(playerTransform.localScale, new Vector2(0.001f, 0.001f), ref playerScaleVelocity, 1f);

            playerTransform.Rotate(-Vector3.forward, rotationSpeed* playerRotationSpeed * Time.deltaTime);
            if (playerRotationSpeed < 30) playerRotationSpeed+= 0.1f;
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(3);
        onPlayerEnter.Invoke();
    }
}
