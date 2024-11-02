# Use an official lightweight Ubuntu image
FROM ubuntu:20.04

# Set environment variables to disable interactive prompts during package installation
ENV DEBIAN_FRONTEND=noninteractive

# Install required packages for Unity headless execution and Python dependencies
RUN apt-get update && \
    apt-get install -y \
    libglib2.0-0 libsm6 libxrender1 libxrandr2 libfontconfig1 \
    python3 python3-pip && \
    apt-get clean

# Install Python dependencies, ensuring a compatible protobuf version
RUN pip3 install protobuf==3.20.3
RUN pip3 install numpy==1.21.0
RUN pip3 install mlagents pandas flask

# Set the working directory inside the container
WORKDIR /usr/src/app

# Copy the entire build folder into the container
COPY build/ /usr/src/app/build/

# Copy experiment scripts into the container
COPY experiments/ /usr/src/app/experiments/

# Copy the Flask app into the container
COPY flask_app/ /usr/src/app/flask_app/


# Grant execute permissions to the Unity executable
RUN chmod +x /usr/src/app/build/BuildTwoSoccer.x86_64

# Set environment variables for Unity to run in headless mode without a display
ENV DISPLAY=:0
ENV UNITY_NOPROXY=1

# Create a results directory inside the container to store output
RUN mkdir -p /usr/src/app/results

# Run the experiment script in the background and start the Flask server
CMD ["bash", "-c", "python3 /usr/src/app/experiments/scripts/log_goals_experiment.py & python3 /usr/src/app/flask_app/app.py"]
