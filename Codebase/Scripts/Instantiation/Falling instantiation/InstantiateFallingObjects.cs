using System.Collections.Generic;
using UnityEngine;

public class InstantiateFallingObjects : InstantiateObjects
{
    private DespawnArea despawnArea;
    private int repetitionCounter = 0;
    
    protected override void Start()
    {
        base.Start();
        despawnArea = DespawnArea.Instance;
        despawnArea.ObjectCollision += DespawnObject;
        
    }

    public override void Spawn(InstantiationStrategy strat)
    {
        InstantiationStrategy pStrat = instantiationStrategy;
        if(strat != null) pStrat = strat;
        
        if(instantiatedObjects == null) instantiatedObjects = new List<GameObject>();
        if (instantiatedObjects.Count <= 0)
        {
            pStrat.Prewarm(repetitionAmount, instantiationPrefab, instantiatedObjects);
        }
        float frustumHeight = 2 * instantiationDistance * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float frustumWidth = frustumHeight * camera.aspect;
        float cellSizeX = instantiationPrefab.transform.localScale.x * 1.05f;
        float cellSizeY = instantiationPrefab.transform.localScale.y * 1.05f;
        int maxCols = Mathf.FloorToInt(frustumWidth / cellSizeX);
        float originX = -(maxCols * cellSizeX) / 2 + cellSizeX / 2;
        float originY = frustumHeight / 2 - cellSizeY / 2;
        float originZ = instantiationDistance;

        Vector3 localPosition = new Vector3();
        while(repetitionCounter < repetitionAmount)
        {
            int currentCol = repetitionCounter % maxCols;
            int currentRow = Mathf.FloorToInt(repetitionCounter / maxCols);
            localPosition.x = originX + currentCol * cellSizeX;
            localPosition.y = originY - currentRow * cellSizeY;
            localPosition.z = originZ;

            Vector3 worldPosition = camera.transform.position + camera.transform.right * localPosition.x + camera.transform.up * localPosition.y + camera.transform.forward * localPosition.z;
            GameObject obj = pStrat.Spawn(instantiationPrefab, worldPosition, instantiatedObjects, centerPosition);
            if (obj != null)
            {
                obj.TryGetComponent(out Rigidbody rb);
                if (rb != null) rb.useGravity = true;
            }
            repetitionCounter++;
        }
        repetitionCounter = 0;
        
    }

    private void DespawnObject(GameObject obj)
    {
        obj.TryGetComponent(out Rigidbody rb);
        if (rb != null)
        {
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
        }
        instantiationStrategy.Despawn(obj);
    }
}
