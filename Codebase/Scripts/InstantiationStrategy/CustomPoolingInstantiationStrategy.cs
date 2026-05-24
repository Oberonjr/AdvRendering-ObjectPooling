using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pooling Instantiation Strategy", menuName = "Instantiation Strategies/Custom Pooling Instantiation Strategy")]
public class CustomPoolingInstantiationStrategy : InstantiationStrategy
{
    [SerializeField] private float initialSpawnMultiplier = 1;
    Queue<GameObject> availableObjects = new Queue<GameObject>();

    public override void Prewarm(int numberOfPrefabs, GameObject prefab, List<GameObject> objects)
    {
        availableObjects.Clear();
        for (int i = 0; i < numberOfPrefabs * initialSpawnMultiplier; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            obj.TryGetComponent(out Rigidbody rb);
            if(rb != null) rb.useGravity = false;
            objects.Add(obj);
            availableObjects.Enqueue(obj);
        }
    }

    public override GameObject Spawn(GameObject prefab, Vector3 position, List<GameObject> objects, Transform parent = null)
    {
        if (availableObjects.Count <= 0)
        {
            Debug.LogWarning("Pool exhausted - no available objects");
            return null;
        }
        GameObject obj = availableObjects.Dequeue();
        obj.transform.position = position;
        obj.SetActive(true);
        if(parent != null) obj.transform.SetParent(parent);
        return obj;
    }

    public override void Despawn(GameObject obj)
    {
        obj.SetActive(false);
        availableObjects.Enqueue(obj);
    }

    public override void DespawnAll(List<GameObject> objects)
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(false);
            availableObjects.Enqueue(obj);
        }
    }
}
