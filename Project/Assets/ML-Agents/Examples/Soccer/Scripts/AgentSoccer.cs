using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
public enum Team
{
    Blue = 0,
    Purple = 1
}

public enum Position
{
    Striker,
    Goalie,
    Generic
}

public class AgentSoccer : Agent
{
    [HideInInspector]
    public Team team;
    public Position position;

    const float k_Power = 2000f;
    float m_KickPower;
    float m_BallTouch;
    float m_Existential;
    float m_LateralSpeed;
    float m_ForwardSpeed;

    public Rigidbody agentRb;
    SoccerSettings m_SoccerSettings;
    BehaviorParameters m_BehaviorParameters;
    public Vector3 initialPos;
    public float rotSign;

    EnvironmentParameters m_ResetParams;

    // Sound Sensor Data
    private SoundSensorData m_SoundData;
    public SoundSensorComponent soundSensorComponent;

    public override void Initialize()
    {
        SoccerEnvController envController = GetComponentInParent<SoccerEnvController>();
        if (envController != null)
        {
            m_Existential = 1f / envController.MaxEnvironmentSteps;
        }
        else
        {
            m_Existential = 1f / MaxStep;
        }

        m_BehaviorParameters = GetComponent<BehaviorParameters>();
        if (m_BehaviorParameters.TeamId == (int)Team.Blue)
        {
            team = Team.Blue;
            initialPos = new Vector3(transform.position.x - 5f, 0.5f, transform.position.z);
            rotSign = 1f;
        }
        else
        {
            team = Team.Purple;
            initialPos = new Vector3(transform.position.x + 5f, 0.5f, transform.position.z);
            rotSign = -1f;
        }

        // Set movement speeds based on position
        switch (position)
        {
            case Position.Goalie:
                m_LateralSpeed = 1.0f;
                m_ForwardSpeed = 1.0f;
                break;
            case Position.Striker:
                m_LateralSpeed = 0.3f;
                m_ForwardSpeed = 1.3f;
                break;
            default:
                m_LateralSpeed = 0.3f;
                m_ForwardSpeed = 1.0f;
                break;
        }

        m_SoccerSettings = FindObjectOfType<SoccerSettings>();
        agentRb = GetComponent<Rigidbody>();
        agentRb.maxAngularVelocity = 500f;

        m_ResetParams = Academy.Instance.EnvironmentParameters;

        soundSensorComponent = GetComponent<SoundSensorComponent>();
        if(soundSensorComponent == null)
        {
            Debug.LogError("SoundSensorCompoennt is missing on the agent. ");
        }
    
    }

     public override void CollectObservations(VectorSensor sensor)
    {
        if (sensor == null)
        {
            Debug.LogError("VectorSensor is null in CollectObservations.");
            return;
        }

        if (transform == null)
        {
            Debug.LogError("Transform is null in CollectObservations.");
            return;
        }

        // Agent's local position
        sensor.AddObservation(transform.localPosition);

        // Agent's velocity
        if (agentRb != null)
        {
            sensor.AddObservation(agentRb.velocity);
        }
        else
        {
            Debug.LogError("Rigidbody component is missing on agent.");
            sensor.AddObservation(Vector3.zero);
        }

        // Ball's position
        GameObject ball = GameObject.FindGameObjectWithTag("SoundOnColide");
        if (ball != null)
        {
            sensor.AddObservation(ball.transform.localPosition);
        }
        else
        {
            Debug.LogError("Ball tagged with 'SoundOnColide' not found in the scene.");
            sensor.AddObservation(Vector3.zero);
        }

        // Team information
        sensor.AddObservation((int)team);

        // Sound data observations
        if (soundSensorComponent != null)
        {
            var soundData = soundSensorComponent.GetSoundData();
            if (soundData != null)
            {
                // Normalize the time (assuming max time of 10 seconds)
                sensor.AddObservation(soundData.Time / 10f);
                // Normalize position (assuming play area is within -50 to 50 units)
                sensor.AddObservation(soundData.Coordinates / 50f);
            }
            else
            {
                // No sound detected
                sensor.AddObservation(0f);
                sensor.AddObservation(Vector3.zero);
            }
        }
        else
        {
            Debug.LogError("SoundSensorComponent is null in CollectObservations.");
        }
    }

    public void ReceiveSoundData(SoundSensorData soundData)
    {
        m_SoundData = soundData;
    }

    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        m_KickPower = 0f;

        var forwardAxis = act[0];
        var rightAxis = act[1];
        var rotateAxis = act[2];

        switch (forwardAxis)
        {
            case 1:
                dirToGo = transform.forward * m_ForwardSpeed;
                m_KickPower = 1f;
                break;
            case 2:
                dirToGo = transform.forward * -m_ForwardSpeed;
                break;
        }

        switch (rightAxis)
        {
            case 1:
                dirToGo += transform.right * m_LateralSpeed;
                break;
            case 2:
                dirToGo += transform.right * -m_LateralSpeed;
                break;
        }

        switch (rotateAxis)
        {
            case 1:
                rotateDir = transform.up * -1f;
                break;
            case 2:
                rotateDir = transform.up * 1f;
                break;
        }

        transform.Rotate(rotateDir, Time.deltaTime * 100f);
        agentRb.AddForce(dirToGo * m_SoccerSettings.agentRunSpeed, ForceMode.VelocityChange);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        if (position == Position.Goalie)
        {
            // Existential bonus for Goalies.
            AddReward(m_Existential);
        }
        else if (position == Position.Striker)
        {
            // Existential penalty for Strikers.
            AddReward(-m_Existential);
        }
        MoveAgent(actionBuffers.DiscreteActions);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        // Forward/Backward
        if (Input.GetKey(KeyCode.W))
            discreteActionsOut[0] = 1;
        else if (Input.GetKey(KeyCode.S))
            discreteActionsOut[0] = 2;
        else
            discreteActionsOut[0] = 0;

        // Left/Right
        if (Input.GetKey(KeyCode.E))
            discreteActionsOut[1] = 1;
        else if (Input.GetKey(KeyCode.Q))
            discreteActionsOut[1] = 2;
        else
            discreteActionsOut[1] = 0;

        // Rotate
        if (Input.GetKey(KeyCode.A))
            discreteActionsOut[2] = 1;
        else if (Input.GetKey(KeyCode.D))
            discreteActionsOut[2] = 2;
        else
            discreteActionsOut[2] = 0;
    }

    void OnCollisionEnter(Collision c)
    {
        var force = k_Power * m_KickPower;
        if (position == Position.Goalie)
        {
            force = k_Power;
        }
        if (c.gameObject.CompareTag("ball"))
        {
            AddReward(0.2f * m_BallTouch);
            var dir = c.contacts[0].point - transform.position;
            dir = dir.normalized;
            c.gameObject.GetComponent<Rigidbody>().AddForce(dir * force);
        }

        // Generate sound data when the ball hits a wall
        if (c.gameObject.CompareTag("SoundOnColide"))
        {
            float timeDelay = Time.time;
            Vector3 soundCoords = c.contacts[0].point;
            var soundData = new SoundSensorData(timeDelay, soundCoords);
            ReceiveSoundData(soundData);
        }
    }

    public override void OnEpisodeBegin()
    {
        m_BallTouch = m_ResetParams.GetWithDefault("ball_touch", 0);
        m_SoundData = null; // Reset sound data at the beginning of the episode
    }
}