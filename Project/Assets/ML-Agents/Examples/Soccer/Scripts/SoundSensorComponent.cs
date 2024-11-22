using UnityEngine;
using Unity.MLAgents.Sensors;

[AddComponentMenu("ML Agents/Sound Sensor")]
public class SoundSensorComponent : SensorComponent
{
    private SoundSensor m_Sensor;

    public override ISensor[] CreateSensors()
    {
        m_Sensor = new SoundSensor("SoundSensor");
        return new ISensor[] { m_Sensor };
    }

    public void SetSoundData(SoundSensorData soundData)
    {
        m_Sensor.SetSoundData(soundData);
    }
}