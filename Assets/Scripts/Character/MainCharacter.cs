using System;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    [Header("手部")]
    [Tooltip("手部 IK 目标，跟随鼠标；持有物品时会挂在其下")]
    public Transform HandIK;

    [Header("拾取")]
    [Tooltip("鼠标射线检测用的摄像机，空则用 Main")]
    [SerializeField] private Camera _pickCamera;
    [Tooltip("ScreenToWorldPoint 时使用的深度（2D 常用 10）")]
    [SerializeField] private float _mouseWorldZ = 10f;
    [Tooltip("持有时物品相对 HandIK 的本地偏移")]
    [SerializeField] private Vector3 _holdLocalOffset = Vector3.zero;

    private PickableItem _heldItem;
    private Rigidbody2D _heldRigidbody;
    private bool _heldWasKinematic;
    private bool _heldWasSimulated;

    private void Update()
    {
        UpdateHandPosition();
        UpdatePickupInput();
    }

    private void UpdateHandPosition()
    {
        if (HandIK == null) return;

        var cam = _pickCamera != null ? _pickCamera : Camera.main;
        if (cam == null) return;

        var screen = Input.mousePosition;
        screen.z = _mouseWorldZ;
        HandIK.position = cam.ScreenToWorldPoint(screen);
    }

    private void UpdatePickupInput()
    {
        var cam = _pickCamera != null ? _pickCamera : Camera.main;
        if (cam == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (_heldItem != null) return;

            Vector2 mouseWorld = GetMouseWorld2D(cam);
            var hit = Physics2D.OverlapPoint(mouseWorld);
            if (hit != null)
            {
                var pickable = hit.GetComponentInParent<PickableItem>();
                if (pickable != null && !pickable.IsHolding)
                {
                    HoldItem(pickable);
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (_heldItem != null)
                ReleaseHeldItem();
        }
    }

    private Vector2 GetMouseWorld2D(Camera cam)
    {
        var screen = Input.mousePosition;
        screen.z = _mouseWorldZ;
        return (Vector2)cam.ScreenToWorldPoint(screen);
    }

    private void HoldItem(PickableItem item)
    {
        _heldItem = item;
        _heldRigidbody = item.GetComponent<Rigidbody2D>();
        if (_heldRigidbody != null)
        {
            _heldWasKinematic = _heldRigidbody.isKinematic;
            _heldWasSimulated = _heldRigidbody.simulated;
            _heldRigidbody.isKinematic = true;
            _heldRigidbody.simulated = false;
            _heldRigidbody.velocity = Vector2.zero;
            _heldRigidbody.angularVelocity = 0f;
        }
        else
        {
            _heldWasKinematic = true;
            _heldWasSimulated = false;
        }

        _heldItem.Pickup();

        if (HandIK != null)
        {
            _heldItem.transform.SetParent(HandIK, true);
            _heldItem.transform.localPosition = _holdLocalOffset;
            _heldItem.transform.localRotation = Quaternion.identity;
            _heldItem.transform.localScale = Vector3.one;
        }
    }

    private void ReleaseHeldItem()
    {
        if (_heldItem == null) return;

        if (_heldRigidbody != null)
        {
            _heldRigidbody.isKinematic = _heldWasKinematic;
            _heldRigidbody.simulated = _heldWasSimulated;
            _heldRigidbody = null;
        }

        _heldItem.transform.SetParent(null, true);
        _heldItem.Drop();
        _heldItem = null;
    }

    /// <summary> 当前是否正在持有物品。 </summary>
    public bool IsHoldingItem => _heldItem != null;

    /// <summary> 当前持有的物品，未持有时为 null。 </summary>
    public PickableItem HeldItem => _heldItem;
}