using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecursiveMaze : MazeGenerator
{
    int trapChance = 0;

    public override void GenerateMap()
    {
        GenerateMap(Random.Range(mapBorderSize + 1, width - mapBorderSize), Random.Range(mapBorderSize + 1, depth - mapBorderSize));
    }

    void GenerateMap(int x, int z)
    {
        if(CountSquareNeighbours(x, z) >= 2)
        {
            return;
        }

        if(mapData[x, z].SetData(0, trapChance))
        {
            trapChance = 0;
        }
        else
        {
            trapChance += trapChanceIncrement;
        }

        possibleSquareDirecions.Shuffle();
        GenerateMap(x + possibleSquareDirecions[0].x, z + possibleSquareDirecions[0].z);
        GenerateMap(x + possibleSquareDirecions[1].x, z + possibleSquareDirecions[1].z);
        GenerateMap(x + possibleSquareDirecions[2].x, z + possibleSquareDirecions[2].z);
        GenerateMap(x + possibleSquareDirecions[3].x, z + possibleSquareDirecions[3].z);
    }
}
