using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using Unity.MLAgents.SideChannels;
using System;

public class SoccerEnvController : MonoBehaviour
{
    [System.Serializable]
    public class PlayerInfo
    {
        public AgentSoccer Agent;
        [HideInInspector]
        public Vector3 StartingPos;
        [HideInInspector]
        public Quaternion StartingRot;
        [HideInInspector]
        public Rigidbody Rb;
    }

    public int MaxEnvironmentSteps = 25000;
    public GameObject ball;
    [HideInInspector]
    public Rigidbody ballRb;
    Vector3 m_BallStartingPos;

    public List<PlayerInfo> AgentsList = new List<PlayerInfo>();

    private SoccerSettings m_SoccerSettings;
    private SimpleMultiAgentGroup m_BlueAgentGroup;
    private SimpleMultiAgentGroup m_PurpleAgentGroup;
    private int m_ResetTimer;

    private static GoalSideChannel goalSideChannel;

    void Start()
    {
        // Ensure side channel is only registered once
        if (goalSideChannel == null)
        {
            goalSideChannel = new GoalSideChannel();
            SideChannelManager.RegisterSideChannel(goalSideChannel);
            Debug.Log("GoalSideChannel successfully registered.");
        }

        m_SoccerSettings = FindObjectOfType<SoccerSettings>();
        m_BlueAgentGroup = new SimpleMultiAgentGroup();
        m_PurpleAgentGroup = new SimpleMultiAgentGroup();
        ballRb = ball.GetComponent<Rigidbody>();
        m_BallStartingPos = ball.transform.position;

        foreach (var item in AgentsList)
        {
            item.StartingPos = item.Agent.transform.position;
            item.StartingRot = item.Agent.transform.rotation;
            item.Rb = item.Agent.GetComponent<Rigidbody>();

            if (item.Agent.team == Team.Blue)
            {
                m_BlueAgentGroup.RegisterAgent(item.Agent);
            }
            else
            {
                m_PurpleAgentGroup.RegisterAgent(item.Agent);
            }
        }

        ResetScene();
    }

    void FixedUpdate()
    {
        m_ResetTimer += 1;
        if (m_ResetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            m_BlueAgentGroup.GroupEpisodeInterrupted();
            m_PurpleAgentGroup.GroupEpisodeInterrupted();
            ResetScene();
        }
    }

    public void GoalTouched(Team scoredTeam)
{
     Debug.Log($"[GoalTouched] Goal detected for team {scoredTeam}.");
    if (scoredTeam == Team.Blue)
    {
        m_BlueAgentGroup.AddGroupReward(1 - (float)m_ResetTimer / MaxEnvironmentSteps);
        m_PurpleAgentGroup.AddGroupReward(-1);
    }
    else
    {
        m_PurpleAgentGroup.AddGroupReward(1 - (float)m_ResetTimer / MaxEnvironmentSteps);
        m_BlueAgentGroup.AddGroupReward(-1);
    }

    // Send goal update through the side channel
    goalSideChannel.SendGoalUpdate(scoredTeam);

    // Debug log to confirm goal detection
    Debug.Log($"Debug: Goal detected for team {scoredTeam}. Side channel message sent.");

    m_PurpleAgentGroup.EndGroupEpisode();
    m_BlueAgentGroup.EndGroupEpisode();
    ResetScene();
}

    public void ResetScene()
    {
        m_ResetTimer = 0;
        foreach (var item in AgentsList)
        {
            var randomPosX = UnityEngine.Random.Range(-5f, 5f);
            var newStartPos = item.Agent.initialPos + new Vector3(randomPosX, 0f, 0f);
            var rot = item.Agent.rotSign * UnityEngine.Random.Range(80.0f, 100.0f);
            var newRot = Quaternion.Euler(0, rot, 0);
            item.Agent.transform.SetPositionAndRotation(newStartPos, newRot);
            item.Rb.velocity = Vector3.zero;
            item.Rb.angularVelocity = Vector3.zero;
        }

        ResetBall();
    }

    public void ResetBall()
    {
        var randomPosX = UnityEngine.Random.Range(-2.5f, 2.5f);
        var randomPosZ = UnityEngine.Random.Range(-2.5f, 2.5f);
        ball.transform.position = m_BallStartingPos + new Vector3(randomPosX, 0f, randomPosZ);
        ballRb.velocity = Vector3.zero;
        ballRb.angularVelocity = Vector3.zero;
    }
}
