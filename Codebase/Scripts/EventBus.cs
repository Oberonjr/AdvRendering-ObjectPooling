using System;
using UnityEngine;

public abstract class Event{}
public static class EventBus<T> where T : Event
{
    public static event Action<T> OnEvent;

    public static void Publish(T pEvent)
    {
        OnEvent?.Invoke(pEvent);
    }
}

public class BenchmarkStartingEvent : Event
{
    public InstantiationStrategy strat;
    public int repetition;
    public int objectCount;
    public int runID;
    public string runDateTime;
    public InstantiationType instantiationType;
    public BenchmarkStartingEvent(InstantiationStrategy pStrat, int pRepetition, int pObjectCount, int pRunID, string pRunDateTime, InstantiationType pInstantiationType)
    {
        strat = pStrat;
        repetition = pRepetition;
        objectCount = pObjectCount;
        runID = pRunID;
        runDateTime = pRunDateTime;
        instantiationType = pInstantiationType;
    }
}

public class BenchmarkEndingEvent : Event
{
    public BenchmarkEndingEvent(){}
}