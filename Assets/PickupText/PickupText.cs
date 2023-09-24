using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickupText : MonoBehaviour
{
    // Maybe not events? Just a static function that can be called to spawn a text?
    public UnityAction<string> OnNewText = delegate{};

    public void SpawnText(string text){
        OnNewText(text);
    }
}
