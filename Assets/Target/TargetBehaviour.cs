using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetBehaviour : MonoBehaviour
{
    public UnityAction<TargetBehaviour, int> OnAttack = delegate{};

    public void Attack(int damage){
        onAttack(this, damage);
    }
}
