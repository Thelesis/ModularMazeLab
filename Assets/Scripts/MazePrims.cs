using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Prim's maze algorithm implementation. Modified version as it holds neighbouring cells not edges
/// </summary>
public class MazePrims : MazeGenerator
{
    public override void GenerateMap()
    {
        int x = Random.Range(mapBorderSize, width - mapBorderSize);
        int z = Random.Range(mapBorderSize, depth - mapBorderSize);

        List<MapLocation> walls = new List<MapLocation>();
        Debug.Log("Entry point at location: (" + x + ", " + z + ")");
        mapData[x, z] = 0;
        AddSquareNeighbours(x, z, ref walls);

        int loopCount = 0;
        while (walls.Count > 0 && loopCount < maxLoopRepetition)
        {
            int randomWall = Random.Range(0, walls.Count);
            x = walls[randomWall].x;
            z = walls[randomWall].z;
            Debug.Log("Next point at location: (" + x + ", " + z + ")");
            walls.RemoveAt(randomWall);
            if (CountSquareNeighbours(x, z) == 1)
            {
                mapData[x, z] = 0;
                AddSquareNeighbours(x, z, ref walls);
                Debug.Log("Created corridor!");
            }

            loopCount++;
        }

    }

    /// <summary>
    /// Add cell Neighbours to list. List is used for picking up potential corridor candidates.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <param name="walls"></param>
    private void AddSquareNeighbours(int x, int z, ref List<MapLocation> walls)
    {
        walls.Add(new MapLocation(x + 1, z));
        walls.Add(new MapLocation(x, z + 1));
        walls.Add(new MapLocation(x - 1, z));
        walls.Add(new MapLocation(x, z - 1));
    }
}
