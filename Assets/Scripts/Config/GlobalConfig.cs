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