using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Sensors;

public class CustomRayPerceptionSensorComponent3D : RayPerceptionSensorComponent3D
{
    [SerializeField, Range(0, 180)]
    private float reducedMaxRayDegrees = 60f; // Reduced field of view

    [SerializeField]
    private List<string> detectableTags = new List<string>(); // Detectable tags

    [SerializeField]
    private float startOffset = 0.5f;

    [SerializeField]
    private float endOffset = 1.0f;

    public float ReducedMaxRayDegrees
    {
        get => reducedMaxRayDegrees;
        set
        {
            reducedMaxRayDegrees = Mathf.Clamp(value, 0, 180);
            UpdateSensor();
        }
    }

    public new List<string> DetectableTags
    {
        get => detectableTags;
        set
        {
            detectableTags = value;
            UpdateSensor();
        }
    }

    public float StartOffset
    {
        get => startOffset;
        set
        {
            startOffset = value;
            UpdateSensor();
        }
    }

    public float EndOffset
    {
        get => endOffset;
        set
        {
            endOffset = value;
            UpdateSensor();
        }
    }

    public override ISensor[] CreateSensors()
    {
        var rayPerceptionInput = GetRayPerceptionInput();
        rayPerceptionInput.Angles = GetRayAngles(RaysPerDirection, ReducedMaxRayDegrees);

        var raySensor = new RayPerceptionSensor(SensorName, rayPerceptionInput);

        if (ObservationStacks != 1)
        {
            var stackingSensor = new StackingSensor(raySensor, ObservationStacks);
            return new ISensor[] { stackingSensor };
        }

        return new ISensor[] { raySensor };
    }

    internal static new float[] GetRayAngles(int raysPerDirection, float maxRayDegrees)
    {
        var anglesOut = new List<float>();
        var delta = maxRayDegrees / raysPerDirection;

        for (var i = 0; i < 2 * raysPerDirection + 1; i++)
        {
            var angle = 90 + (i - raysPerDirection) * delta;
            if (angle > 135 && angle < 225) // Skip backward rays
            {
                continue;
            }
            anglesOut.Add(angle);
        }

        return anglesOut.ToArray();
    }


    public new RayPerceptionInput GetRayPerceptionInput()
    {
        return new RayPerceptionInput
        {
            RayLength = RayLength,
            DetectableTags = detectableTags, // Pass detectable tags here
            Angles = GetRayAngles(RaysPerDirection, ReducedMaxRayDegrees),
            StartOffset = StartOffset,
            EndOffset = EndOffset,
            CastRadius = SphereCastRadius,
            Transform = transform,
            CastType = RayPerceptionCastType.Cast3D,
            LayerMask = RayLayerMask,
            UseBatchedRaycasts = UseBatchedRaycasts
        };
    }
}
