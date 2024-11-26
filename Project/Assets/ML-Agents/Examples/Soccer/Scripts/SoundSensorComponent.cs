using UnityEngine;
using Unity.MLAgents.Sensors;

[AddComponentMenu("ML Agents/Sound Sensor")]
public class SoundSensorComponent : SensorComponent
{
    private SoundSensor m_Sensor;
    // private SoundSensorData m_SoundData;

    public override ISensor[] CreateSensors()
    {
        m_Sensor = new SoundSensor("SoundSensor");
        return new ISensor[] { m_Sensor };
    }

    public void SetSoundData(SoundSensorData soundData)
    {
         m_Sensor.SetSoundData(soundData);
        // m_SoundData = soundData;
    }


    public SoundSensorData GetSoundData()
    {
        return m_Sensor.GetSoundData(m_Sensor);
    }
    }