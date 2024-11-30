using UnityEngine;
using Unity.MLAgents.Sensors;
using System.Collections;
using System.Collections.Generic;



public class SoundSensor :  MonoBehaviour,ISensor
{

    private string m_Name;
    // private ObservationWriter writer;
    private Vector3 coordonates;
    private Vector3 anglesAndTime;
     
    private ObservationSpec m_ObservationSpec;

    public SoundSensor(string name)
    {
        m_Name = name;
        m_ObservationSpec = ObservationSpec.Vector(6); // 1 for Time, 3 for Coordinates
        // this.writer  = new ObservationWriter();
    }

       public ObservationSpec GetObservationSpec()
    {
        return m_ObservationSpec;
    }

    public int Write(ObservationWriter writer)
    {
        // Debug.Log("Writing Sound Data");
        // writer.Add(allData);  
        writer[0] = coordonates[0]; // Coordinates.x
        writer[1] = coordonates[1]; // Coordinates.y
        writer[2] = coordonates[2]; // Coordinates.z
        writer[3] = anglesAndTime[0]; // Time
        writer[4] = anglesAndTime[1]; // Angle1
        writer[5] = anglesAndTime[2]; //Angle2

        
        return 6; // Total number of observations written  // Total number of observations written
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

    //Set and get are not used(functional)
     public void SetSoundData(Vector4 soundData)
    {
        // allData = soundData;
    }

    public Vector4 GetSoundData()
    {
        return new Vector4();
    }

     void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sound"))
        {

            DiskBehavior collidedObject = other.GetComponent<DiskBehavior>();

            if ((collidedObject != null) && (collidedObject.getParentName() != transform.name)){
                GameObject reciever1 = null;
                GameObject reciever2 = null;
                //Sound coordinates
                Vector3 soundPosition = collidedObject.transform.position;
                float timeDelay = getTimeDelay(collidedObject);

                reciever1 = transform.gameObject;
                if(transform.tag == "purpleAgent"){
                    if(transform.name == "PurpleStriker"){
                        reciever2 = GameObject.Find("PurpleStriker (1)");
                    }else{
                        reciever2 = GameObject.Find("PurpleStriker");
                    }
                }else if(transform.tag == "blueAgent"){
                    if(transform.name == "BlueStriker"){
                        reciever2 = GameObject.Find("BlueStriker (1)");
                    }else{
                        reciever2 = GameObject.Find("BlueStriker");
                    }
                }
                
                //Sound angles, time and coordinates
                var(angle1, angle2) = getAngles(reciever1,reciever2,soundPosition); 
                Vector3 anglesAndTime = new Vector3(timeDelay,angle1,angle2);
                print(angle1);
                print(angle2);
                this.coordonates = soundPosition;
                this.anglesAndTime = anglesAndTime;
                //Write(writer);
            }
            else
            {
                Debug.LogError("The object does not have a DiskBehavior component.");
            }
        }
    }


    (float,float) getAngles(GameObject objectA,GameObject objectB,Vector3 soundPosition){
        return (computeAngle(objectA, soundPosition), computeAngle(objectB, soundPosition));
    }

    float computeAngle(GameObject objectA, Vector3 diskCenter){

        Vector3 forwardA = objectA.transform.forward;
        forwardA.y = 0;

        Vector3 radius = objectA.transform.position - diskCenter;
        radius.y = 0;

        forwardA.Normalize();
        radius.Normalize();

        float angle = Vector3.Angle(forwardA, radius);
        return angle;
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