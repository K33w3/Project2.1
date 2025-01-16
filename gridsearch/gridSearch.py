import csv
import subprocess
import yaml
from mlagents_envs.environment import UnityEnvironment

# if you want to run ppo or sac just call the coresponding method, with the chosen environment as a parameter


ppo_param_grid = {
    'learning_rate': [0.0001, 0.0005, 0.001, 0.003, 0.01],
    'gamma': [0.9, 0.95, 0.99],
    'batch_size': [64, 128, 256],
    'hidden_units': [64, 128, 256],
    'num_epoch': [3, 5, 10],
    'lambd': [0.9, 0.95, 0.98]
}



# Hyperparameter grid for SAC
sac_param_grid = {
    'learning_rate': [0.0001, 0.0005, 0.001, 0.003, 0.01],
    'gamma': [0.9, 0.95, 0.99],
    'batch_size': [64, 128, 256],
    'hidden_units': [64, 128, 256],
    'init_entcoef': [0.1, 0.2, 0.5, 1.0],
    'buffer_size': [1e5, 5e5, 1e6],
}


def train_ml_agents(config_path, run_id, environment_name, reward_threshold=None):
    """
    Train Unity ML-Agents and monitor average reward.

    Args:
        config_path (str): Path to the ML-Agents YAML configuration file.
        run_id (str): Unique identifier for this training run.
        environment_name (str): Path to the Unity environment executable.
        max_episodes (int): Maximum number of episodes to train.
        reward_threshold (float): Reward threshold to stop training early.

    Returns:
        float: Final average reward calculated from the last 100 episodes.
    """
    # Prepare the command
    command = [
        "mlagents-learn",
        config_path,
        "--run-id", run_id,
        "--env", environment_name,
        "--train"
    ]

    # Run the command
    process = subprocess.Popen(command, stdout=subprocess.PIPE, stderr=subprocess.STDOUT, text=True)

    # Monitor training progress
    cumulative_rewards = []
    try:
        for line in iter(process.stdout.readline, ""):
            print(line, end="")  # Print log output in real-time

            # Parse average reward from TensorBoard logs
            if "Cumulative Reward Mean" in line:
                try:
                    reward = float(line.split(":")[-1].strip())
                    cumulative_rewards.append(reward)

                    # Check for reward threshold
                    if reward_threshold and reward >= reward_threshold:
                        print(f"Reward threshold {reward_threshold} reached. Stopping training.")
                        process.terminate()
                        break

                except ValueError:
                    continue

            # # Check episode limit
            # if len(cumulative_rewards) >= max_episodes:
            #     print(f"Maximum episodes {max_episodes} reached. Stopping training.")
            #     process.terminate()
            #     break

    except KeyboardInterrupt:
        print("Training interrupted manually.")
        process.terminate()

    # Wait for process to finish
    process.wait()

    # Calculate and return final average reward from the last 100 episodes
    if cumulative_rewards:
        avg_reward = sum(cumulative_rewards[-100:]) / len(cumulative_rewards[-100:])
        print(f"Final Average Reward (Last 100 Episodes): {avg_reward:.2f}")
        return avg_reward
    else:
        print("No rewards collected.")
        return None

def ppo_grid_search(environment_name,param_grid=ppo_param_grid):

    config_path = ""
    ppo_result_file_path = ""

    if(environment_name == "Crawler"):
        config_path = "./config/ppo/Crawler.yaml"
        ppo_result_file_path = "ppo_crawler_data_path.csv"
    elif (environment_name == "PushBlock"):
        config_path = "./config/ppo/PushBlock.yaml"
        ppo_result_file_path = "ppo_pushblock_data_path.csv"
    
    # Track results
    algorithm_name = "PPO"
    results = []
    counter = 1
    # Perform grid search
    for lr in param_grid['learning_rate']:
        for gamma in param_grid['gamma']:
            for batch in param_grid['batch_size']:
                for units in param_grid['hidden_units']:
                    for ne in param_grid['num_epoch']:
                        for gl in param_grid['lambd']:

                            change_yaml_file(lr,gamma,batch,units,ne,gl,config_path,environment_name,algorithm_name)

                            avg_reward = train_ml_agents(
                                config_path,
                                "ppo"+str(counter)
                                ,
                                environment_name
                            )
                            
                            # Log results
                            results.append({
                                'learning_rate': lr,
                                'gamma': gamma,
                                'batch_size': batch,
                                'hidden_units': units,
                                'num_epoch': ne,
                                'lambd': gl,
                                'avg_reward': avg_reward,
                                'configuration_number': counter
                            })
                            counter+=1


    # Sort the results from best to worst based on 'avg_reward'
    sorted_results = sorted(results, key=lambda x: x['avg_reward'], reverse=True)

    store_results(ppo_result_file_path,sorted_results)

    print(sorted_results)

    # Return results
    return sorted_results




