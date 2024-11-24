using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public enum Team
{
    Blue,
    Purple
}

public class AgentSoccer : Agent
{
    public SoundSensorComponent soundSensorComponent;
    public Team team;
    public float kickPower = 2000f;
    public float ballTouchReward = 0.2f;
    public Vector3 initialPos;
    public float rotSign;

    public override void Initialize()
    {
        base.Initialize();
        soundSensorComponent = GetComponent<SoundSensorComponent>();
        initialPos = transform.position;
        rotSign = 1.0f;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Base observations if any
        base.CollectObservations(sensor);
        // No need to manually add observations here since the SoundSensor will handle it
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SoundOnColide"))
        {
            DiskBehavior collidedObject = other.GetComponent<DiskBehavior>();

            if ((collidedObject != null) && (collidedObject.getParentName() != transform.name))
            {
                float timeDelay = GetTimeDelay(collidedObject);
                Vector3 soundCoords = GetSoundPropagationCoordinates(collidedObject);
                var soundData = new SoundSensorData(timeDelay, soundCoords);
                soundSensorComponent.SetSoundData(soundData);
            }
        }
    }

    private float GetTimeDelay(DiskBehavior collidedObject)
    {
        float localTime = Time.time;
        return localTime - collidedObject.getCreationTime();
    }

    private Vector3 GetSoundPropagationCoordinates(DiskBehavior collidedObject)
    {
        return collidedObject.transform.position;
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.CompareTag("ball"))
        {
            AddReward(ballTouchReward);
            var dir = c.contacts[0].point - transform.position;
            dir = dir.normalized;
            c.gameObject.GetComponent<Rigidbody>().AddForce(dir * kickPower);
        }
    }
}