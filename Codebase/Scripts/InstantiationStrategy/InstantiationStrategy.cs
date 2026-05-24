using System.Collections.Generic;
using UnityEngine;

public class InstantiationStrategy : ScriptableObject, IInstantiateStrategy
{
    public virtual void Prewarm(int numberOfPrefabs, GameObject prefab, List<GameObject> objects)
    {

    }

    public virtual GameObject Spawn(GameObject prefab, Vector3 position, List<GameObject> objects, Transform parent)
    {
        return null;
    }

    public virtual void Despawn(GameObject obj)
    {

    }

    public virtual void DespawnAll(List<GameObject> objects)
    {

    }
}
