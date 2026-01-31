using System;
using Framework;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class CandleInstance : MonoBehaviour
{
    public GameObject Fire;
    
    public bool IsBurning = false;

    [LabelText("燃烧状态最短时间")] public float MinBurnTime;
    [LabelText("燃烧状态最大时间")] public float MaxBurnTime;

    private float _remainBurnTime = 0;

    public void Start()
    {
        GamePlay.Instance.CandleInstanceList.Add(this);
        
        SetBurn(IsBurning);
    }

    /// <summary>
    /// 设置是否点燃中
    /// </summary>
    /// <param name="isBurning"></param>
    public void SetBurn(bool isBurning)
    {
        IsBurning = isBurning;
        
        Fire?.SetActive(isBurning);

        if (isBurning)
        {
            _remainBurnTime = Random.Range(MinBurnTime, MaxBurnTime);
        }
    }

    public void Update()
    {
        if (!IsBurning)
            return;

        var dt = Time.deltaTime;
        if (_remainBurnTime > 0)
        {
            _remainBurnTime -= dt;
            return;
        }

        SetBurn(false);
    }
}