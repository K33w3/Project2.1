using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Unity.MLAgents.Sensors;

public class RayPerceptionSensorShortend : RayPerceptionSensorComponentBase
{
    [HideInInspector, SerializeField, Range(0, 90)]
    private float limitedMaxRayDegrees = 45f;

    public override RayPerceptionCastType GetCastType()
    {
        return RayPerceptionCastType.Cast3D;
    }

    // Use 'new' keyword to hide the base property
    public new float MaxRayDegrees
    {
        get => limitedMaxRayDegrees;
        set
        {
            limitedMaxRayDegrees = Mathf.Clamp(value, 0, 45);
            UpdateSensor();
        }
    }

    // Create your custom GetRayAngles method
    private float[] GetCustomRayAngles(int raysPerDirection, float maxRayDegrees)
    {
        // Limit the maxRayDegrees to 45 degrees (left and right), resulting in a 90-degree FOV.
        maxRayDegrees = Mathf.Min(limitedMaxRayDegrees, 45f);

        // Calculate the ray angles for a 90-degree FOV.
        var anglesOut = new float[2 * raysPerDirection + 1];
        var delta = maxRayDegrees / raysPerDirection;

        for (int i = 0; i < 2 * raysPerDirection + 1; i++)
        {
            anglesOut[i] = 90 + (i - raysPerDirection) * delta;
        }

        return anglesOut;
    }

    // Use 'new' to hide GetRayPerceptionInput
    public new RayPerceptionInput GetRayPerceptionInput()
    {
        var rayAngles = GetCustomRayAngles(RaysPerDirection, MaxRayDegrees);

        var rayPerceptionInput = new RayPerceptionInput
        {
            RayLength = RayLength,
            DetectableTags = DetectableTags,
            Angles = rayAngles,
            StartOffset = GetStartVerticalOffset(),
            EndOffset = GetEndVerticalOffset(),
            CastRadius = SphereCastRadius,
            Transform = transform,
            CastType = GetCastType(),
            LayerMask = RayLayerMask,
            UseBatchedRaycasts = UseBatchedRaycasts
        };

        return rayPerceptionInput;
    }

    // Override CreateSensors to use your custom input
    public override ISensor[] CreateSensors()
    {
        var rayPerceptionInput = GetRayPerceptionInput();

        m_RaySensor = new RayPerceptionSensor(SensorName, rayPerceptionInput);

        if (ObservationStacks != 1)
        {
            var stackingSensor = new StackingSensor(m_RaySensor, ObservationStacks);
            return new ISensor[] { stackingSensor };
        }

        return new ISensor[] { m_RaySensor };
    }

    // Optionally override the Start and End Vertical Offsets if needed
    public override float GetStartVerticalOffset()
    {
        return StartVerticalOffset;
    }

    public override float GetEndVerticalOffset()
    {
        return EndVerticalOffset;
    }

    [SerializeField, Range(-10f, 10f)]
    private float StartVerticalOffset = 0f;

    [SerializeField, Range(-10f, 10f)]
    private float EndVerticalOffset = 0f;
}
