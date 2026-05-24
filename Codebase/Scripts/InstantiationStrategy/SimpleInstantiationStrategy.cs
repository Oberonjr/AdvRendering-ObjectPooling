using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Simple Instantiation Strategy", menuName = "Instantiation Strategies/Simple Instantiation Strategy")]
public class SimpleInstantiationStrategy : InstantiationStrategy
{
    public override GameObject Spawn(GameObject prefab, Vector3 position, List<GameObject> objects, Transform parent = null)
    {
        GameObject instantiatedObject = Instantiate(prefab, position, Quaternion.identity);
        objects.Add(instantiatedObject);
        if (parent != null)
        {
            instantiatedObject.transform.SetParent(parent.transform);
        }
        return instantiatedObject;
    }

    public override void Despawn(GameObject obj)
    {
        Destroy(obj);
    }

    public override void DespawnAll(List<GameObject> objects)
    {
        foreach (GameObject obj in objects)
        {
            #if UNITY_EDITOR
            DestroyImmediate(obj);
            #else
            Destroy(obj, 0.1f);
            #endif
        }
        objects.Clear();
    }
}
