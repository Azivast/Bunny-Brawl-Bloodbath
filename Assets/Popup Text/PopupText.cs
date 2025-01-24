using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupText : MonoBehaviour
{
    [SerializeField] private float speed = 0.3f;
    [SerializeField] private float lifetime = 3f;
    private float timer;
    private Vector3 direction = Vector2.up;
    [SerializeField]  private TMP_Text text;
    public string Text
    {
        get{ return text.text; }
        set{ text.text = value; }
    }

    private void Awake()
    {
        timer = 0;
    }

    private void Update()
    {
        transform.position += direction*speed*Time.deltaTime;
        timer += Time.deltaTime;
        if (timer > lifetime) Destroy(gameObject);
    }

}
