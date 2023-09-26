using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetBehaviour : MonoBehaviour
{
    public UnityAction<int> OnAttacked = delegate{};

    public void Attack(int damage){
        OnAttacked(damage);
    }
}
