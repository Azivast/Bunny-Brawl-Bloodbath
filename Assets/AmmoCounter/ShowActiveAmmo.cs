using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowActiveAmmo : MonoBehaviour
{
    [SerializeField] private EquippedWeaponsObject weapons;
    [SerializeField] private Image PistolAmmoBar;
    [SerializeField] private Image RifelAmmoBar;
    [SerializeField] private Image ShotGunAmmoBar;

    private Image currentSprite;

    private void Start()
    {
        RifelAmmoBar.color = Color.black;
        PistolAmmoBar.color = Color.black;
        ShotGunAmmoBar.color = Color.black;
        OnChangeWeapon();
    }

    public void OnChangeWeapon()
    {
        if(currentSprite != null)currentSprite.color = Color.black;
        if (weapons.GetEquipedWeapon().WeaponName == "Assault Rifle")
        {
            RifelAmmoBar.color = Color.white;
            currentSprite = RifelAmmoBar;
        }
        else if (weapons.GetEquipedWeapon().WeaponName == "Pistol")
        {
            PistolAmmoBar.color = Color.white;
            currentSprite = PistolAmmoBar;
        }
        else if (weapons.GetEquipedWeapon().WeaponName == "Shotgun")
        {
            ShotGunAmmoBar.color = Color.white;
            currentSprite = ShotGunAmmoBar;
        }
    }
}
