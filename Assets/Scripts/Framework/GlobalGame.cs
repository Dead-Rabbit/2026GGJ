using System;
using Framework;
using Framework.Task;
using UnityEngine;

public class GlobalGame : SingletonMono<GlobalGame>
{
    public TaskManager TaskManager;

    public void Start()
    {
        TaskManager = new TaskManager();
        TaskManager.Init();
    }

    public void Update()
    {
        var dt = Time.deltaTime;
        TaskManager.OnUpdate(dt);
    }
}