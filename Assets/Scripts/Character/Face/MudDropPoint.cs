using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 泥巴下落表演控制器。
/// 挂载到节点后：模拟泥巴缓慢下沉；鼠标进入/离开时触发带位置的回调；支持鼠标拖拽中心点，带阻尼跟随。
/// 2D：需要 Collider2D（可选 Rigidbody2D，有则自动设为 Kinematic）；3D：需要 Collider。否则无法检测悬停与拖拽。
/// </summary>
public class MudDropPoint : MonoBehaviour
{
    [Header("下沉表演")]
    [Tooltip("下沉速度下限（单位/秒），与上限之间用连续随机插值")]
    [SerializeField] private float descentSpeedMin = 0.2f;
    [Tooltip("下沉速度上限（单位/秒）")]
    [SerializeField] private float descentSpeedMax = 0.8f;
    [Tooltip("左右漂移最大速度（单位/秒），0 表示不漂移；方向由连续随机决定")]
    [SerializeField] private float descentDriftSpeed = 0.15f;
    [Tooltip("速度随机变化快慢：值越大，速度随时间变化越频繁")]
    [SerializeField] private float descentNoiseScale = 1f;
    [Tooltip("噪声种子，不同物体可设不同值以得到不同曲线；0 则用实例 ID")]
    [SerializeField] private float descentNoiseSeed = 0f;
    [Tooltip("下沉平滑时间，越大越慢、越像泥巴")]
    [SerializeField] private float descentSmoothTime = 1.5f;

    [Header("拖拽与阻尼")]
    [Tooltip("拖拽时中心点跟随的平滑时间")]
    [SerializeField] private float dragSmoothTime = 0.15f;
    [Tooltip("拖拽时跟随的阻尼系数，越大越不跟手")]
    [SerializeField] [Range(0f, 1f)] private float dragDamping = 0.85f;
    [Tooltip("用于将屏幕坐标转为世界坐标的摄像机，空则用 Main")]
    [SerializeField] private Camera referenceCamera;

    [Header("鼠标事件回调（世界坐标）")]
    [SerializeField] private UnityEvent<Vector3> onMouseEnterWithPosition;
    [SerializeField] private UnityEvent<Vector3> onMouseExitWithPosition;

    /// <summary> 当前是否被鼠标悬停。 </summary>
    public bool IsHovered { get; private set; }
    /// <summary> 当前是否正在被拖拽。 </summary>
    public bool IsDragging { get; private set; }

    /// <summary> 鼠标进入时调用，参数为当前鼠标世界坐标。 </summary>
    public event Action<Vector3> MouseEntered;
    /// <summary> 鼠标离开时调用，参数为离开时的鼠标世界坐标。 </summary>
    public event Action<Vector3> MouseExited;

    private Vector3 _currentCenter;
    private Vector3 _descentVelocity;
    private Vector3 _dragVelocity;
    /// <summary> 按下时鼠标相对中心点的偏移，拖拽时保持该相对位置。 </summary>
    private Vector3 _dragOffset;
    /// <summary> Perlin 噪声采样用时间，用于连续随机下沉速度。 </summary>
    private float _descentNoiseT;
    private Collider2D _collider2D;
    private Rigidbody2D _rb2d;

    private void Awake()
    {
        _collider2D = GetComponent<Collider2D>();
        _rb2d = GetComponent<Rigidbody2D>();
        if (_rb2d != null)
        {
            _rb2d.isKinematic = true;
            _rb2d.simulated = false;
        }
        _currentCenter = transform.position;
        if (referenceCamera == null)
            referenceCamera = Camera.main;
        float seed = descentNoiseSeed != 0f ? descentNoiseSeed : GetInstanceID();
        _descentNoiseT = seed * 0.1f;
    }

    private void OnEnable()
    {
        IsHovered = false;
        IsDragging = false;
    }

    private void Update()
    {
        if (referenceCamera == null)
            referenceCamera = Camera.main;

        UpdateHoverWithRaycast();
        UpdateDrag();
        UpdateDescent();
        ApplyCenterToTransform();
    }

