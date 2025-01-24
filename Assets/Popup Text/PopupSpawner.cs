using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PopupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject textPrefab;

    public void SpawnText(string text)
    {   
        var newText = Instantiate(textPrefab, transform.position, Quaternion.identity, null);
        newText.GetComponent<PopupText>().Text = text;
    }
}
