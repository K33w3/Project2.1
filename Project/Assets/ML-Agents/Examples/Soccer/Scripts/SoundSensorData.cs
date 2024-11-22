using UnityEngine;

public class SoundSensorData
{
    public float Time;
    public Vector3 Coordinates;

    public SoundSensorData(float time, Vector3 coordinates)
    {
        Time = time;
        Coordinates = coordinates;
    }
}