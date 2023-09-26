using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "AmmoType", menuName = "AmmoType")]
public class AmmoType : ScriptableObject
{
    public UnityAction<int> OnAmountChance = delegate{};
    public UnityAction<int> OnAmountIncrease = delegate{};

    public int Amount;
    public Image icon;

    public void AddAmmo(int amount) {
        Amount += amount;
        OnAmountChance(Amount);
        OnAmountIncrease(amount);
    }
    
    public void UseAmmo(int amount) {
        Amount -= amount;
        OnAmountChance(Amount);
    }
}
