// using System.Collections.Generic;
// using UnityEngine;
// using Unity.MLAgents.Sensors;

// public class CustomRayPerceptionSensorComponent : SensorComponent
// {
//     [HideInInspector, SerializeField]
//     private string sensorName = "CustomRayPerceptionSensor";

//     [SerializeField]
//     private List<string> detectableTags;

//     [SerializeField, Range(0, 50)]
//     private int raysPerDirection = 3;

//     [SerializeField, Range(0, 45)]
//     private float maxRayDegrees = 45f;

//     [SerializeField, Range(0f, 10f)]
//     private float sphereCastRadius = 0.5f;

//     [SerializeField, Range(1, 1000)]
//     private float rayLength = 20f;

//     [SerializeField]
//     private LayerMask rayLayerMask = -5;

//     [SerializeField, Range(1, 50)]
//     private int observationStacks = 1;

//     [SerializeField]
//     private bool useBatchedRaycasts = false;

//     [SerializeField, Range(-10f, 10f)]
//     private float startVerticalOffset = 0f;

//     [SerializeField, Range(-10f, 10f)]
//     private float endVerticalOffset = 0f;

//     private RayPerceptionSensor m_RaySensor;

//     public override ISensor[] CreateSensors()
//     {
//         var rayPerceptionInput = GetCustomRayPerceptionInput();

//         m_RaySensor = new RayPerceptionSensor(sensorName, rayPerceptionInput);

//         if (observationStacks > 1)
//         {
//             var stackingSensor = new StackingSensor(m_RaySensor, observationStacks);
//             return new ISensor[] { stackingSensor };
//         }

//         return new ISensor[] { m_RaySensor };
//     }

//     private RayPerceptionInput GetCustomRayPerceptionInput()
//     {
//         var rayAngles = GetCustomRayAngles(raysPerDirection, maxRayDegrees);

//         var rayPerceptionInput = new RayPerceptionInput
//         {
//             RayLength = rayLength,
//             DetectableTags = detectableTags,
//             Angles = rayAngles,
//             StartOffset = startVerticalOffset,
//             EndOffset = endVerticalOffset,
//             CastRadius = sphereCastRadius,
//             Transform = transform,
//             CastType = RayPerceptionCastType.Cast3D,
//             LayerMask = rayLayerMask,
//             UseBatchedRaycasts = useBatchedRaycasts
//         };

//         return rayPerceptionInput;
//     }

//     private float[] GetCustomRayAngles(int raysPerDirection, float maxRayDegrees)
//     {
//         // Custom implementation to limit the FOV to 90 degrees
//         maxRayDegrees = Mathf.Min(maxRayDegrees, 45f);

//         var anglesOut = new float[2 * raysPerDirection + 1];
//         var delta = maxRayDegrees / raysPerDirection;

//         for (int i = 0; i < 2 * raysPerDirection + 1; i++)
//         {
//             anglesOut[i] = 90 + (i - raysPerDirection) * delta;
//         }

//         return anglesOut;
//     }
// }
