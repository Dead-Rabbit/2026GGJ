using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 单个头顶 UI 实例。
/// 负责根据目标 Transform 或世界坐标，实时更新自身在 Canvas 中的位置。
/// </summary>
public class PateInstance : MonoBehaviour
{
    private Transform _target;
    private Vector2 _worldPosition;
    private bool _followTransform;

    /// <summary>
    /// 屏幕坐标系下的偏移（像素），在世界坐标转换到屏幕坐标后再加上。
    /// </summary>
    private Vector2 _screenOffset;

    private Camera _worldCamera;
    private Canvas _canvas;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        if (_canvas == null)
        {
            _canvas = GetComponentInParent<Canvas>();
        }
    }

    /// <summary>
    /// 初始化：跟随目标 Transform。
    /// </summary>
    public void InitForTarget(Transform target, Vector2 screenOffset, Camera worldCamera, Canvas canvas)
    {
        _target = target;
        _followTransform = true;
        _screenOffset = screenOffset;
        _worldCamera = worldCamera;
        _canvas = canvas;
    }

    /// <summary>
    /// 初始化：跟随固定世界坐标点。
    /// </summary>
    public void InitForWorldPosition(Vector2 worldPosition, Vector2 screenOffset, Camera worldCamera, Canvas canvas)
    {
        _worldPosition = worldPosition;
        _followTransform = false;
        _screenOffset = screenOffset;
        _worldCamera = worldCamera;
        _canvas = canvas;
    }

    /// <summary>
    /// 如果是固定世界坐标模式，可以通过该接口更新绑定的世界坐标。
    /// </summary>
    public void SetWorldPosition(Vector3 worldPosition)
    {
        _worldPosition = worldPosition;
    }

    /// <summary>
    /// 更新偏移配置。
    /// </summary>
    public void SetOffsets(Vector2 screenOffset)
    {
        _screenOffset = screenOffset;
    }

    private void LateUpdate()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        if (_rectTransform == null)
        {
            return;
        }

        if (_worldCamera == null)
        {
            _worldCamera = Camera.main;
        }

        if (_canvas == null)
        {
            _canvas = GetComponentInParent<Canvas>();
        }

        Vector3 baseWorldPos;

        if (_followTransform)
        {
            if (_target == null)
            {
                // 目标被销毁时，自动销毁自身
                Destroy(gameObject);
                return;
            }

            baseWorldPos = _target.position;
        }
        else
        {
            baseWorldPos = _worldPosition;
        }

        // 世界坐标 -> 屏幕坐标
        Vector3 screenPos = _worldCamera.WorldToScreenPoint(baseWorldPos);

        // 如果在摄像机背面，不显示
        if (screenPos.z < 0)
        {
            if (_rectTransform.gameObject.activeSelf)
            {
                _rectTransform.gameObject.SetActive(false);
            }
            return;
        }

        if (!_rectTransform.gameObject.activeSelf)
        {
            _rectTransform.gameObject.SetActive(true);
        }

        // 添加屏幕像素偏移
        screenPos.x += _screenOffset.x;
        screenPos.y += _screenOffset.y;

        // 将屏幕坐标转换为 Canvas 本地坐标
        if (_canvas != null)
        {
            RectTransform canvasRect = _canvas.transform as RectTransform;
            Vector2 localPos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _worldCamera, out localPos))
            {
                _rectTransform.localPosition = localPos;
            }
        }
        else
        {
            // 回退：直接设置世界坐标（适用于 World Space Canvas）
            _rectTransform.position = screenPos;
        }
    }
}