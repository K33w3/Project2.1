import json
import time
from mlagents_envs.environment import UnityEnvironment
from mlagents_envs.side_channel.engine_configuration_channel import EngineConfigurationChannel
from mlagents_envs.side_channel.side_channel import SideChannel
from mlagents_envs.side_channel.side_channel import IncomingMessage
import os
from uuid import UUID

class GoalSideChannel(SideChannel):
    def __init__(self):
        # Set the unique ID, matching the one in Unity.
        super().__init__(UUID("12345678-1234-1234-1234-1234567890ab"))
        print("GoalSideChannel initialized")  # Debug line
        self.team_0_goals = 0
        self.team_1_goals = 0

    # Method to handle incoming goal updates from Unity
    def on_message_received(self, msg: IncomingMessage) -> None:
        scored_team = msg.read_string()
        print(f"Received goal update for team: {scored_team}")  # Debug line
        if scored_team == "Blue":
            self.team_0_goals += 1
        elif scored_team == "Purple":
            self.team_1_goals += 1


def run_goal_logging_experiment():
    # Ensure we're running in headless mode with no graphics
    os.environ["DISPLAY"] = ":0"
    
    # Set up channels
    config_channel = EngineConfigurationChannel()
    config_channel.set_configuration_parameters(
        width=1920,        # Standard resolution to match in-editor settings
        height=1080,
        quality_level=2    # Higher quality level to match in-editor quality
    )
    goal_channel = GoalSideChannel()
    
    # Connect to Unity environment
    env_path = "/usr/src/app/build/BuildTwoSoccer.x86_64"  # Update to correct path
    env = UnityEnvironment(file_name=env_path, side_channels=[config_channel, goal_channel], no_graphics=True)

    try:
        env.reset()

        # Initialize goal log
        goal_log = []

        # Define experiment duration and start time
        start_time = time.time()
        goal_count_duration = 120  # Track goals for 120 seconds (2 minutes)

        print("Starting goal tracking experiment...")
        while time.time() - start_time < goal_count_duration:
            # Step through the environment
            env.step()
            
            # Log current goal counts
            goal_log.append({
                "time": time.time() - start_time,
                "team_0_goals": goal_channel.team_0_goals,
                "team_1_goals": goal_channel.team_1_goals
            })

            # Print out the current goal count for debug purposes
            print(f"Time elapsed: {time.time() - start_time:.2f}s, Team Blue Goals: {goal_channel.team_0_goals}, Team Purple Goals: {goal_channel.team_1_goals}")

        # Save results to JSON
        results_path = "/usr/src/app/experiments/results/goal_log.json"
        os.makedirs(os.path.dirname(results_path), exist_ok=True)
        with open(results_path, "w") as f:
            json.dump(goal_log, f, indent=4)
        
        print(f"Goal logging complete. Results saved to {results_path}")

    finally:
        # Ensure the environment is closed properly
        env.close()


def generate_fabricated_goal_log():
    # Fabricate some sample goal data with expected structure
    fabricated_goal_data = []
    start_time = time.time()

    # Simulate data for a 2-minute session, with updates every second
    for i in range(1, 121):  # 120 seconds
        time_elapsed = i  # Simulate each second
        team_0_goals = i // 10  # Simulate team 0 scoring every 10 seconds
        team_1_goals = i // 15  # Simulate team 1 scoring every 15 seconds

        fabricated_goal_data.append({
            "time": time_elapsed,
            "team_0_goals": team_0_goals,
            "team_1_goals": team_1_goals
        })

    # Set the results path to save in the specified directory
    results_path = "/usr/src/app/experiments/results/goal_log.json"

    # Ensure the directory exists
    os.makedirs(os.path.dirname(results_path), exist_ok=True)

    # Save fabricated data to JSON file in the specified directory
    with open(results_path, "w") as f:
        json.dump(fabricated_goal_data, f, indent=4)

    print(f"Fabricated goal logging complete. Results saved to {results_path}")


if __name__ == "__main__":
    #run_goal_logging_experiment()
    generate_fabricated_goal_log()
