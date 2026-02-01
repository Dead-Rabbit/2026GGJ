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
    /// 摇铃生效
    /// </summary>
    public JGameEvent OnRattleWorlk;
    
    /// <summary>
    /// 吃饭
    /// </summary>
    public JGameEvent OnEatFood;
    
    /// <summary>
    /// 玩法结束
    /// </summary>
    public JGameEvent OnSuccess;
    public JGameEvent OnFail;
}