using UnityEngine;

public class SoccerBallController : MonoBehaviour
{
    public GameObject area;
    [HideInInspector]
    public SoccerEnvController envController;
    public string purpleGoalTag; // will be used to check if collided with purple goal
    public string blueGoalTag;   // will be used to check if collided with blue goal

    private Team lastTouchedTeam;

    void Start()
    {
        envController = area.GetComponent<SoccerEnvController>();
    }

    // Detect when the ball is touched by an agent
    void OnCollisionEnter(Collision col)
    {
        AgentSoccer agent = col.gameObject.GetComponent<AgentSoccer>();
        if (agent != null)
        {
            lastTouchedTeam = agent.team; // Record the team that last touched the ball
        }

        if (col.gameObject.CompareTag(purpleGoalTag)) // ball touched purple goal
        {
            bool isOwnGoal = (lastTouchedTeam == Team.Purple); // Check if it's an own goal
            envController.GoalTouched(Team.Blue, isOwnGoal);
        }

        if (col.gameObject.CompareTag(blueGoalTag)) // ball touched blue goal
        {
            bool isOwnGoal = (lastTouchedTeam == Team.Blue); // Check if it's an own goal
            envController.GoalTouched(Team.Purple, isOwnGoal);
        }
    }
}
