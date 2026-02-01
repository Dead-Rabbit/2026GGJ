using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.U2D.IK;

public class MainCharacter : MonoBehaviour
{
    [Header("手部")]
    [Tooltip("手部 IK 目标，跟随鼠标；持有物品时会挂在其下")]
    public Transform LeftHandIK;
    [Tooltip("手部 IK 目标，跟随鼠标；持有物品时会挂在其下")]
    public Transform RightHandIK;

    public Transform DefaultLeftHandPosition;
    public Transform DefaultRightHandPosition;
    
    public Transform SwitchHandPosition;

    [LabelText("左手模型")] public Transform LeftHand;
    [LabelText("左手模型_捏")] public Transform LeftHand_HoldItem;
    [LabelText("左手捏位置")] public Transform LeftHandHoldPosition;
    [LabelText("左手IK解算器")] public LimbSolver2D LeftLimbSolver2D;
    [LabelText("右手模型")] public Transform RightHand;
    [LabelText("右手模型_捏")] public Transform RightHand_HoldItem;
    [LabelText("右手捏位置")] public Transform RightHandHoldPosition;
    [LabelText("右手IK解算器")] public LimbSolver2D RightLimbSolver2D;

    [Header("IK 过渡")]
    [Tooltip("手部 IK 跟随目标位置的平滑时间，越小跟手越快，越大越柔和")]
    [SerializeField] private float _ikSmoothTime = 0.06f;
    [Tooltip("手回到默认位置时的平滑时间，可略大一些更自然")]
    [SerializeField] private float _ikReturnSmoothTime = 0.12f;

    [Header("拾取")]
    [Tooltip("鼠标射线检测用的摄像机，空则用 Main")]
    [SerializeField] private Camera _pickCamera;
    [Tooltip("ScreenToWorldPoint 时使用的深度（2D 常用 10）")]
    [SerializeField] private float _mouseWorldZ = 10f;
    [Tooltip("持有时物品相对 HandIK 的本地偏移")]
    [SerializeField] private Vector3 _holdLocalOffset = Vector3.zero;

    private PickableItem _heldItem;
    private Vector3 _leftIKVelocity;
    private Vector3 _rightIKVelocity;
    private Rigidbody2D _heldRigidbody;
    private bool _heldWasKinematic;
    private bool _heldWasSimulated;
    private Transform _currentIKHolder;
    private Transform _currentHolderSocket;
    private bool _isLeftHold = false;

    private void Update()
    {
        UpdateHandPosition();
        UpdatePickupInput();
    }

    private void UpdateHandPosition()
    {
        if (LeftHandIK == null || RightHandIK == null) return;

        var cam = _pickCamera != null ? _pickCamera : Camera.main;
        if (cam == null) return;

        var screen = Input.mousePosition;
        screen.z = _mouseWorldZ;
        var worldPosition = cam.ScreenToWorldPoint(screen);

        Vector3 defaultLeft = DefaultLeftHandPosition != null ? DefaultLeftHandPosition.position : LeftHandIK.position;
        Vector3 defaultRight = DefaultRightHandPosition != null ? DefaultRightHandPosition.position : RightHandIK.position;

        bool useLeft = worldPosition.x < SwitchHandPosition.position.x;

        if (_heldItem != null && useLeft != _isLeftHold)
        {
            var newSocket = useLeft ? LeftHandHoldPosition : RightHandHoldPosition;
            if (newSocket != null)
            {
                _heldItem.transform.SetParent(newSocket, true);
                _heldItem.transform.localPosition = _holdLocalOffset;
                _heldItem.transform.localRotation = Quaternion.identity;
                _heldItem.transform.localScale = Vector3.one;
            }
        }

        _isLeftHold = useLeft;
        _currentIKHolder = useLeft ? LeftHandIK : RightHandIK;
        _currentHolderSocket = useLeft ? LeftHandHoldPosition : RightHandHoldPosition;

        float dt = Mathf.Min(Time.deltaTime, 0.1f);
        float followSmooth = _ikSmoothTime;
        float returnSmooth = _ikReturnSmoothTime;

        Vector3 leftTarget = useLeft ? worldPosition : defaultLeft;
        Vector3 rightTarget = useLeft ? defaultRight : worldPosition;

        float leftSmooth = useLeft ? followSmooth : returnSmooth;
        float rightSmooth = useLeft ? returnSmooth : followSmooth;

        LeftHandIK.position = Vector3.SmoothDamp(LeftHandIK.position, leftTarget, ref _leftIKVelocity, leftSmooth, Mathf.Infinity, dt);
        RightHandIK.position = Vector3.SmoothDamp(RightHandIK.position, rightTarget, ref _rightIKVelocity, rightSmooth, Mathf.Infinity, dt);

        if (_heldItem == null)
        {
            LeftHand?.gameObject.SetActive(true);
            LeftHand_HoldItem?.gameObject.SetActive(false);
            RightHand?.gameObject.SetActive(true);
            RightHand_HoldItem?.gameObject.SetActive(false);
        }
        else
        {
            if (_isLeftHold)
            {
                LeftHand?.gameObject.SetActive(false);
                LeftHand_HoldItem?.gameObject.SetActive(true);
                RightHand?.gameObject.SetActive(true);
                RightHand_HoldItem?.gameObject.SetActive(false);
            }
            else
            {
                LeftHand?.gameObject.SetActive(true);
                LeftHand_HoldItem?.gameObject.SetActive(false);
                RightHand?.gameObject.SetActive(false);
                RightHand_HoldItem?.gameObject.SetActive(true);
            }
        }
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

        if (_currentHolderSocket != null)
        {
            _heldItem.transform.SetParent(_currentHolderSocket, true);
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

    #region 动画

    

    #endregion
}