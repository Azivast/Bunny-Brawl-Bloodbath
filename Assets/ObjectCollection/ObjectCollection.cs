using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ObjectCollection", menuName = "Bunny Brawl Bloodbath/ObjectCollection")]
public class ObjectCollection : ScriptableObject {
    public UnityAction OnCollectionEmpty = delegate{};
    public UnityAction<Vector2> OnCollectionEmptyGetLastPos = delegate(Vector2 position) {  };
    [SerializeField] private List<GameObject>objects = new List<GameObject>();

    public List<GameObject> GetObjects() {
        return objects;
    }

    public void Register(GameObject o) {
        objects.Add(o);
    }
    
    public void Unregister(GameObject o) {
        objects.Remove(o);
        if (!objects.Any())
        {
            OnCollectionEmpty.Invoke();
            OnCollectionEmptyGetLastPos.Invoke(o.transform.position);
        }
    }
}
