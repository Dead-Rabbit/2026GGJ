using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 负责管理所有头顶 UI
/// 将该组件挂在 Canvas 下面的一个空节点上，然后在 Inspector 中指定 PatePrefab。
/// </summary>
public class PateManager : MonoBehaviour
{
    private static PateManager _instance;
    public static PateManager Instance => _instance;

    private readonly List<PateInstance> _instances = new List<PateInstance>();
    private Canvas _canvas;
    private Camera _worldCamera;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        _canvas = GetComponentInParent<Canvas>();

        // 默认使用主摄像机，你也可以在外部手动指定
        _worldCamera = Camera.main;
    }

    private void OnDestroy()
    {
        foreach (var instance in _instances)
        {
            Destroy(instance.gameObject);
        }
        _instances.Clear();
    }

    /// <summary>
    /// 可选：外部手动指定用于转换坐标的摄像机。
    /// </summary>
    public void SetWorldCamera(Camera cam)
    {
        _worldCamera = cam;
    }

    /// <summary>
    /// 创建一个跟随目标 Transform 的头顶 UI。
    /// worldOffset：世界坐标系下的偏移（单位：世界单位，比如(0,2,0)表示在目标上方 2 米处）。
    /// screenOffset：屏幕坐标系下的偏移，单位是像素（在世界坐标转换成屏幕坐标后再加上）。
    /// </summary>
    public PateInstance CreateForTarget(PateInstance patePrefab, Transform target, Vector2 screenOffset)
    {
        if (patePrefab == null || target == null)
        {
            Debug.LogWarning("[PateManager] Prefab 或 Target 为空，创建失败。");
            return null;
        }

        PateInstance instance = Instantiate(patePrefab, transform);
        instance.InitForTarget(target, screenOffset, GetWorldCamera(), GetCanvas());
        _instances.Add(instance);
        return instance;
    }

    /// <summary>
    /// 创建一个跟随指定世界坐标点的头顶 UI。
    /// worldPosition：基础世界坐标点。
    /// worldOffset：世界坐标系下的附加偏移。
    /// screenOffset：屏幕坐标系下的偏移，单位是像素。
    /// </summary>
    public PateInstance CreateForWorldPosition(PateInstance patePrefab, Vector3 worldPosition, Vector2 screenOffset)
    {
        if (patePrefab == null)
        {
            Debug.LogWarning("[PateManager] Prefab 为空，创建失败。");
            return null;
        }

        PateInstance instance = Instantiate(patePrefab, transform);
        instance.InitForWorldPosition(worldPosition, screenOffset, GetWorldCamera(), GetCanvas());
        _instances.Add(instance);
        return instance;
    }

    /// <summary>
    /// 从管理器中移除并销毁一个头顶 UI 实例。
    /// </summary>
    public void Remove(PateInstance instance)
    {
        if (instance == null) return;

        if (_instances.Contains(instance))
        {
            _instances.Remove(instance);
        }

        Destroy(instance.gameObject);
    }

    private Camera GetWorldCamera()
    {
        if (_worldCamera == null)
        {
            _worldCamera = Camera.main;
        }
        return _worldCamera;
    }

    private Canvas GetCanvas()
    {
        if (_canvas == null)
        {
            _canvas = GetComponentInParent<Canvas>();
        }
        return _canvas;
    }
}
