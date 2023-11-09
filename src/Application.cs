
namespace ProtoEngine;

using ComputeSharp;
using ProtoEngine.Rendering;
using ProtoEngine.UI;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

public class Application
{
    public Window window;
    public Camera camera;
    public Loop drawLoop = new(60);
    public Loop updateLoop = new(60);
    public Loop fixedUpdateLoop = new(60);

    public static Application? currentApplication = null;

    private static readonly List<Action> mainThreadQueue = new();
    private static bool processThreadQueue = true;
    public static GraphicsDevice GPU = GraphicsDevice.GetDefault();
    public static Random random = new();

    private Stopwatch startTimer = Stopwatch.StartNew();
    public float Time => (float)startTimer.Elapsed.TotalSeconds;

    public void RunOnMainThread(Action action)
    {
        mainThreadQueue.Add(action);
    }

    public Application(string name, Color windowFill, bool fullscreen, Vector2? size = null)
    {
        drawLoop.RunActionSync(() =>
        {
            camera = new Camera(new Vector2(0, 0), 1);
            window = new Window(fullscreen, name, drawLoop, size)
            {
                ActiveCamera = camera
            };

            window.globalEvents.Closed += (win) =>
            {
                Loop.AbortAll();
                processThreadQueue = false;
            };
            window.fillColor = windowFill;
            window.globalEvents.KeyPressed += (e, win) =>
            {
                if (e.Code == SFML.Window.Keyboard.Key.Escape) window.Close();
            };
        });

        drawLoop.Connect(Draw);
        updateLoop.Connect(Update);
        fixedUpdateLoop.Connect(FixedUpdate);

        Setup();

        currentApplication = this;
    }

    public virtual void Run()
    {
        Loop.RunAll();
        while (processThreadQueue)
        {
            if (mainThreadQueue.Count > 0)
            {
                mainThreadQueue[0]();
                mainThreadQueue.RemoveAt(0);
            }

            Thread.Sleep(1);
        }
    }

    protected virtual void Setup()
    {
    }

    protected virtual void Draw(float dt)
    {
        
    }

    protected virtual void Update(float dt)
    {
    }

    protected virtual void FixedUpdate(float dt)
    {
    }
}










