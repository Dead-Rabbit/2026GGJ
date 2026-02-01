using System;
using UnityEngine;

public class ForkInstance : PickableItem
{
    public Transform ForkPoint;

    private FoodInstance food;
    private Vector3 localOffset;

    public void Update()
    {
        if (!IsHolding || !ForkPoint)
            return;

        if (!food)
        {
            var hit = Physics2D.OverlapPoint(ForkPoint.transform.position, LayerMask.GetMask("Food"));
            if (hit != null)
            {
                food = hit.GetComponentInParent<FoodInstance>();
                localOffset = transform.InverseTransformPoint(food.transform.position);
            }
        }
        else
        {
            food.transform.position = transform.TransformPoint(localOffset);
        }
    }

    public override void OnDrop()
    {
        base.OnDrop();

        food = null;
    }
}