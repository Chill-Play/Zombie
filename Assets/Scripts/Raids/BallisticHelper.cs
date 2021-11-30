using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BallisticData
{
    public float beginVelocity;
    public float endVelocity;
    public float acceleration;

    public BallisticData(float beginVelocity, float endVelocity, float acceleration)
    {
        this.beginVelocity = beginVelocity;
        this.endVelocity = endVelocity;
        this.acceleration = acceleration;
    }
}

public static class BallisticHelper 
{
    public static Vector3 CalculateVelocity(Vector3 begin, Vector3 end, float initialVelocity)
    {
        Vector3 distance = end - begin;
        Vector3 distanceXZ = distance.SetY(0f);

        float sY = distance.y;
        float sXZ = distance.magnitude;
        float time = sXZ / initialVelocity;
        float yVelocity = sY / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;
        Vector3 result = distanceXZ.normalized;
        result *= initialVelocity;
        result.y = yVelocity;

        return result;
    }

    public static Vector3 CalculateVelocityWithFixedY(Vector3 begin, Vector3 end, float initialVelocity, float y)
    {
        Vector3 distance = end - begin;
        Vector3 distanceXZ = distance.SetY(0f);

        float sY = distance.y;
        float sXZ = distance.magnitude;
        float time = sXZ / initialVelocity;
        float yVelocity = sY / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;
        result *= initialVelocity;
        result.y = yVelocity;

        return result;
    }

    public static Vector3 CalculatePositionInTime(Vector3 begin, Vector3 initialVelovity, float time)
    {       
        Vector3 result = begin + initialVelovity * time;

        float sY = (-0.5f * Mathf.Abs(Physics.gravity.y) * (time * time)) + (initialVelovity.y * time) + begin.y;
        result.y = sY;

        return result;
    }

    public static BallisticData CalculateLinearVelocity(float begin, float end, float endVelocity, float time)
    {
        float path = end - begin;
        float beginVelocity = 2f * path / time - endVelocity;
        float acceleration = (endVelocity - beginVelocity) / time;
        return new BallisticData(beginVelocity, endVelocity, acceleration);
    }

    public static BallisticData CalculateLinearVelocityByBeginVelocity(float begin, float end, float beginVelocity, float time)
    {
        float path = end - begin;
        float endVelocity = 2f * path / time - beginVelocity;
        float acceleration = (endVelocity - beginVelocity) / time;
        return new BallisticData(beginVelocity, endVelocity, acceleration);
    }

    public static float GetVelocityInTime(BallisticData ballisticData, float deltaTime)
    {
        return ballisticData.beginVelocity + ballisticData.acceleration * deltaTime;
    }
}
