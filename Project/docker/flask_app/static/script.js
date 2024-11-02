document.addEventListener("DOMContentLoaded", function () {
    // Fetch data from the server
    fetchData();

    // Initialize charts
    initializeCumulativeGraph();
    initializeGoalsHistogram();
});

// Function to fetch data from the server
function fetchData() {
    fetch('/data')
        .then(response => response.json())
        .then(data => {
            updateDataDisplay(data);
            updateCumulativeGraph(data);
            updateGoalsHistogram(data);
        });
}

// Update raw data display
function updateDataDisplay(data) {
    document.getElementById("tasks-completed").innerText = data.tasks_completed;
    document.getElementById("goals-team1").innerText = data.goals_team1;
    document.getElementById("goals-team2").innerText = data.goals_team2;
}

// Show specific graph
function showGraph(graphId) {
    const graphs = document.querySelectorAll(".graph");
    graphs.forEach(graph => {
        graph.style.display = "none"; // Hide all graphs
    });
    document.getElementById(graphId + "Graph").style.display = "block"; // Show selected graph
}

// Initialize Cumulative Reward Chart
let cumulativeChart;
function initializeCumulativeGraph() {
    const ctx = document.getElementById("cumulativeGraph").getContext("2d");
    cumulativeChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: Array.from({ length: 10 }, (_, i) => `Step ${i + 1}`),
            datasets: [{
                label: 'Cumulative Reward',
                data: [],
                borderColor: 'blue',
                fill: false
            }]
        },
        options: {
            responsive: true,
            scales: {
                x: { display: true },
                y: { display: true }
            }
        }
    });
}

// Update Cumulative Reward Chart
function updateCumulativeGraph(data) {
    cumulativeChart.data.datasets[0].data = data.cumulative_rewards;
    cumulativeChart.update();
}

// Initialize Goals Histogram Chart
let goalsHistogramChart;
function initializeGoalsHistogram() {
    const ctx = document.getElementById("goalsHistogram").getContext("2d");
    goalsHistogramChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ['Team 1', 'Team 2'],
            datasets: [{
                label: 'Goals',
                data: [],
                backgroundColor: ['purple', 'orange']
            }]
        },
        options: {
            responsive: true,
            scales: {
                x: { display: true },
                y: { display: true }
            }
        }
    });
}

// Update Goals Histogram Chart
function updateGoalsHistogram(data) {
    goalsHistogramChart.data.datasets[0].data = [data.goals_team1, data.goals_team2];
    goalsHistogramChart.update();
}
