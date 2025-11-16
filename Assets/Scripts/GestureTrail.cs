using System.Collections.Generic;
using UnityEngine;

public class GestureTrail : MonoBehaviour
{
    [Header("Drawing")]
    public Transform drawTip;               // position at controller or finger
    public LineRenderer lineRenderer;
    public float minDistance = 0.015f;      // sensitivity for adding new points

    private bool isDrawing = false;
    private List<Vector3> points = new List<Vector3>();

    void Update()
    {
        // Detect trigger pull on the RIGHT controller (Quest Touch)
        bool triggerDown = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        bool triggerUp   = OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);

        // Start drawing
        if (triggerDown)
        {
            StartDrawing();
        }

        // Stop drawing
        if (triggerUp)
        {
            StopDrawing();
        }

        // Record trail while drawing
        if (isDrawing)
        {
            AddPoint();
        }
    }

    void StartDrawing()
    {
        isDrawing = true;
        points.Clear();
        lineRenderer.positionCount = 0;
    }

    void StopDrawing()
    {
        isDrawing = false;

        // Remove visual trail
        lineRenderer.positionCount = 0;

        // Send points to MiVRy
        // MiVRyRecognizer.Instance.Recognize(points);
    }

    void AddPoint()
    {
        Vector3 tipPos = drawTip.position;

        if (points.Count == 0 || Vector3.Distance(points[^1], tipPos) > minDistance)
        {
            points.Add(tipPos);

            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPosition(points.Count - 1, tipPos);
        }
    }

    public List<Vector3> GetPoints()
    {
        return points;
    }
}
