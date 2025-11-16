using System.Collections.Generic;
using UnityEngine;

public class GestureTrail : MonoBehaviour
{
    [Header("Drawing - Right Hand")]
    public Transform rightDrawTip;
    public LineRenderer rightLineRenderer;

    [Header("Drawing - Left Hand")]
    public Transform leftDrawTip;
    public LineRenderer leftLineRenderer;

    [Header("Settings")]
    public float minDistance = 0.015f;

    private bool isDrawingRight = false;
    private bool isDrawingLeft = false;

    private List<Vector3> rightPoints = new List<Vector3>();
    private List<Vector3> leftPoints = new List<Vector3>();

    void Update()
    {
        // RIGHT HAND
        bool rightTriggerDown = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        bool rightTriggerUp = OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);

        // Only allow right hand to start if left hand isn't already drawing
        if (rightTriggerDown && !isDrawingLeft)
        {
            StartDrawing(ref isDrawingRight, rightPoints, rightLineRenderer);
        }
        if (rightTriggerUp)
        {
            StopDrawing(ref isDrawingRight, rightPoints, rightLineRenderer, true);
        }
        if (isDrawingRight)
        {
            AddPoint(rightDrawTip.position, rightPoints, rightLineRenderer);
        }

        // LEFT HAND
        bool leftTriggerDown = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
        bool leftTriggerUp = OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch);

        // Only allow left hand to start if right hand isn't already drawing
        if (leftTriggerDown && !isDrawingRight)
        {
            StartDrawing(ref isDrawingLeft, leftPoints, leftLineRenderer);
        }
        if (leftTriggerUp)
        {
            StopDrawing(ref isDrawingLeft, leftPoints, leftLineRenderer, false);
        }
        if (isDrawingLeft)
        {
            AddPoint(leftDrawTip.position, leftPoints, leftLineRenderer);
        }
    }

    void StartDrawing(ref bool isDrawing, List<Vector3> points, LineRenderer lr)
    {
        isDrawing = true;
        points.Clear();
        lr.positionCount = 0;
    }

    void StopDrawing(ref bool isDrawing, List<Vector3> points, LineRenderer lr, bool isRightHand)
    {
        isDrawing = false;
        lr.positionCount = 0;
    }

    void AddPoint(Vector3 tipPos, List<Vector3> points, LineRenderer lr)
    {
        if (points.Count == 0 || Vector3.Distance(points[^1], tipPos) > minDistance)
        {
            points.Add(tipPos);
            lr.positionCount = points.Count;
            lr.SetPosition(points.Count - 1, tipPos);
        }
    }

    public List<Vector3> GetRightPoints() => rightPoints;
    public List<Vector3> GetLeftPoints() => leftPoints;
}