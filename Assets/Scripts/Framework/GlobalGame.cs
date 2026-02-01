using System;
using Framework;
using Framework.Task;
using UnityEngine;

public class GlobalGame : SingletonMono<GlobalGame>
{
    public int CurrentDiffIndex = 0;

    public void Start()
    {
    }

    public void Update()
    {
        var dt = Time.deltaTime;
        
    }
}