def sac_grid_search(environment_name,param_grid=sac_param_grid):

    config_path = "./config/sac/Crawler.yaml"
    sac_result_file_path = ""

    if(environment_name == "Crawler"):
        config_path = "./config/sac/Crawler.yaml"
        sac_result_file_path = "sac_crawler_data_path.csv"
    elif (environment_name == "PushBlock"):
        config_path = "./config/sac/PushBlock.yaml"
        sac_result_file_path = "sac_pushblock_data_path.csv"

    # Track results
    results = []
    counter = 1
    algorithm_name = "SAC"
    # Perform grid search
    for lr in param_grid['learning_rate']:
        for gamma in param_grid['gamma']:
            for batch in param_grid['batch_size']:
                for units in param_grid['hidden_units']:
                    for temp in param_grid['init_entcoef']:
                        for rbs in param_grid['buffer_size']:

                            change_yaml_file(lr,gamma,batch,units,temp,rbs,config_path,environment_name,algorithm_name)

                            avg_reward = train_ml_agents(
                                config_path,
                                "sac"+str(counter),
                                environment_name
                            )

                            results.append({
                                'learning_rate': lr,
                                'gamma': gamma,
                                'batch_size': batch,
                                'hidden_units': units,
                                'init_entcoef': temp,
                                'buffer_size': rbs,
                                'avg_reward': avg_reward,
                                'configuration_number': counter
                            })
                            counter+=1


    # Sort the results from best to worst based on 'avg_reward'
    sorted_results = sorted(results, key=lambda x: x['avg_reward'], reverse=True)

    store_results(sac_result_file_path,sorted_results)
    # Print the sorted results
    print(sorted_results)

    # Return results
    return sorted_results


def change_yaml_file(learning_rate,gamma,batch_size,hidden_units,param5,param6,path,environment_name,algorithm_name):# crawler    ,path="./config/ppo/Crawler.yaml"

    param5_name = ""
    param6_name = ""

    if algorithm_name == "PPO":
        param5_name = "num_epoch"
        param6_name = "lambd"
    elif algorithm_name == "SAC":
        param5_name = "init_entcoef"
        param6_name = "buffer_size"
    
    print(algorithm_name)
    
    # Load the YAML file
    with open(path, "r") as file:
        data = yaml.safe_load(file)

    # Update the data
    data['behaviors'][environment_name]['max_steps'] = 1000
    data['behaviors'][environment_name]['hyperparameters']['learning_rate'] = learning_rate
    data['behaviors'][environment_name]['reward_signals']['extrinsic']['gamma'] = gamma
    data['behaviors'][environment_name]['hyperparameters']['batch_size'] = batch_size
    data['behaviors'][environment_name]['network_settings']['hidden_units'] = hidden_units
    if param5_name == "":
        print("Parameters 5 and 6, empty")

    data['behaviors'][environment_name]['hyperparameters'][param5_name] = param5
    data['behaviors'][environment_name]['hyperparameters'][param6_name] = param6

    # Write the updated data back to the YAML file
    with open(path, "w") as file:
        yaml.safe_dump(data, file, default_flow_style=False)




def store_results(result_store_file,data):
    with open(result_store_file, mode='w', newline='') as file:
        writer = csv.DictWriter(file, fieldnames=data[0].keys())
        writer.writeheader()
        writer.writerows(data)



ppo_grid_search("Crawler")
# sac_grid_search("Crawler")
# ppo_grid_search("PushBlock")
# sac_grid_search("PushBlock")
