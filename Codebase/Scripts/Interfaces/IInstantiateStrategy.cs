using System.Collections.Generic;
using UnityEngine;

public interface IInstantiateStrategy
{
    void Prewarm(int numberOfPrefabs, GameObject prefab, List<GameObject> objects);
    GameObject Spawn(GameObject prefab, Vector3 position, List<GameObject> objects, Transform parent);
    void Despawn(GameObject obj);
    void DespawnAll(List<GameObject> objects);
}
