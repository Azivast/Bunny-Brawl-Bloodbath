using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {
    [SerializeField] private InputActionReference look;
    [SerializeField] private Texture2D crosshairTexture;
    [SerializeField] private float cameraDistance = 2f;
    [SerializeField] private float smoothTime = 0.2f;
    [SerializeField] private Transform targetTransform;
    private Camera cam;
    private float initialZ;
    private Vector3 currentVelocity;

    private void Start() {
        cam = GetComponent<Camera>();

        initialZ = transform.position.z;
        
        Vector2 hotspot = new Vector2(crosshairTexture.width / 2, crosshairTexture.height / 2);
        Cursor.SetCursor(crosshairTexture, hotspot, CursorMode.Auto);
    }

    private void OnEnable() {
        look.action.Enable();
    }
    
    private void OnDisable() {
        look.action.Disable();
    }

    public Vector3 MouseWorldPosition() {
        var mousePosition = look.action.ReadValue<Vector2>();
        var mousePositionZ = cam.farClipPlane * .5f;
        
        return cam.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mousePositionZ));
    }
    
    public Vector3 MouseViewPosition() {
        var mousePosition = cam.ScreenToViewportPoint(look.action.ReadValue<Vector2>());
        mousePosition -= Vector3.one * 0.5f; // 0,0 as middle of screen
        mousePosition *= 2; // -1 to 1

        return mousePosition;
    }
    
    private void UpdateCameraPosition() {
        var offset = MouseViewPosition() * cameraDistance;
        var target = targetTransform.position + offset;
        target.z = initialZ;

        transform.position = Vector3.SmoothDamp(transform.position, target, ref currentVelocity, smoothTime) + CameraShake.ShakeOffset;
    }
    
    private void Update() {
        UpdateCameraPosition();
    }
}