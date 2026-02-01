using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GlobalConfig : MonoBehaviour
{
    [LabelText("测试Pate")] public PateInstance TestPate;

    [LabelText("游戏时长")] public float GameDuringTime = 120;
    
    /// <summary>
    /// 数值
    /// </summary>
    [LabelText("怀疑度最大值")] public float MaxDoubtValue = 1000;
    [LabelText("怀疑度掉落速度")] public float DoubtRevertSpeed = 50;
    
    [LabelText("区域0怀疑度")] public float DoubtArea0Value = 0;
    [LabelText("区域1怀疑度")] public float DoubtArea1Value = 0;
    [LabelText("区域外怀疑度")] public float OutDoubtAreaValue = 0;

    [LabelText("特殊事件随机开始最小时间")] public float EventStartMinTime = 5;
    [LabelText("特殊事件随机开始最大时间")] public float EventStartMaxTime = 10;
    
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