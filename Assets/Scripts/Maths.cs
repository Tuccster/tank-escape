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

    public static Quaternion VelocityToRotation(Vector3 velocity)
    {

        Vector3 rotation = new Vector3();
        rotation.y = Mathf.Tan(velocity.z / velocity.x);

        return Quaternion.Euler(rotation);
    }
}
