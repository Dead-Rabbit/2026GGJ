using Sirenix.OdinInspector;
using UnityEngine;

public class GlobalConfig : MonoBehaviour
{
    [LabelText("玩家本体")] public MainPlayer MainPlayer;
    [LabelText("测试Pate")] public PateInstance TestPate;
    
    private static GlobalConfig _instance;
    public static GlobalConfig Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }
}