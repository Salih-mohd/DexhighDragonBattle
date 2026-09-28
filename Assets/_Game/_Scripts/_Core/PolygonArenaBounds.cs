using System.Collections.Generic;
using UnityEngine;

public class PolygonArenaBounds : MonoBehaviour
{
    [SerializeField]
    private List<Vector3> points = new List<Vector3>
    {
        new Vector3(-10f, 0f, -10f),
        new Vector3(-10f, 0f,  10f),
        new Vector3( 10f, 0f,  10f),
        new Vector3( 10f, 0f, -10f)
    };

    public IReadOnlyList<Vector3> Points => points;

    public Vector3 ClampPosition(Vector3 worldPosition)
    {
        if (points == null || points.Count < 3)
            return worldPosition;

        Vector3 localPosition =
            transform.InverseTransformPoint(worldPosition);

        if (IsPointInside(localPosition))
            return worldPosition;

        Vector3 closestPoint =
            GetClosestPointOnBoundary(localPosition);

        // Keep the character's existing Y position.
        closestPoint.y = localPosition.y;

        return transform.TransformPoint(closestPoint);
    }

    private bool IsPointInside(Vector3 point)
    {
        bool inside = false;

        int j = points.Count - 1;

        for (int i = 0; i < points.Count; i++)
        {
            Vector3 a = points[i];
            Vector3 b = points[j];

            bool intersects =
                ((a.z > point.z) != (b.z > point.z)) &&
                (point.x <
                    (b.x - a.x) *
                    (point.z - a.z) /
                    (b.z - a.z) +
                    a.x);

            if (intersects)
                inside = !inside;

            j = i;
        }

        return inside;
    }

    private Vector3 GetClosestPointOnBoundary(Vector3 point)
    {
        Vector3 closestPoint = points[0];
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < points.Count; i++)
        {
            Vector3 start = points[i];
            Vector3 end =
                points[(i + 1) % points.Count];

            Vector3 pointOnEdge =
                ClosestPointOnSegment(
                    point,
                    start,
                    end
                );

            float distance =
                (pointOnEdge - point).sqrMagnitude;

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = pointOnEdge;
            }
        }

        return closestPoint;
    }

    private Vector3 ClosestPointOnSegment(
        Vector3 point,
        Vector3 start,
        Vector3 end)
    {
        Vector2 p =
            new Vector2(point.x, point.z);

        Vector2 a =
            new Vector2(start.x, start.z);

        Vector2 b =
            new Vector2(end.x, end.z);

        Vector2 ab = b - a;

        float lengthSquared =
            ab.sqrMagnitude;

        if (lengthSquared <= Mathf.Epsilon)
            return start;

        float t =
            Vector2.Dot(p - a, ab) /
            lengthSquared;

        t = Mathf.Clamp01(t);

        Vector2 result =
            a + ab * t;

        return new Vector3(
            result.x,
            point.y,
            result.y
        );
    }

    private void OnDrawGizmosSelected()
    {
        if (points == null || points.Count < 2)
            return;

        Gizmos.color = Color.green;

        for (int i = 0; i < points.Count; i++)
        {
            Vector3 current =
                transform.TransformPoint(points[i]);

            Vector3 next =
                transform.TransformPoint(
                    points[(i + 1) % points.Count]
                );

            Gizmos.DrawLine(current, next);
            Gizmos.DrawSphere(current, 0.15f);
        }
    }

#if UNITY_EDITOR

    public void SetPoint(int index, Vector3 point)
    {
        if (index < 0 || index >= points.Count)
            return;

        point.y = 0f;
        points[index] = point;
    }

    public void AddPoint()
    {
        Vector3 newPoint;

        if (points.Count >= 2)
        {
            Vector3 last =
                points[points.Count - 1];

            Vector3 previous =
                points[points.Count - 2];

            newPoint =
                last + (last - previous);
        }
        else
        {
            newPoint = Vector3.zero;
        }

        newPoint.y = 0f;

        points.Add(newPoint);
    }

    public void RemoveLastPoint()
    {
        if (points.Count <= 3)
            return;

        points.RemoveAt(
            points.Count - 1
        );
    }

#endif
}