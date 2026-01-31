using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 区域检测器。在 Scene 中定义多个多边形区域（如安全区、危险区），检测被追踪节点当前所在区域，返回对应加权值（用于怀疑度等）。
/// 区域顶点为本地坐标（相对本 Transform），在 Scene 窗口可通过自定义 Editor 拖拽顶点编辑。
/// </summary>
public class AreaDetector : MonoBehaviour
{
    [Serializable]
    public class AreaDefinition
    {
        [Tooltip("区域名称，如：安全区域、危险区域1")]
        public string displayName = "区域";

        [Tooltip("该区域的加权值，用于怀疑度等计算")]
        public float weight = 0f;

        [Tooltip("多边形顶点（本地 XY），至少 3 个点；在 Scene 中可用手柄拖拽编辑")]
        public List<Vector2> points = new List<Vector2>();

        public bool IsValid => points != null && points.Count >= 3;
    }

    [Header("区域列表")]
    [Tooltip("按顺序检测，先命中的区域生效")]
    [SerializeField] private List<AreaDefinition> areas = new List<AreaDefinition>();

    [Header("默认值")]
    [Tooltip("不在任何区域内时的加权值")]
    [SerializeField] private float defaultWeight = 0f;

    /// <summary> 获取当前所在区域索引；不在任何区域内返回 -1。 </summary>
    public int GetCurrentAreaIndex(Vector3 worldPosition)
    {
        if (areas == null || areas.Count == 0) return -1;

        Vector2 local = transform.InverseTransformPoint(worldPosition);

        for (int i = 0; i < areas.Count; i++)
        {
            if (!areas[i].IsValid) continue;
            if (ContainsPoint(areas[i].points, local))
                return i;
        }

        return -1;
    }
    
    

    /// <summary> 射线法判断点是否在多边形内（XY 平面）。 </summary>
    public static bool ContainsPoint(List<Vector2> polygon, Vector2 point)
    {
        if (polygon == null || polygon.Count < 3) return false;

        int n = polygon.Count;
        int crossings = 0;

        for (int i = 0, j = n - 1; i < n; j = i, i++)
        {
            Vector2 a = polygon[j];
            Vector2 b = polygon[i];

            if ((a.y > point.y) == (b.y > point.y)) continue;

            float t = (point.y - a.y) / (b.y - a.y);
            float x = a.x + t * (b.x - a.x);
            if (point.x < x)
                crossings++;
        }

        return (crossings & 1) == 1;
    }

    /// <summary> 获取区域列表（只读）。 </summary>
    public IReadOnlyList<AreaDefinition> GetAreas() => areas;

    /// <summary> 在运行时添加区域。 </summary>
    public void AddArea(AreaDefinition area)
    {
        if (areas == null) areas = new List<AreaDefinition>();
        areas.Add(area);
    }

    /// <summary> 清空区域列表。 </summary>
    public void ClearAreas()
    {
        areas?.Clear();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        DrawAreaGizmos(true);
    }

    private void OnDrawGizmos()
    {
        DrawAreaGizmos(false);
    }

    private void DrawAreaGizmos(bool selected)
    {
        if (areas == null) return;

        for (int i = 0; i < areas.Count; i++)
        {
            var area = areas[i];
            if (!area.IsValid) continue;

            // bool isCurrent = Application.isPlaying && GetCurrentAreaIndex() == i;
            // Gizmos.color = selected ? (isCurrent ? Color.green : Color.yellow) : new Color(1f, 1f, 0f, 0.3f);
            //
            // Vector3 p0 = transform.TransformPoint(new Vector3(area.points[area.points.Count - 1].x, area.points[area.points.Count - 1].y, 0f));
            // for (int j = 0; j < area.points.Count; j++)
            // {
            //     Vector3 p1 = transform.TransformPoint(new Vector3(area.points[j].x, area.points[j].y, 0f));
            //     Gizmos.DrawLine(p0, p1);
            //     p0 = p1;
            // }
        }
    }
#endif
}
