using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class PickableItem : MonoBehaviour
{
    private Collider2D _collider2D;

    [LabelText("释放时随机最小力度")] public float DropMinForce;
    [LabelText("释放时随机最大力度")] public float DropMaxForce;

    /// <summary>
    /// 是否正在被持有
    /// </summary>
    public bool IsHolding { get; private set; }

    public void Awake()
    {
        _collider2D = GetComponent<Collider2D>();
    }

    public void Pickup()
    {
        IsHolding = true;
        OnPickup();
    }
    
    public virtual void OnPickup()
    {
    }

    public void Drop()
    {
        IsHolding = false;
        OnDrop();
        
        // 随机角度乱飞
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            var randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(0f, 1f));
            rb.AddForce(randomDirection.normalized * Random.Range(DropMinForce, DropMaxForce));
        }
    }

    public virtual void OnDrop()
    {
    }
}