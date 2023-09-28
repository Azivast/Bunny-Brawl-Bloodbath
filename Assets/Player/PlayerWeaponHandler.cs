using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponHandler : MonoBehaviour {
    [SerializeField] private EquippedWeaponsObject weapons;
    [SerializeField] private Transform weaponPosition;
    [SerializeField] private CameraController camera;
    [SerializeField] private InputActionReference fire, switchWeapon;
    
    private int activeWeaponIndex = 0;
    private GameObject activeWeapon;
    private WeaponBehaviour activeWeaponBehaviour;

    private void Start() {
        ChangeToWeapon(0);
    }

    private void OnEnable() {
        fire.action.Enable();
        switchWeapon.action.Enable();
        fire.action.performed += OnFire;
        switchWeapon.action.performed += CycleWeapon;
    }
    
    private void OnDisable() {
        fire.action.Disable();
        fire.action.performed -= OnFire;
        switchWeapon.action.Disable();
        switchWeapon.action.performed -= CycleWeapon;
    }

    private void Aim() {
        activeWeaponBehaviour.Target = camera.MouseWorldPosition();
    }

    private void Update() {
        Aim();
    }

    private void ChangeToWeapon(int index) {
        if (weaponPosition.childCount > 0) {
            Destroy(weaponPosition.GetChild(0).gameObject);
        }

        activeWeapon = Instantiate(weapons.List[index], weaponPosition);
        activeWeaponBehaviour = activeWeapon.GetComponent<WeaponBehaviour>();
    }

    private void OnFire(InputAction.CallbackContext context) {
        activeWeaponBehaviour.Shoot();
    }
    
    private void CycleWeapon(InputAction.CallbackContext context) {
        activeWeaponIndex++;
        if (activeWeaponIndex > weapons.List.Length - 1) activeWeaponIndex = 0;
        
        ChangeToWeapon(activeWeaponIndex);
    }
}
