using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Maths
{
    public static bool IsBetween(float value, float min, float max)
    {
        if (value >= min && value <= max) return true;
        return false;
    }

    public static bool IsApproximately(float value, float targetValue, float threshold)
    {
        if (value >= targetValue - threshold && value <= targetValue + threshold) return true;
        return false;
    }

    public static Vector3 CalculateProjectileVelocity(Vector3 initPos, Vector3 target, float vertDisplacement, float gravity = -9.81f) // Sabastian Lague
    {
        float displacementY = target.y - initPos.y;
        Vector3 displancementXZ = new Vector3(target.x - initPos.x, 0, target.z - initPos.z);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * vertDisplacement);
        Vector3 velocityZX = displancementXZ / (Mathf.Sqrt(-2 * vertDisplacement / gravity) + Mathf.Sqrt(2 * (displacementY - vertDisplacement) / gravity));
        return velocityZX + velocityY;
    }

    public static float RemainingDistance(Vector3[] points)
    {
        if (points.Length < 2) return 0;
        float distance = 0;
        for (int i = 0; i < points.Length - 1; i++)
            distance += Vector3.Distance(points[i], points[i + 1]);
        return distance;
    }

    public static float RoundToInverval(float value, float interval)
    {
        return Mathf.Ceil(value / interval) * interval;
    }
}
