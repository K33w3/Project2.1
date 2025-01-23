# Experiments Guide: PPO vs. SAC in Unity ML-Agents

This guide provides details on the experiments conducted to compare the performance and computational trade-offs of **Proximal Policy Optimization (PPO)** and **Soft Actor-Critic (SAC)** algorithms in the Unity ML-Agents Toolkit. The experiments were conducted in two environments: **Crawler** (continuous action space) and **Push Block** (discrete action space).

---

## Environments

### 1. **Crawler Environment**
- **Description**: A multi-limbed agent learns to navigate a complex terrain.
- **Action Space**: Continuous (smooth and dynamic control over movements).
- **Observation Space**:
  - Normalized velocity of the agent.
  - Average body velocity relative to the cube.
  - Rotation difference between the agent and the cube.
  - Position of the target relative to the cube's orientation.
- **Reward Structure**:
  - **LookAtTargetReward**: Encourages the agent to face the target:
    \[
    \text{lookAtTargetReward} = \frac{(\text{CubeForward} \cdot \text{BodyForward}) + 1}{2}
    \]
  - **MatchSpeedReward**: Rewards speed alignment with the target's direction:
    \[
    \text{matchSpeedReward} = \text{GetMatchingVelocityReward}(\text{CubeForward} \times \text{TargetWalkingSpeed}, \text{GetAvgVelocity})
    \]
- **Reward Threshold**: 1000 cumulative reward marks successful training.
- **Environment Modifications**: Introduced random terrain generation to increase complexity.

---

### 2. **Push Block Environment**
- **Description**: An agent pushes a block into a target area.
- **Action Space**: Discrete (5 possible actions: move forward, backward, left, right, or no movement).
- **Observation Space**:
  - Agent's position.
  - Block's position.
  - Target's position.
  - Relative positions such as block-to-target and agent-to-block.
- **Reward Structure**:
  - **Goal Reward**: +5 for successfully pushing the block into the target.
  - **Step Penalty**: Penalized per step to encourage efficiency:
    \[
    \frac{-1}{\text{MaxStep}}
    \]
- **Reward Threshold**: 4.5 cumulative reward marks successful training.
- **Environment Modifications**: Default setup retained, as it was sufficiently challenging.

---

## Training Configurations

### **Hyperparameters**

| **Parameter**            | **PPO (Crawler)**    | **SAC (Crawler)**  | **PPO (Push Block)** | **SAC (Push Block)** |
|--------------------------|----------------------|---------------------|-----------------------|-----------------------|
| Learning Rate            | Linear Decay (0.0003) | Constant (0.003)    | Linear Decay (0.0003) | Constant (0.003)      |
| Neural Network Sizes      | 512 (Hidden Units)   | 512 (Hidden Units)  | 256 (Hidden Units)    | 512 (Hidden Units)    |
| Batch Size               | 2048                 | 256                 | 128                   | 128                   |
| Entropy Coefficient      | ---                  | 1.0                 | ---                   | 0.05                  |
| Discount Factor (\(\gamma\)) | 0.95             | 0.95                | 0.99                  | 0.99                  |

---

### **Experimental Setup**
- **Training Hardware**:
  - **Computer 1**:
    - CPU: Ryzen 5 5600X
    - GPU: GeForce RTX 2070 Super
    - RAM: 16GB DDR4 @ 3600MHz
  - **Computer 2**:
    - CPU: Intel i7-13650HX
    - GPU: GeForce RTX 4060
    - RAM: 32GB DDR5 @ 4800MHz
- **Training Configuration**:
  - Used **Unity In-Editor** training setup.
  - Used the `mlagents-learn` command to initiate training.
  - Metrics recorded using **Tensorboard** for detailed analysis.
- **Training Metrics**:
  - Cumulative rewards.
  - Policy loss.
  - Entropy levels.
  - Resource utilization (CPU, memory).

---

## Results

### 1. **Crawler Environment**
- **SAC**:
  - **Cumulative Reward**: Average: 933.75, Max: 1564.66.
  - **Entropy**: Increased over time, indicating continuous exploration.
  - **Policy Loss**: Increased, suggesting the agent did not converge optimally within the training time.
  - **Resource Usage**:
    - CPU: Avg: 36.067 ms, Min: 32.18 ms, Max: 43.76 ms.
    - Memory: Avg: 1.511 GB, Max: 1.54 GB.

- **PPO**:
  - **Cumulative Reward**: Average: 1043.80, Max: 1828.07.
  - **Entropy**: Decreased significantly, favoring exploitation.
  - **Policy Loss**: Stabilized quickly, indicating convergence.
  - **Resource Usage**:
    - CPU: Avg: 33.255 ms, Min: 31.83 ms, Max: 36.83 ms.
    - Memory: Avg: 1.71 GB, Max: 1.71 GB.

### 2. **Push Block Environment**
- **SAC**:
  - **Cumulative Reward**: Average: 4.68, Max: 4.97.
  - **Training Time**: 3h 53m 50s.
  - **Resource Usage**:
    - CPU: Avg: 30.83 ms, Min: 17.21 ms, Max: 42.24 ms.
    - Memory: Avg: 0.45 GB, Max: 1.14 GB.

- **PPO**:
  - **Cumulative Reward**: Average: 4.36, Max: 4.89.
  - **Training Time**: 1h 40m 55s.
  - **Resource Usage**:
    - CPU: Avg: 29.24 ms, Min: 15.14 ms, Max: 46.07 ms.
    - Memory: Avg: 0.60 GB, Max: 1.14 GB.

---

## Key Insights

1. **Crawler Environment**:
   - PPO outperformed SAC in terms of cumulative reward and convergence speed.
   - SAC's continuous exploration resulted in higher entropy and policy loss, indicating it requires more time and computational resources to converge.

2. **Push Block Environment**:
   - SAC achieved slightly higher rewards but took more than double the time compared to PPO.
   - PPO proved to be more resource-efficient in this simpler environment.

---

## Conclusion

The experiments revealed that:
- **PPO** is suitable for scenarios where resource efficiency and quick convergence are prioritized.
- **SAC** is more robust in complex environments requiring extensive exploration, albeit at higher computational costs.

---
Ex