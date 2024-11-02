from flask import Flask, render_template, jsonify
import json
import os

app = Flask(__name__)

@app.route('/')
def index():
    return render_template('index.html')

@app.route('/data')
def data():
    result_path = '/usr/src/app/experiments/results/goal_log.json'
    if os.path.exists(result_path):
        with open(result_path, 'r') as f:
            goal_data = json.load(f)
    else:
        goal_data = []

    # Calculate total goals scored by each team across all entries
    total_goals_team1 = sum(entry['team_0_goals'] for entry in goal_data) if goal_data else 0
    total_goals_team2 = sum(entry['team_1_goals'] for entry in goal_data) if goal_data else 0

    data = {
        'tasks_completed': len(goal_data),
        'goals_team1': total_goals_team1,
        'goals_team2': total_goals_team2,
        'cumulative_rewards': [entry['team_0_goals'] - entry['team_1_goals'] for entry in goal_data[-10:]]
    }
    return jsonify(data)

if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0')
