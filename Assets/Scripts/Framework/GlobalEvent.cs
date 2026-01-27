using UnityEngine;

public delegate void JGameEvent();

public class GlobalEvent
{
    private static GlobalEvent _instance;
    public static GlobalEvent Instance => _instance ??= new GlobalEvent();

    public JGameEvent OnStart;
}