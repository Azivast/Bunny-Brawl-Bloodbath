using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponHandler : MonoBehaviour {
    [SerializeField] private EquippedWeaponsObject weapons;
    [SerializeField] private Transform weaponPosition;
    [SerializeField] private CameraController camera;
    [SerializeField] private InputActionReference fire;

    private void Start() {
        weaponPosition = Instantiate(activeWeapon, weaponPosition);
    }

    private void OnEnable() {
        fire.action.Enable(); // TODO: is this needed?
        fire.action.performed += Fire;
    }
    
    private void OnDisable() {
        fire.action.Disable(); // TODO: is this needed?
        fire.action.performed -= Fire;
    }

    private void Aim() {
        weapons.GetActiveWeaponBehavior().Target = camera.MouseWorldPosition();
    }

    private void Update() {
        Aim();
    }

    private void ChangeWeapon(int index) {
        weapons.SwitchWeapon();
    }

    private void Fire(InputAction.CallbackContext context) {
        weapons.GetActiveWeaponBehavior().Shoot(); // TODO: Use events and reduce coupling!
    }
}
