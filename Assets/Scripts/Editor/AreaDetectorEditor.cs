#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AreaDetector))]
public class AreaDetectorEditor : Editor
{
    private const float HandleSize = 0.08f;
    private int _selectedAreaIndex = -1;
    private int _selectedPointIndex = -1;

    private void OnSceneGUI()
    {
        var detector = (AreaDetector)target;
        if (detector == null) return;

        var areas = detector.GetAreas();
        if (areas == null) return;

        Transform tr = detector.transform;

        for (int a = 0; a < areas.Count; a++)
        {
            var area = areas[a];
            if (area.points == null || area.points.Count < 3) continue;

            for (int p = 0; p < area.points.Count; p++)
            {
                Vector2 local = area.points[p];
                Vector3 world = tr.TransformPoint(new Vector3(local.x, local.y, 0f));

                float size = HandleUtility.GetHandleSize(world) * HandleSize;
                EditorGUI.BeginChangeCheck();
                var fmh_34_66_639054504772434315 = Quaternion.identity; Vector3 newWorld = Handles.FreeMoveHandle(world, size, Vector3.zero, Handles.DotHandleCap);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(detector, "Move Area Point");
                    Vector3 newLocal = tr.InverseTransformPoint(newWorld);
                    area.points[p] = new Vector2(newLocal.x, newLocal.y);
                    EditorUtility.SetDirty(detector);
                }
            }

            Handles.color = new Color(1f, 1f, 0f, 0.6f);
            Vector3 prev = tr.TransformPoint(new Vector3(area.points[area.points.Count - 1].x, area.points[area.points.Count - 1].y, 0f));
            for (int i = 0; i < area.points.Count; i++)
            {
                Vector3 curr = tr.TransformPoint(new Vector3(area.points[i].x, area.points[i].y, 0f));
                Handles.DrawLine(prev, curr);
                prev = curr;
            }
        }

        Handles.color = Color.white;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var detector = (AreaDetector)target;
        if (detector == null) return;

        EditorGUILayout.Space(4);
        EditorGUILayout.LabelField("区域编辑", EditorStyles.boldLabel);

        var areas = detector.GetAreas();
        if (areas != null)
        {
            EditorGUI.BeginChangeCheck();
            for (int i = 0; i < areas.Count; i++)
            {
                var area = areas[i];
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                area.displayName = EditorGUILayout.TextField("名称", area.displayName);
                area.weight = EditorGUILayout.FloatField("加权值", area.weight);

                int count = area.points != null ? area.points.Count : 0;
                EditorGUILayout.LabelField("顶点数", count.ToString());

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("添加顶点 (末尾)"))
                {
                    Undo.RecordObject(detector, "Add Area Point");
                    if (area.points == null) area.points = new System.Collections.Generic.List<Vector2>();
                    Vector2 last = count > 0 ? area.points[count - 1] : Vector2.zero;
                    area.points.Add(last + new Vector2(0.5f, 0f));
                    EditorUtility.SetDirty(detector);
                }
                if (count >= 3 && GUILayout.Button("删除末顶点"))
                {
                    Undo.RecordObject(detector, "Remove Area Point");
                    area.points.RemoveAt(area.points.Count - 1);
                    EditorUtility.SetDirty(detector);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();
            }
            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(detector);
        }

        EditorGUILayout.Space(4);
        if (GUILayout.Button("添加新区域"))
        {
            Undo.RecordObject(detector, "Add Area");
            var newArea = new AreaDetector.AreaDefinition
            {
                displayName = "新区域",
                weight = 0f,
                points = new System.Collections.Generic.List<Vector2>
                {
                    new Vector2(-1f, -1f),
                    new Vector2(1f, -1f),
                    new Vector2(0f, 1f)
                }
            };
            detector.AddArea(newArea);
            EditorUtility.SetDirty(detector);
        }

        EditorGUILayout.Space(2);
        EditorGUILayout.HelpBox(
            "在 Scene 窗口中选中此物体后，可拖拽各区域顶点以调整形状。\n加权值用于怀疑度等计算：安全区可设 0，危险区设更大值。",
            MessageType.Info);
    }
}
#endif
