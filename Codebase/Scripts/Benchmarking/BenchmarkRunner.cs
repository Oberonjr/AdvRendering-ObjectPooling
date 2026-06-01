using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Profiling;

public class BenchmarkRunner : MonoBehaviour
{
    private InstantiationStrategy strat;
    private int obj;
    private int repetition;
    private int runID;
    private string runDateTime;
    private InstantiationType instType;
    private long totalGCAlloc;
    private List<float> frameTimes = new List<float>();
    private bool isRunning;
    private bool shouldStart;
    private ProfilerRecorder profilerRecorder;

    public void Awake()
    {
        EventBus<BenchmarkStartingEvent>.OnEvent += StartBenchmark;
        EventBus<BenchmarkEndingEvent>.OnEvent += StopBenchmark;
    }

    public void StartBenchmark(BenchmarkStartingEvent e)
    {
        shouldStart = true;
        if(frameTimes == null) frameTimes = new List<float>();
        frameTimes.Clear();
        totalGCAlloc = 0;
        GC.Collect();
        profilerRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Allocated In Frame");
        strat = e.strat;
        obj = e.objectCount;
        repetition = e.repetition;
        runID = e.runID;
        runDateTime = e.runDateTime;
        instType = e.instantiationType;
    }

    public void StopBenchmark(BenchmarkEndingEvent e)
    {
        if(!Application.isPlaying) return;
        isRunning = false;
        profilerRecorder.Dispose();
        if (frameTimes.Count == 0)
        {
            return;
        }
        float avg = frameTimes.Average();
        float max = frameTimes.Max();
        float min = frameTimes.Min();
        float sumSquareDiffs = 0;
        foreach (var frameTime in frameTimes) sumSquareDiffs += Mathf.Pow(frameTime - avg, 2);
        float stdDev = Mathf.Sqrt(sumSquareDiffs / frameTimes.Count);
        WriteCSV(strat, obj, repetition, frameTimes.Count, avg, max, min, stdDev, totalGCAlloc);

    }

    public void WriteCSV(InstantiationStrategy strat, int objectCount, int repetition, float frameCount, float avgFrameTimeMs, float maxTimeMs, float  minFloatTimeMs, float stdDevFrameTimeMs, long GCAllocBytes)
    {
        string dir = Application.dataPath + "/../Benchmarks";
        string path = dir + "/results.csv";
        
        System.IO.Directory.CreateDirectory(dir);
        bool isNew = !System.IO.File.Exists(path);
        if (isNew)
        {
            System.IO.File.WriteAllText(path, "RunID,RunDateTime,InstantiationType,Strategy,ObjectCount,Repetition,FrameCount,AvgFrameTimeMs,MaxFrameTimeMs,MinFrameTimeMs,StdDevFrameTimeMs,GCAllocBytes\n");
        }

        string row = $"{runID},{runDateTime},{instType},{strat.name},{objectCount},{repetition},{frameCount},{avgFrameTimeMs},{maxTimeMs},{minFloatTimeMs},{stdDevFrameTimeMs},{GCAllocBytes}\n";
        System.IO.File.AppendAllText(path, row);
    }

    public void Update()
    {
        if (shouldStart)
        {
            isRunning = true;
            shouldStart = false;
        }
        if (isRunning)
        {
            frameTimes.Add(Time.unscaledDeltaTime * 1000f);
            totalGCAlloc += profilerRecorder.LastValue;
        }
    }
}
