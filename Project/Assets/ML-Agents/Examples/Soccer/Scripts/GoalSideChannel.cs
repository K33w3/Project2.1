using Unity.MLAgents.SideChannels;
using UnityEngine;
using System;
using System.Text;

public class GoalSideChannel : SideChannel
{
    // Constructor to initialize the side channel with a unique ID
    public GoalSideChannel()
    {
        // Set the channel ID to a specific GUID
        ChannelId = new Guid("12345678-1234-1234-1234-1234567890ab");
    }

    // Send a goal update to Python based on which team scored
    public void SendGoalUpdate(Team scoredTeam)
    {
        using (var msg = new OutgoingMessage())
        {
            // Convert the team to a string and send it as a message
            string scoredTeamStr = scoredTeam == Team.Blue ? "Blue" : "Purple";
            msg.WriteString(scoredTeamStr);
            base.QueueMessageToSend(msg); // Corrected method for sending message
            
            // Debug log to confirm message sending
            Debug.Log($"Debug: Sent goal update for {scoredTeamStr} team.");
        }
    }

    // Method to receive messages if needed (empty here, as we only send messages)
    protected override void OnMessageReceived(IncomingMessage msg)
    {
        // Currently unused
    }
}
