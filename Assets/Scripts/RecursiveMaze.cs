using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecursiveMaze : MazeGenerator
{
    public override void GenerateMap()
    {
        GenerateMap(Random.Range(mapBorderSize, width), Random.Range(mapBorderSize, depth));
    }

    void GenerateMap(int x, int z)
    {
        if(CountSquareNeighbours(x, z) >= 2)
        {
            return;
        }

        mapData[x, z] = 0;

        possibleSquareDirecions.Shuffle();
        GenerateMap(x + possibleSquareDirecions[0].x, z + possibleSquareDirecions[0].z);
        GenerateMap(x + possibleSquareDirecions[1].x, z + possibleSquareDirecions[1].z);
        GenerateMap(x + possibleSquareDirecions[2].x, z + possibleSquareDirecions[2].z);
        GenerateMap(x + possibleSquareDirecions[3].x, z + possibleSquareDirecions[3].z);
    }
}
