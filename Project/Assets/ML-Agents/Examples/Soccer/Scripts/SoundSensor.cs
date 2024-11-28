using UnityEngine;
using Unity.MLAgents.Sensors;
using System.Collections;
using System.Collections.Generic;



public class SoundSensor :  MonoBehaviour,ISensor
{

    private string m_Name;
    // private ObservationWriter writer;
    private Vector4 allData;
    private ObservationSpec m_ObservationSpec;

    public SoundSensor(string name)
    {
        m_Name = name;
        m_ObservationSpec = ObservationSpec.Vector(4); // 1 for Time, 3 for Coordinates
        // this.writer  = new ObservationWriter();
    }

       public ObservationSpec GetObservationSpec()
    {
        return m_ObservationSpec;
    }

    public int Write(ObservationWriter writer)
    {
        Debug.Log("Writing Sound Data");
        // writer.Add(allData);  
        writer[0] = allData[0]; // Time
        writer[1] = allData[1]; // Coordinates.x
        writer[2] = allData[2]; // Coordinates.y
        writer[3] = allData[3]; // Coordinates.z
        return 4; // Total number of observations written  // Total number of observations written
    }



    public byte[] GetCompressedObservation()
    {
        return null; // No compression
    }

     public void Update() { }
     public void Reset() { }

    public  string GetName()
    {
        return m_Name;
    }
     public void SetSoundData(Vector4 soundData)
    {
        allData = soundData;
    }

    public Vector4 GetSoundData()
    {
        return allData;
    }

     void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sound"))
        {

            DiskBehavior collidedObject = other.GetComponent<DiskBehavior>();

            if ((collidedObject != null) && (collidedObject.getParentName() != transform.name))
            {
                float timeDelay = getTimeDelay(collidedObject);
                Vector3 soundPosition = collidedObject.transform.position;
                Vector4 allData = new Vector4(timeDelay, soundPosition.x, soundPosition.y, soundPosition.z);  
                this.allData = allData;
                //Write(writer);
            }
            else
            {
                Debug.LogError("The object does not have a DiskBehavior component.");
            }
        }
    }

     float getTimeDelay(DiskBehavior collidedObject)
    {
        float localTime = Time.time;
        float timeDelay = localTime - collidedObject.getCreationTime();
        return timeDelay;
    }

     (float, float, float) getSoundPropagationCoordinates(DiskBehavior collidedObject)
    {
        return (collidedObject.getXCoordinate(), collidedObject.getYCoordinate(), collidedObject.getZCoordinate());
    }

    public CompressionSpec GetCompressionSpec()
    {
        return CompressionSpec.Default();
    }
}