    /// <summary> 用射线检测当前帧鼠标是否悬停在本物体上，并触发进入/离开回调。 </summary>
    private void UpdateHoverWithRaycast()
    {
        Vector3 mouseWorld = GetMouseWorldPosition();
        bool hit = IsPointerOverThis(mouseWorld);

        if (hit && !IsHovered)
        {
            IsHovered = true;
            onMouseEnterWithPosition?.Invoke(mouseWorld);
            MouseEntered?.Invoke(mouseWorld);
        }
        else if (!hit && IsHovered)
        {
            IsHovered = false;
            onMouseExitWithPosition?.Invoke(mouseWorld);
            MouseExited?.Invoke(mouseWorld);
        }
    }

    private void UpdateDrag()
    {
        if (referenceCamera == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorld = GetMouseWorldPosition();
            bool hit = IsPointerOverThis(mouseWorld);
            if (hit)
            {
                IsDragging = true;
                _dragVelocity = Vector3.zero;
                _dragOffset = mouseWorld - _currentCenter;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            IsDragging = false;
        }

        if (IsDragging && Input.GetMouseButton(0))
        {
            Vector3 mouseWorld = GetMouseWorldPosition();
            Vector3 target = mouseWorld - _dragOffset;
            float dt = Time.deltaTime;
            float smooth = Mathf.Clamp01(1f - dragDamping);
            _currentCenter = Vector3.SmoothDamp(_currentCenter, target, ref _dragVelocity, dragSmoothTime * (1f / (smooth + 0.01f)), Mathf.Infinity, dt);
        }
    }

    private void UpdateDescent()
    {
        if (IsDragging) return;

        float seed = descentNoiseSeed != 0f ? descentNoiseSeed : GetInstanceID() * 0.01f;
        _descentNoiseT += Time.deltaTime * descentNoiseScale;

        float t = Mathf.PerlinNoise(_descentNoiseT, seed);
        float speed = Mathf.Lerp(descentSpeedMin, descentSpeedMax, t);

        float driftT = Mathf.PerlinNoise(_descentNoiseT + 50f, seed + 0.5f);
        float drift = (driftT - 0.5f) * 2f;
        float driftX = descentDriftSpeed > 0f ? drift * descentDriftSpeed * Time.deltaTime : 0f;
        float downY = -speed * Time.deltaTime;

        Vector3 delta = new Vector3(driftX, downY, 0f);
        Vector3 descentTarget = _currentCenter + delta;
        _currentCenter = Vector3.SmoothDamp(_currentCenter, descentTarget, ref _descentVelocity, descentSmoothTime, Mathf.Infinity, Time.deltaTime);
    }

    private void ApplyCenterToTransform()
    {
        transform.position = _currentCenter;
    }

    private bool IsPointerOverThis(Vector3 mouseWorld)
    {
        if (_collider2D != null)
        {
            return _collider2D.OverlapPoint(new Vector2(mouseWorld.x, mouseWorld.y));
        }
        
        return false;
    }

    private Vector3 GetMouseWorldPosition()
    {
        if (referenceCamera == null) return _currentCenter;
        Vector3 screen = Input.mousePosition;
        screen.z = referenceCamera.orthographic ? Mathf.Abs(referenceCamera.transform.position.z - transform.position.z) : Mathf.Abs(transform.position.z - referenceCamera.transform.position.z);
        return referenceCamera.ScreenToWorldPoint(screen);
    }

    /// <summary> 设置中心点世界坐标（会平滑过渡）。 </summary>
    public void SetCenterWorldPosition(Vector3 worldPosition)
    {
        _currentCenter = worldPosition;
        _descentVelocity = Vector3.zero;
    }

    /// <summary> 设置下沉速度范围（同时设上下限为同一值则等效恒定速度）。 </summary>
    public void SetDescentSpeedRange(float min, float max)
    {
        descentSpeedMin = min;
        descentSpeedMax = max;
    }
}
