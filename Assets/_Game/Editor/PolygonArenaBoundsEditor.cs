
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PolygonArenaBounds))]
public class PolygonArenaBoundsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PolygonArenaBounds bounds =
            (PolygonArenaBounds)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Add Point"))
        {
            Undo.RecordObject(
                bounds,
                "Add Arena Point"
            );

            bounds.AddPoint();

            EditorUtility.SetDirty(bounds);
        }

        if (GUILayout.Button("Remove Last Point"))
        {
            Undo.RecordObject(
                bounds,
                "Remove Arena Point"
            );

            bounds.RemoveLastPoint();

            EditorUtility.SetDirty(bounds);
        }
    }

    private void OnSceneGUI()
    {
        PolygonArenaBounds bounds =
            (PolygonArenaBounds)target;

        for (int i = 0;
             i < bounds.Points.Count;
             i++)
        {
            Vector3 worldPoint =
                bounds.transform.TransformPoint(
                    bounds.Points[i]
                );

            float handleSize =
                HandleUtility.GetHandleSize(
                    worldPoint
                ) * 0.1f;

            EditorGUI.BeginChangeCheck();

            Vector3 newWorldPoint =
                Handles.FreeMoveHandle(
                    worldPoint,
                    handleSize,
                    Vector3.zero,
                    Handles.SphereHandleCap
                );

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(
                    bounds,
                    "Move Arena Point"
                );

                Vector3 localPoint =
                    bounds.transform
                        .InverseTransformPoint(
                            newWorldPoint
                        );

                localPoint.y = 0f;

                bounds.SetPoint(
                    i,
                    localPoint
                );

                EditorUtility.SetDirty(bounds);
            }

            Handles.Label(
                worldPoint + Vector3.up * 0.3f,
                $"Point {i}"
            );
        }
    }
}