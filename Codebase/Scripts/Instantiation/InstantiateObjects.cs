using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class InstantiateObjects : MonoBehaviour
{
    [SerializeField] protected InstantiationType instantiationType;
    [SerializeField] protected GameObject instantiationPrefab;
    [SerializeField] protected int repetitionAmount;
    [SerializeField] protected Transform centerPosition;
    [SerializeField] protected int instantiationDistance;
    [SerializeField] protected Camera camera;
    [SerializeField] protected InstantiationStrategy instantiationStrategy;
    [SerializeField] protected StrategiesList instantiationStrategyList;
    [SerializeField] protected int benchmarkRepetitions;
    protected List<GameObject> instantiatedObjects = new List<GameObject>();
    private static int runID = 0;

    protected virtual void Start()
    {
        if(camera == null) camera = Camera.main;
        instantiationStrategy.Prewarm(repetitionAmount, instantiationPrefab, instantiatedObjects);
    }

    public abstract void Spawn(InstantiationStrategy strat = null);

    public virtual void DestroyPrefabs()
    {
        instantiationStrategy.DespawnAll(instantiatedObjects);
    }

    public void BenchmarkAllStrategies()
    {
        runID++;
        string runDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        for (int i = 0; i < benchmarkRepetitions; i++)
        {
            foreach (var strategy in instantiationStrategyList.instantiationStrategies)
            {
                EventBus<BenchmarkStartingEvent>.Publish(new BenchmarkStartingEvent(strategy, i+1, repetitionAmount, runID, runDateTime, instantiationType));
                Spawn(strategy);
                EventBus<BenchmarkEndingEvent>.Publish(new BenchmarkEndingEvent());
                strategy.DespawnAll(instantiatedObjects);
            }
        }
        
    }

    public void HardDestroyPrefabs()
    {
        for (int i = centerPosition.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(centerPosition.GetChild(i).gameObject);
        }
        instantiatedObjects.Clear();
    }
}

public enum InstantiationType
{
    WALL,
    FALLING
}