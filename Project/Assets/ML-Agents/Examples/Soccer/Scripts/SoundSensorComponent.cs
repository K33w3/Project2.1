using UnityEngine;
using Unity.MLAgents.Sensors;

[AddComponentMenu("ML Agents/Sound Sensor Component")]
public class SoundSensorComponent : SensorComponent
{
    private SoundSensor m_SoundSensor;

    public override ISensor[] CreateSensors()
    {
        m_SoundSensor = new SoundSensor("SoundSensor");
        return new ISensor[] { m_SoundSensor };
    }

     public void SetSoundData(Vector4 soundData)
    {
        if (m_SoundSensor != null)
        {
            m_SoundSensor.SetSoundData(soundData);
        }
    }

    public Vector4 GetSoundData()
    {
        return m_SoundSensor != null ? m_SoundSensor.GetSoundData() : Vector4.zero;
    }

}