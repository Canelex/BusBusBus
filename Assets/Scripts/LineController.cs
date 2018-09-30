using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineController : MonoBehaviour
{
    private Dictionary<int, Vector2> fingers;
    private AnimationCurve curveX;
    private AnimationCurve curveY;
    private int numKnots;
    public int maximumFingers;
    private Keyframe[] keysX;
    private Keyframe[] keysY;
    private Vector2[] knots;
    private Vector3[] smoothKnots;
    public int numSmoothKnots;
    private int numUsedKnots;
    public Vector2 startPoint;
    public Vector2 endPoint;
    private LineRenderer line;

    void Start()
    {
        fingers = new Dictionary<int, Vector2>();
        curveX = new AnimationCurve();
        curveY = new AnimationCurve();
        numKnots = maximumFingers + 2; // Number of fingers + 2 end points
        keysX = new Keyframe[numKnots];
        keysY = new Keyframe[numKnots];
        knots = new Vector2[numKnots];
        smoothKnots = new Vector3[numSmoothKnots];
        line = GetComponent<LineRenderer>();
        line.positionCount = numSmoothKnots;

        // Fix this lame bug where line looks funky until first update
        line.SetPosition(0, startPoint);
        for (int i = 1; i < numSmoothKnots; i++)
        {
            line.SetPosition(i, endPoint);
        }
    }

    public void UpdateLine()
    {
        RecordFingerInputs(); // Record touch points for line
        GenerateKnots(); // Convert touch points into vertices
        SmoothKnots(); // Interpolate more vertices for smoother effect (smoothKnots array)
        line.SetPositions(smoothKnots);
    }

    private void RecordFingerInputs()
    {
        foreach (Touch touch in Input.touches)
        {
            int fingerId = touch.fingerId; // Unique finger ID
            Vector2 position = touch.position;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (fingers.Count < maximumFingers) // Limit number of knots user can add.
                    {
                        fingers.Add(fingerId, position);
                    }
                    break;
                case TouchPhase.Ended:
                    if (fingers.ContainsKey(fingerId))
                    {
                        fingers.Remove(fingerId);
                    }
                    break;
                default:
                    if (fingers.ContainsKey(fingerId))
                    {
                        position.y = fingers[fingerId].y;
                        fingers[fingerId] = position;
                    }
                    break;
            }
        }

        numUsedKnots = fingers.Count + 2; // Fingers + 2 end points
    }

    private void GenerateKnots()
    {
        Vector2 camera = Camera.main.transform.position;
        knots[0] = camera + startPoint; // Start point

        int index = 1;
        foreach (Vector2 finger in fingers.Values) // Generate as many knots as you can with touch points
        {
            knots[index++] = Camera.main.ScreenToWorldPoint(finger);
        }

        while (index < numKnots) // Fill the rest (at least one) with end points.
        {
            knots[index++] = camera + endPoint;
        }

        Array.Sort(knots, CompareVectors); // Order knots bottom-up
    }

    private void SmoothKnots()
    {
        // Add every keyframe (including repeating endpoints).
        for (int i = 0; i < numKnots; i++)
        {
            keysX[i] = new Keyframe(i, knots[i].x);
            keysY[i] = new Keyframe(i, knots[i].y);
        }

        curveX.keys = keysX;
        curveY.keys = keysY;

        // Smooth every keyframe (including repeating endpoints);
        for (int i = 0; i < numKnots; i++)
        {
            curveX.SmoothTangents(i, 0);
            curveY.SmoothTangents(i, 0);
        }

        // Evaluate unique keyframes.
        for (int i = 0; i < numSmoothKnots; i++)
        {
            float t = (float)i / (numSmoothKnots - 1) * (numUsedKnots - 1); // [0.0, 1.0 * knotsUsed]
            smoothKnots[i] = new Vector3(curveX.Evaluate(t), curveY.Evaluate(t), 0);
        }
    }

    int CompareVectors(Vector2 a, Vector2 b)
    {
        if (a.y > b.y) return 1;
        if (a.y < b.y) return -1;
        return 0;
    }
}
