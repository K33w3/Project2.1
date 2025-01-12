using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerainBuilder : MonoBehaviour
{
    // Flag to decide whether to create a random terrain matrix or a simple one.
    public bool randomMatrix = true;

    // Reference to the terrain floor object (GameObject).
    private GameObject floor;

    // Lists to store the X and Z coordinates of each square in the terrain grid.
    private List<float> xSqareCoordinates;
    private List<float> zSqareCoordinates;

    // 2D array (matrix) that stores the height values of each square on the terrain.
    public float[][] matrix;

    // Method to create a random terrain matrix with given rows and columns.
    void createRandomMatrix(int rowsNum, int colsNum)
    {
        // Initialize the matrix with the specified dimensions (rowsNum x colsNum).
        matrix = new float[rowsNum][];
        for (int i = 0; i < rowsNum; i++)
        {
            matrix[i] = new float[colsNum];  // Initialize each row with the correct number of columns
        }

        // Fill the matrix with random values between 1 and 4, then scaled down by 0.5f.
        for (int i = 0; i < rowsNum; i++)
        {
            for (int e = 0; e < colsNum; e++)
            {
                // Generate random float between 1 and 4, and then scale it down by 0.5.
                float randomNumber = Random.Range(1, 4);
                matrix[i][e] = randomNumber * 0.5f;
            }
        }
    }

    // Method to create a fixed simple matrix with predefined height values.
    void createSimpleMatrix()
    {
        matrix = new float[4][]
        {
            new float[] { 1f, 1.5f, 1f, 0.5f },
            new float[] { 0.5f, 1f, 1.5f, 1.5f },
            new float[] { 1f, 1.5f, 1f, 0.5f },
            new float[] { 0.5f, 1f, 1f, 1.5f }
        };
    }

    // Method to place terrain cubes based on the matrix values.
    void placeTerain()
    {
        floor = gameObject;  // Get the reference to the GameObject this script is attached to.

        // Get the scale and position of the floor GameObject.
        Vector3 floorScale = transform.localScale;
        Vector3 floorPosition = transform.position;

        // Get the number of squares in the X and Z directions (based on the matrix dimensions).
        float numberOfSquaresX = matrix.Length;
        float numberOfSquaresZ = matrix[0].Length;

        // Calculate the length of a single square in the X and Z directions (based on the floor's scale).
        float shapeLengthX = floorScale.x / matrix.Length;
        float shapeLengthZ = floorScale.z / matrix[0].Length;

        // Calculate the starting coordinates (minimum values) for the X and Z axes.
        float startingCoordinateX = (shapeLengthX / 2) - (floorScale.x / 2) + floorPosition.x; // Adjust for center alignment.
        float startingCoordinateZ = (shapeLengthZ / 2) - (floorScale.z / 2) + floorPosition.z; // Adjust for center alignment.

        // Generate and store the X coordinates for the terrain grid squares.
        for (int i = 0; i < numberOfSquaresX; i++)
        {
            xSqareCoordinates.Add(startingCoordinateX);  // Store the current X coordinate.
            Debug.Log(startingCoordinateX);  // Debug log the X coordinate for visual tracking.
            startingCoordinateX += shapeLengthX;  // Increment to next coordinate in X direction.
        }

        // Generate and store the Z coordinates for the terrain grid squares.
        for (int i = 0; i < numberOfSquaresZ; i++)
        {
            zSqareCoordinates.Add(startingCoordinateZ);  // Store the current Z coordinate.
            Debug.Log(startingCoordinateZ);  // Debug log the Z coordinate for visual tracking.
            startingCoordinateZ += shapeLengthZ;  // Increment to next coordinate in Z direction.
        }

        // Loop through the X and Z coordinates to build the terrain cubes at the correct positions.
        for (int x = 0; x < numberOfSquaresX; x++)
        {
            for (int z = 0; z < numberOfSquaresZ; z++)
            {
                float xCoo = xSqareCoordinates[x];  // Get the X coordinate for the current square.
                float zCoo = zSqareCoordinates[z];  // Get the Z coordinate for the current square.

                // The X and Z scale are determined by the square size.
                float xScale = shapeLengthX;
                float zScale = shapeLengthZ;

                // The Y scale (height) is determined by the matrix value at this (x, z) position.
                float yScale = matrix[x][z];

                // Calculate the Y coordinate to center the cube on the terrain grid.
                float yCoo = (floorScale.y / 2) + (yScale / 2);

                // Create a new cube primitive for the terrain square.
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                // Set the position of the cube.
                cube.transform.position = new Vector3(xCoo, yCoo, zCoo);

                // Set the scale of the cube based on the terrain grid square dimensions and height.
                cube.transform.localScale = new Vector3(xScale, yScale, zScale);
            }
        }
    }

    // Start is called when the script is first run.
    void Start()
    {
        // Initialize the lists for storing the X and Z coordinates.
        xSqareCoordinates = new List<float>();
        zSqareCoordinates = new List<float>();

        // If randomMatrix is true, create a random matrix. Otherwise, use the simple matrix.
        if (randomMatrix == true)
        {
            createRandomMatrix(10, 10);  // Generate a 10x10 random matrix.
        }
        else
        {
            createSimpleMatrix();  // Use the predefined simple matrix.
        }

        // Place the terrain cubes based on the matrix values.
        placeTerain();
    }
}
