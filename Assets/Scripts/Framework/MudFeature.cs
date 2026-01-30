using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 泥巴效果：图片像泥巴一样随时间向下滴落，玩家可通过鼠标拖拽将顶点拉回以恢复形状。
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class MudFeature : MonoBehaviour
{
    [Header("网格细分")]
    [Tooltip("网格横向与纵向的格子数，越大变形越细腻")]
    [SerializeField] private int _gridSize = 20;

    [Header("滴落")]
    [Tooltip("顶点向下滴落的速度（单位/秒）")]
    [SerializeField] private float _dripSpeed = 0.4f;
    [Tooltip("单顶点最大向下偏移量")]
    [SerializeField] private float _maxDripOffset = 1.5f;

    [Header("拖拽恢复")]
    [Tooltip("拖拽时能影响到的顶点半径（本地空间）")]
    [SerializeField] private float _restoreRadius = 0.3f;
    [Tooltip("每次拖拽对顶点的恢复强度（0~1）")]
    [SerializeField] private float _restoreStrength = 0.25f;

    private SpriteRenderer _spriteRenderer;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private Mesh _mesh;
    private Vector3[] _originalVertices;
    private float[] _dripOffsets;  // 每个顶点的当前滴落偏移（负值=向下）
    private Camera _camera;
    private bool _isDragging;
    private Vector3 _lastLocalDragPoint;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _camera = Camera.main;
        BuildDeformableMesh();
    }

    private void BuildDeformableMesh()
    {
        Sprite sprite = _spriteRenderer.sprite;
        if (sprite == null)
        {
            Debug.LogWarning("MudFeature: SpriteRenderer 没有指定 Sprite。");
            return;
        }

        Bounds bounds = sprite.bounds;
        int vertCount = (_gridSize + 1) * (_gridSize + 1);
        _originalVertices = new Vector3[vertCount];
        _dripOffsets = new float[vertCount];

        List<Vector3> vertices = new List<Vector3>(vertCount);
        List<Vector2> uvs = new List<Vector2>(vertCount);
        List<int> triangles = new List<int>();

        float minX = bounds.min.x;
        float maxX = bounds.max.x;
        float minY = bounds.min.y;
        float maxY = bounds.max.y;

        for (int y = 0; y <= _gridSize; y++)
        {
            for (int x = 0; x <= _gridSize; x++)
            {
                float u = (float)x / _gridSize;
                float v = (float)y / _gridSize;
                Vector3 localPos = new Vector3(
                    Mathf.Lerp(minX, maxX, u),
                    Mathf.Lerp(minY, maxY, v),
                    0f
                );
                vertices.Add(localPos);
                _originalVertices[vertices.Count - 1] = localPos;
                _dripOffsets[vertices.Count - 1] = 0f;
                uvs.Add(new Vector2(u, v));
            }
        }

        for (int y = 0; y < _gridSize; y++)
        {
            for (int x = 0; x < _gridSize; x++)
            {
                int i0 = y * (_gridSize + 1) + x;
                int i1 = i0 + 1;
                int i2 = i0 + (_gridSize + 1);
                int i3 = i2 + 1;
                triangles.Add(i0);
                triangles.Add(i2);
                triangles.Add(i1);
                triangles.Add(i1);
                triangles.Add(i2);
                triangles.Add(i3);
            }
        }

        _mesh = new Mesh { name = "MudMesh" };
        _mesh.SetVertices(vertices);
        _mesh.SetUVs(0, uvs);
        _mesh.SetTriangles(triangles, 0);
        _mesh.RecalculateBounds();
        _mesh.MarkDynamic();

        if (_meshFilter == null)
            _meshFilter = gameObject.AddComponent<MeshFilter>();
        _meshFilter.mesh = _mesh;

        if (_meshRenderer == null)
            _meshRenderer = gameObject.AddComponent<MeshRenderer>();
        _meshRenderer.sharedMaterial = _spriteRenderer.sharedMaterial;
        _meshRenderer.sortingOrder = _spriteRenderer.sortingOrder;
        _meshRenderer.sortingLayerID = _spriteRenderer.sortingLayerID;

        _spriteRenderer.enabled = false;
    }

    private void Update()
    {
        if (_mesh == null || _dripOffsets == null) return;

        // 随时间滴落：所有顶点向下偏移
        float dripDelta = -_dripSpeed * Time.deltaTime;
        for (int i = 0; i < _dripOffsets.Length; i++)
        {
            _dripOffsets[i] = Mathf.Max(_dripOffsets[i] + dripDelta, -_maxDripOffset);
        }

        // 鼠标拖拽恢复
        HandleDragRestore();

        ApplyVertices();
    }

    private void HandleDragRestore()
    {
        if (_camera == null) _camera = Camera.main;
        if (_camera == null) return;

        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = _camera.WorldToScreenPoint(transform.position).z;
        Vector3 worldPoint = _camera.ScreenToWorldPoint(mouseScreen);
        Vector3 localPoint = transform.InverseTransformPoint(worldPoint);

        if (Input.GetMouseButtonDown(0))
        {
            _isDragging = true;
            _lastLocalDragPoint = localPoint;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
        }

        if (_isDragging && Input.GetMouseButton(0))
        {
            // 在拖拽路径附近恢复顶点（当前点 + 上一帧点之间插值，扩大恢复范围）
            RestoreVerticesNear(localPoint);
            RestoreVerticesNear(_lastLocalDragPoint);
            _lastLocalDragPoint = localPoint;
        }
    }

    private void RestoreVerticesNear(Vector3 localPoint)
    {
        float radiusSq = _restoreRadius * _restoreRadius;
        for (int i = 0; i < _originalVertices.Length; i++)
        {
            Vector3 orig = _originalVertices[i];
            float dx = orig.x - localPoint.x;
            float dy = orig.y - localPoint.y;
            if (dx * dx + dy * dy <= radiusSq)
            {
                // 向 0 恢复
                _dripOffsets[i] = Mathf.Lerp(_dripOffsets[i], 0f, _restoreStrength);
            }
        }
    }

    private void ApplyVertices()
    {
        Vector3[] verts = _mesh.vertices;
        for (int i = 0; i < _originalVertices.Length; i++)
        {
            verts[i] = _originalVertices[i] + new Vector3(0f, _dripOffsets[i], 0f);
        }
        _mesh.vertices = verts;
        _mesh.RecalculateBounds();
    }

    /// <summary>
    /// 重置所有顶点滴落偏移，恢复为原始形状。
    /// </summary>
    public void ResetShape()
    {
        if (_dripOffsets == null) return;
        for (int i = 0; i < _dripOffsets.Length; i++)
            _dripOffsets[i] = 0f;
        ApplyVertices();
    }
}
