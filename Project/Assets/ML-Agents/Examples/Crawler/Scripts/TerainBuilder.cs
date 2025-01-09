using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerainBuilder : MonoBehaviour
{
    public GameObject floor;
    public List<float> xSqareCoordinates;
    public List<float> zSqareCoordinates;

    public float[][] matrix = new float[4][]
        {
            new float[] { 1f, 1.5f, 1f, 0.5f },
            new float[] { 0.5f, 1f, 1.5f, 1.5f },
            new float[] { 1f, 1.5f, 1f, 0.5f },
            new float[] { 0.5f, 1f, 1f, 1.5f }
        };

    // Start is called before the first frame update
    void Start()
    {
        floor = gameObject;
    
        // Get the scale of the GameObject to which this script is attached
        Vector3 floorScale = transform.localScale;
        Vector3 floorPosition = transform.localPosition;

        float numberOfSquaresX = matrix.Length;  
        float numberOfSquaresZ = matrix[0].Length;       

        // Get the shape of the small square
        float shapeLengthX = floorScale.x/matrix.Length;
        float shapeLengthZ = floorScale.z/matrix[0].Length;

        // Geeting the minimum square coordinate
        float startingCoordinateX = (shapeLengthX/2)-(floorScale.x/2);//----------->+floorPosition.x
        float startingCoordinateZ = (shapeLengthZ/2)-(floorScale.z/2);//----------->+floorPosition.z

        // Geeting the coordinates list
        for (int i = 0; i < numberOfSquaresX; i++){
            xSqareCoordinates.Add(startingCoordinateX);
            Debug.Log(startingCoordinateX);
            startingCoordinateX += shapeLengthX;
        }

        for (int i = 0; i < numberOfSquaresZ; i++){
            zSqareCoordinates.Add(startingCoordinateZ);
            Debug.Log(startingCoordinateZ);
            startingCoordinateZ += shapeLengthZ;
        }
        
        // Building the shapes

        for (int x = 0; x < numberOfSquaresX; x++){
            for (int z = 0; z < numberOfSquaresZ; z++){
                float xCoo = xSqareCoordinates[x];
                float zCoo = zSqareCoordinates[z];

                float xScale = shapeLengthX;
                float zScale = shapeLengthZ;
                float yScale = matrix[x][z];

                float yCoo = (floorScale.y/2)+(yScale/2);
            

                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                // Set the position of the cube
                cube.transform.position = new Vector3(xCoo, yCoo, zCoo); // Set to (x, y, z)

                // Set the scale of the cube (e.g., 2 units on x, 1 unit on y, 3 units on z)
                cube.transform.localScale = new Vector3(xScale, yScale, zScale); // Scale (x, y, z)
                    
            }
        }   
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
