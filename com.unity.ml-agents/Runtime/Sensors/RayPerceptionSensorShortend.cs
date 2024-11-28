using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Sensors;

public class RayPerceptionSensorShortend : RayPerceptionSensorComponentBase
{
    [HideInInspector, SerializeField, Range(0, 90)]
    private float limitedMaxRayDegrees = 45f;

    private RayPerceptionSensor customRaySensor;

    public override RayPerceptionCastType GetCastType()
    {
        return RayPerceptionCastType.Cast3D;
    }

    public new float MaxRayDegrees
    {
        get => limitedMaxRayDegrees;
        set
        {
            limitedMaxRayDegrees = Mathf.Clamp(value, 0, 45);
            UpdateSensor();
        }
    }

    [SerializeField, Range(1, 50)]
    private int raysPerDirection = 4; // Default is 4 rays per direction

    public new int RaysPerDirection
    {
        get => raysPerDirection;
        set
        {
            raysPerDirection = Mathf.Clamp(value, 1, 50); // Limit to a reasonable range
            UpdateSensor(); // Ensure sensor updates when value changes
        }
    }

    private float[] GetCustomRayAngles()
    {
        var maxRayDegrees = Mathf.Min(limitedMaxRayDegrees, 45f);
        var anglesOut = new float[2 * raysPerDirection + 1];
        var delta = maxRayDegrees / raysPerDirection;

        for (int i = 0; i < 2 * raysPerDirection + 1; i++)
        {
            anglesOut[i] = 90 + (i - raysPerDirection) * delta;
        }

        return anglesOut;
    }

    public new RayPerceptionInput GetRayPerceptionInput()
    {
        var rayAngles = GetCustomRayAngles();

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

    public override ISensor[] CreateSensors()
    {
        var rayPerceptionInput = GetRayPerceptionInput();

        customRaySensor = new RayPerceptionSensor(SensorName, rayPerceptionInput);

        if (ObservationStacks != 1)
        {
            var stackingSensor = new StackingSensor(customRaySensor, ObservationStacks);
            return new ISensor[] { stackingSensor };
        }

        return new ISensor[] { customRaySensor };
    }

    public override float GetStartVerticalOffset()
    {
        return StartVerticalOffset;
    }

    public override float GetEndVerticalOffset()
    {
        return EndVerticalOffset;
    }

    [SerializeField, Range(-10f, 10f)]
    private float StartVerticalOffset = 0.5f;

    [SerializeField, Range(-10f, 10f)]
    private float EndVerticalOffset = -0.3f;
}
