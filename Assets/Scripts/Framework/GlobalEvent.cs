public delegate void JGameEvent();

public class GlobalEvent
{
    private static GlobalEvent _instance;
    public static GlobalEvent Instance => _instance ??= new GlobalEvent();

    /// <summary>
    /// 玩法正式开始
    /// </summary>
    public JGameEvent OnStart;
    
    /// <summary>
    /// 玩法结束
    /// </summary>
    public JGameEvent OnEnd;
}