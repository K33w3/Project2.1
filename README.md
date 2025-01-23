# Controlling Agents in the Unity Game Engine

This repository contains the work for **Project 2.1 - AI and Machine Learning** in the Bachelor Computer Science program (Year 2) at Maastricht University. The central topic is evaluating and analyzing the performance of Machine Learning (ML) solutions for controlling agents in real-time 3D video game environments.

## Project Overview

The objective of this project is to:
1. **Apply** and **analyze** state-of-the-art Deep Reinforcement Learning (DRL) algorithms for training agents in Unity game engine environments.
2. **Develop new simulated sensors** for agents and experiment with training scenarios.
3. **Document** the findings and code in a public GitHub repository for showcasing to future employers.

This project spans **Periods 2.1, 2.2, and 2.3** of the academic year 2024-2025.

---

## Contents

- [Getting Started](#getting-started)
- [Phases and Deliverables](#phases-and-deliverables)
- [Technologies Used](#technologies-used)
- [Setup](#setup)
- [Documentation](#documentation)
- [License](#license)

---

## Getting Started

### Prerequisites

1. Install Unity (Personal or Student license recommended).
2. Install Python (version compatible with ML-Agents and virtual environments).
3. Ensure Git is installed and configured.
4. Clone this repository:
   ```bash
   git clone https://github.com/K33w3/Project2.1.git
   cd Project2.1
   ```

### Installing Dependencies

Follow the steps below to set up the required tools and dependencies:
1. **Unity ML-Agents Toolkit**:
   Use the fixed version of ML-Agents:
   ```bash
   git clone https://github.com/DennisSoemers/ml-agents.git --branch fix-numpy-release-21-branch
   ```
   Follow the [installation guide](https://github.com/Unity-Technologies/ml-agents/blob/develop/docs/Installation.md).

2. **Python Virtual Environment**:
   Set up a virtual environment for Python dependencies:
   ```bash
   python -m venv venv
   source venv/bin/activate   # On Windows, use `venv\Scripts\activate`
   pip install -r requirements.txt
   ```

---

## Phases and Deliverables

### Phase 1: Project Setup
- **Deliverables**:
  - Written project plan outlining steps, timelines, and risks.
  - Public GitHub repository with clear documentation.
- **Key Learning Goals**:
  - Familiarity with Unity, ML-Agents, and Python virtual environments.
  - Initial code modifications and testing.

### Phase 2: Agent Training and Sensor Development
- **Deliverables**:
  - Train agents using DRL algorithms in ML-Agents.
  - Develop a new sensor type for the "Soccer Twos" environment.
  - Present work during the Midway Evaluation.

### Phase 3: Performance Analysis
- **Deliverables**:
  - Analyze RL algorithm performance based on parameters, sensors, and environment complexity.
  - Written report and live demonstration.
- **Key Focus Areas**:
  - Experiment reproducibility.
  - Use of Unity Profiler for performance insights.

---

## Technologies Used

- **Unity Game Engine**: For building and running real-time 3D simulations.
- **Unity ML-Agents Toolkit**: For implementing DRL algorithms in Unity environments.
- **Python**: For scripting and managing ML-Agents.
- **PPO (Proximal Policy Optimization)**: For stable policy training in discrete and continuous environments.
- **SAC (Soft Actor-Critic)**: For robust performance in continuous action spaces.
- **Git/GitHub**: For version control and collaboration.

---

## Setup

1. Clone the repository and install dependencies as described in [Getting Started](#getting-started).
2. Open the Unity project in the Unity Editor.
3. Run the example ML-Agents environments to confirm setup.

*Please note that the model training for Push Block is currently on a different branch.*

---

## Documentation

Documentation for this project is available in the repository:
- **[Setup Guide](docs/Getting-Started.md)**: Step-by-step installation and setup instructions.

---

## Contact

For questions or feedback, contact the project contributors:
- **Contributor Name**: Alvaro Murillo Terre  
  **GitHub**: [Alvaro Murillo Terre GitHub Profile](https://github.com/Alvaro-Murillo)

- **Contributor Name**: Cojocaru Cristian  
  **GitHub**: [Cojocaru Cristian GitHub Profile](https://github.com/cristic0j)

- **Contributor Name**: Eugeniu Gheorghita 
  **GitHub**: [Eugeniu Gheorghita GitHub Profile](https://github.com/EugeniuGh)

- **Contributor Name**: Andrei Visoiu  
  **GitHub**: [Andrei Visoiu GitHub Profile](https://github.com/K33w3)

- **Contributor Name**: Khaled Ismail 
  **GitHub**: [Khaled Ismail GitHub Profile](https://github.com/KHALEDism-17)

- **Contributor Name**: Bogdan Sirbu  
  **GitHub**: [Bogdan Sirbu GitHub Profile](https://github.com/RocketFuel7)

- **Contributor Name**: Yusuf Serhat Ozkan  
  **GitHub**: [Yusuf Serhat Ozkan GitHub Profile](https://github.com/yusufserhatozkan)
