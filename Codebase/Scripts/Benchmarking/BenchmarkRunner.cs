using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BenchmarkRunner : MonoBehaviour
{
    private InstantiationStrategy strat;
    private int obj;
    private int repetition;
    private int runID;
    private string runDateTime;
    private InstantiationType instType;
    private Stopwatch sw;
    private long memoryBeginning;
    private long memoryEnding;


    public void Awake()
    {
        EventBus<BenchmarkStartingEvent>.OnEvent += StartBenchmark;
        EventBus<BenchmarkEndingEvent>.OnEvent += StopBenchmark;
    }

    public void StartBenchmark(BenchmarkStartingEvent e)
    {
        System.GC.Collect();
        memoryBeginning = GC.GetTotalMemory(false);
        sw = Stopwatch.StartNew();
        strat = e.strat;
        obj = e.objectCount;
        repetition = e.repetition;
        runID = e.runID;
        runDateTime = e.runDateTime;
    }

    public void StopBenchmark(BenchmarkEndingEvent e)
    {
        sw.Stop();
        memoryEnding = GC.GetTotalMemory(false);
        WriteCSV(strat, obj, repetition, sw.ElapsedMilliseconds, memoryEnding - memoryBeginning);
    }

    public void WriteCSV(InstantiationStrategy strat, int objectCount, int repetition, long instantiationTime, long GCAllocBytes)
    {
        string dir = Application.dataPath + "/../Benchmarks";
        string path = dir + "/results.csv";
        
        System.IO.Directory.CreateDirectory(dir);
        bool isNew = !System.IO.File.Exists(path);
        if (isNew)
        {
            System.IO.File.WriteAllText(path, "RunID,RunDateTime,InstantiationType,Strategy,ObjectCount,Repetition,InstantiationTime,GCAllocBytes\n");
        }

        string row = $"{runID},{runDateTime},{instType},{strat.name},{objectCount},{repetition},{instantiationTime},{GCAllocBytes}\n";
        System.IO.File.AppendAllText(path, row);
    }
}
