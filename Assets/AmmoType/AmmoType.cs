using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "AmmoType", menuName = "Bunny Brawl Bloodbath/AmmoType")]
public class AmmoType : ScriptableObject
{
    public UnityAction<int> OnAmountChance = delegate{};
    public UnityAction<int> OnAmountIncrease = delegate{};

    [SerializeField] private int ammoLeft;
    public Image icon;

    public int GetAmmoLeft() {
        return ammoLeft;
    }
    

    public void AddAmmo(int amount) {
        ammoLeft += amount;
        OnAmountChance(ammoLeft);
        OnAmountIncrease(amount);
    }
    
    public void UseAmmo(int amount) {
        ammoLeft -= amount;
        OnAmountChance(ammoLeft);
    }
}
