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
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            TaskManager?.StartTask("Burn1");
        }
    }
}