using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

[CreateAssetMenu(fileName = "LeanPool Instantiation Strategy", menuName = "Instantiation Strategies/Lean Pool Instantiation Strategy")]
public class LeanPoolInstantiationStrategy : InstantiationStrategy
{
    public override GameObject Spawn(GameObject prefab, Vector3 position, List<GameObject> objects, Transform parent = null)
    {
        GameObject obj = LeanPool.Spawn(prefab, position, Quaternion.identity);
        objects.Add(obj);
        if(parent != null) obj.transform.SetParent(parent);
        return obj;
    }

    public override void Despawn(GameObject obj)
    {
        LeanPool.Despawn(obj);
    }

    public override void DespawnAll(List<GameObject> objects)
    {
        LeanPool.DespawnAll();
        objects.Clear();
    }
}
