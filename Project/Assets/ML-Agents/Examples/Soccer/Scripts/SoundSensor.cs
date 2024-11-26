using UnityEngine;
using Unity.MLAgents.Sensors;

public class SoundSensor : ISensor
{
    private SoundSensorData m_SoundData;
    private string m_Name;
    private ObservationSpec m_ObservationSpec;

    public SoundSensor(string name)
    {
        m_Name = name;
        m_ObservationSpec = ObservationSpec.Vector(4); // 1 for Time, 3 for Coordinates
    }

    public void SetSoundData(SoundSensorData soundData)
    {
        m_SoundData = soundData;
    }

    public SoundSensorData GetSoundData(SoundSensor sound)
    {
        return sound.m_SoundData;
    }

    public ObservationSpec GetObservationSpec()
    {
        return m_ObservationSpec;
    }

    public int Write(ObservationWriter writer)
    {
        writer[0] = m_SoundData.Time;                 // Assign Time at index 0
        writer[1] = m_SoundData.Coordinates.x;        // Assign Coordinates.x at index 1
        writer[2] = m_SoundData.Coordinates.y;        // Assign Coordinates.y at index 2
        writer[3] = m_SoundData.Coordinates.z;        // Assign Coordinates.z at index 3
        return 4; // Total number of observations written
    }

    public byte[] GetCompressedObservation()
    {
        return null;
    }

    public void Update() { }

    public void Reset() { }

    public string GetName()
    {
        return m_Name;
    }

    public CompressionSpec GetCompressionSpec()
    {
        return CompressionSpec.Default();
    }
}