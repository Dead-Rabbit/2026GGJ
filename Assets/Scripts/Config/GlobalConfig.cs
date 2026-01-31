using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GlobalConfig : MonoBehaviour
{
    [LabelText("测试Pate")] public PateInstance TestPate;
    
    /// <summary>
    /// 数值
    /// </summary>
    [LabelText("怀疑度最大值")] public float MaxDoubtValue = 1000;
    [LabelText("怀疑度掉落速度")] public float DoubtRevertSpeed = 50;
    
    [LabelText("区域0怀疑度")] public float DoubtArea0Value = 0;
    [LabelText("区域1怀疑度")] public float DoubtArea1Value = 0;
    [LabelText("区域外怀疑度")] public float OutDoubtAreaValue = 0;
    
    [HideInInspector] public Dictionary<int, float> DoubtIncreaseConfig = new();
    
    private static GlobalConfig _instance;
    public static GlobalConfig Instance => _instance;

    private void Awake()
    {
        // 读入权重
        DoubtIncreaseConfig.Add(0, DoubtArea0Value);
        DoubtIncreaseConfig.Add(1, DoubtArea1Value);
        DoubtIncreaseConfig.Add(-1, OutDoubtAreaValue);
        
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }
}