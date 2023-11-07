using System.Diagnostics;

namespace ProtoEngine;

public static class Debug
{
    public static float TestTiming(Action action, int iterations)
    {
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            action();
        }
        sw.Stop();
        return sw.ElapsedTicks / (float)iterations;
    }

    public static (float time1, float time2, float totalTime1, float totalTime2) CompareTimings(Action action1, Action action2, int iterations)
    {
        float timeTotal1 = 0;
        float timeTotal2 = 0;

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            sw.Restart();
            action1();
            sw.Stop();
            timeTotal1 += sw.ElapsedTicks;

            sw.Restart();
            action2();
            sw.Stop();
            timeTotal2 += sw.ElapsedTicks;
        }

        //convert to milliseconds
        timeTotal1 /= 10000;
        timeTotal2 /= 10000;

        return (timeTotal1 / iterations, timeTotal2 / iterations, timeTotal1, timeTotal2);
    }

    public static (float time1, float time2, float time3, float totalTime1, float totalTime2, float totalTime3) CompareTimings(Action action1, Action action2, Action action3, int iterations)
    {
        float timeTotal1 = 0;
        float timeTotal2 = 0;
        float timeTotal3 = 0;

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            sw.Restart();
            action1();
            sw.Stop();
            timeTotal1 += sw.ElapsedTicks;

            sw.Restart();
            action2();
            sw.Stop();
            timeTotal2 += sw.ElapsedTicks;

            sw.Restart();
            action3();
            sw.Stop();
            timeTotal3 += sw.ElapsedTicks;
        }

        //convert to milliseconds
        timeTotal1 /= 10000;
        timeTotal2 /= 10000;
        timeTotal3 /= 10000;

        return (timeTotal1 / iterations, timeTotal2 / iterations, timeTotal3 / iterations, timeTotal1, timeTotal2, timeTotal3);
    }

    // compare four timings
    public static (float time1, float time2, float time3, float time4, float totalTime1, float totalTime2, float totalTime3, float totalTime4) CompareTimings(Action action1, Action action2, Action action3, Action action4, int iterations)
    {
        float timeTotal1 = 0;
        float timeTotal2 = 0;
        float timeTotal3 = 0;
        float timeTotal4 = 0;

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            sw.Restart();
            action1();
            sw.Stop();
            timeTotal1 += sw.ElapsedTicks;

            sw.Restart();
            action2();
            sw.Stop();
            timeTotal2 += sw.ElapsedTicks;

            sw.Restart();
            action3();
            sw.Stop();
            timeTotal3 += sw.ElapsedTicks;

            sw.Restart();
            action4();
            sw.Stop();
            timeTotal4 += sw.ElapsedTicks;
        }

        //convert to milliseconds
        timeTotal1 /= 10000;
        timeTotal2 /= 10000;
        timeTotal3 /= 10000;
        timeTotal4 /= 10000;

        return (timeTotal1 / iterations, timeTotal2 / iterations, timeTotal3 / iterations, timeTotal4 / iterations, timeTotal1, timeTotal2, timeTotal3, timeTotal4);
    }

    [DebuggerHidden]
    [Conditional("DEBUG")] 
    public static void Break()
    {
        if(Debugger.IsAttached)
            Debugger.Break();
    }
}