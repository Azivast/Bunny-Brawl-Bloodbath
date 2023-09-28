using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponHandler : MonoBehaviour {
    [SerializeField] private EquippedWeaponsObject weapons;
    [SerializeField] private Transform weaponPosition;
    [SerializeField] private CameraController camera;
    [SerializeField] private InputActionReference fire, switchWeapon;
    
    private GameObject activeWeapon;
    private WeaponBehaviour activeWeaponBehaviour;
    private int bulletLayer = 8;

    private void Start() {
        ChangeToWeapon(0);
    }

    private void OnEnable() {
        fire.action.Enable();
        switchWeapon.action.Enable();
        fire.action.performed += OnFire;
        switchWeapon.action.performed += CycleWeapon;
        weapons.OnWeaponEquipped += OnWeaponEquipped;
    }
    
    private void OnDisable() {
        fire.action.Disable();
        fire.action.performed -= OnFire;
        switchWeapon.action.Disable();
        switchWeapon.action.performed -= CycleWeapon;
        weapons.OnWeaponEquipped -= OnWeaponEquipped;
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

    private void OnWeaponEquipped(GameObject _) {
        ChangeToWeapon(weapons.ActiveWeaponIndex);
    }

    private void OnFire(InputAction.CallbackContext context) {
        activeWeaponBehaviour.Shoot(bulletLayer);
    }
    
    private void CycleWeapon(InputAction.CallbackContext context) {
        weapons.ActiveWeaponIndex++;
        if (weapons.ActiveWeaponIndex > weapons.List.Length - 1) weapons.ActiveWeaponIndex = 0;
        
        ChangeToWeapon(weapons.ActiveWeaponIndex);
    }
}
