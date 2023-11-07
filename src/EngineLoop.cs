using System.Diagnostics;
using System.Collections.Concurrent;
using SFML.Window;

namespace ProtoEngine;

public class Loop
{
    public delegate void LoopEvent(float dt);
    private static readonly List<Loop> loops = new();

    public Thread LoopThread { get; private set; }
    public event LoopEvent? OnLoop;
    private readonly ConcurrentQueue<Action> threadQueue = new();

    private int _targetFPS;
    public int targetFPS 
    {
        get => _targetFPS;
        set
        {
            _targetFPS = value;
            _deltaTime = 1f / value;
        }
    }
    private float _deltaTime;
    public float deltaTime 
    {
        get => _deltaTime;
        set
        {
            _deltaTime = value;
            _targetFPS = (int)(1f / value);
        }
    }

    public float timeScale = 1;

    public float potentialFPS { get; private set; }
    public float measuredFPS { get; private set; }

    public bool running { get; private set; } = false;
    private bool aborted = false;
    private int stepCountdown = 0;

    public Loop(int targetFPS, bool start = false)
    {
        this.targetFPS = targetFPS;
        this.OnLoop = null;

        loops.Add(this);

        LoopThread = new Thread(LoopThreadCall);
        LoopThread.Start();


        
        if(start) Run();
    }

    void LoopThreadCall()
    {
        Console.WriteLine($"Loop initialized");

        #pragma warning disable IDE0059 
        Context context = new Context();

        Stopwatch frameTimer = new Stopwatch();

        while (!aborted)
        {
            while (running)
            {
                frameTimer.Restart();

                ThreadStep();

                frameTimer.Stop();
                
                float frameTime = (float)frameTimer.ElapsedTicks / 10000 / 1000; // Convert ticks to seconds

                frameTimer.Restart();
                if (frameTime > 0)
                    potentialFPS = (potentialFPS * 0.95f) + 1.0f / frameTime * 0.05f;

                float waitTime = Math.Max(0, deltaTime - frameTime);
                Thread.Sleep((int)(waitTime * 900));
                frameTimer.Stop();

                frameTime += (float)frameTimer.ElapsedTicks / 10000 / 1000; // Convert ticks to seconds
                if (frameTime > 0)
                    measuredFPS = (measuredFPS * 0.95f) + 1.0f / frameTime * 0.05f;
            }

            if (stepCountdown > 0)
            {
                ThreadStep();
                stepCountdown--;
            }
            else
            {
                Thread.Sleep(10);
            }
        }

        Console.WriteLine($"Loop finished");
    }

    void ThreadStep()
    {
        while (threadQueue.Count > 0)
        {
            threadQueue.TryDequeue(out Action? action);
            action?.Invoke();
        }
        
        OnLoop?.Invoke(1/measuredFPS * timeScale);
    }

    public void Abort()
    {
        running = false;
        aborted = true;
    }

    public void Pause()
    {
        running = false;
    }

    public void Run()
    {
        running = true;
    }

    public void Step()
    {
        stepCountdown = 1;
    }

    public void Step(int steps)
    {
        if(steps < 0) throw new Exception("Steps must be positive");
        stepCountdown = steps;
    }

    public void Connect(LoopEvent onLoop)
    {
        OnLoop += onLoop;
    }

    public void RunAction(Action action)
    {
        threadQueue.Enqueue(action);
    }

    public void RunActionSync(Action action)
    {
        threadQueue.Enqueue(action);

        Step();

        while (stepCountdown > 0)
        {
            Thread.Sleep(1);
        }
    }

    
    public static void AbortAll()
    {
        foreach (Loop loop in loops)
        {
            loop.Abort();
        }
    }

    public static void RunAll()
    {
        foreach (Loop loop in loops)
        {
            loop.Run();
        }
    }

    public static void StepAll()
    {
        foreach (Loop loop in loops)
        {
            loop.Step();
        }
    }

    public static void StepAll(int steps)
    {
        foreach (Loop loop in loops)
        {
            loop.Step(steps);
        }
    }
}