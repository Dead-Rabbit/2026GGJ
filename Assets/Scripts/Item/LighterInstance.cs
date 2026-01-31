using System;
using UnityEngine;

[ExecuteInEditMode]
public class LighterInstance : PickableItem
{
    public Transform Fire;

    public void Start()
    {
        SetActive(IsHolding);
    }

    public override void OnPickup()
    {
        base.OnPickup();
        
        SetActive(true);
    }

    public override void OnDrop()
    {
        base.OnDrop();
        
        SetActive(false);
    }

    public void SetActive(bool isActive)
    {
        Fire?.gameObject.SetActive(isActive);
    }

    public void Update()
    {
        if (Fire == null)
        {
            return;
        }

        if (IsHolding)
        {
            // 更新火苗始终朝向
            Fire.transform.rotation = Quaternion.Euler(0, 0, 0);
        
            // 检查点燃行为
            var hit = Physics2D.OverlapPoint(Fire.transform.position);
            if (hit != null)
            {
                var candle = hit.GetComponentInParent<CandleInstance>();
                if (candle != null)
                {
                    candle.SetBurn(true);
                }
            }
        }
    }
}