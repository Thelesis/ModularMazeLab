using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Willson's maze algorithm implementation.
/// </summary>
public class MazeWillsons : MazeGenerator
{
    int trapChance = 0;
    List<MapLocation> potentialStarts = new List<MapLocation>();

    public override void GenerateMap()
    {
        int x = Random.Range(mapBorderSize, width - mapBorderSize);
        int z = Random.Range(mapBorderSize, depth - mapBorderSize);

        Debug.Log("Entry point at location: (" + x + ", " + z + ")");
        mapData[x, z].SetData(2, trapChance);

        int loopCount = 0;
        while(GetPotentialStarts() > 1 && loopCount < maxLoopRepetition)
        {
            RandomWalk();
            loopCount++;
        }
        
    }

    /// <summary>
    /// Get all potential cells that do not have any connections
    /// </summary>
    /// <returns></returns>
    private int GetPotentialStarts()
    {
        potentialStarts.Clear();
        for(int z = mapBorderSize; z < depth - mapBorderSize; z++)
        {
            for(int x = mapBorderSize; x < width - mapBorderSize; x++)
            {
                if(CountSquareNeighbours(x, z, 2) == 0)
                {
                    potentialStarts.Add(new MapLocation(x, z));
                }
            }
        }

        return potentialStarts.Count;
    }

    /// <summary>
    /// Execute random walk and check if it links to existing maze
    /// </summary>
    private void RandomWalk()
    {

        int potentialStartingIndex = Random.Range(0, potentialStarts.Count);
        int x = potentialStarts[potentialStartingIndex].x;
        int z = potentialStarts[potentialStartingIndex].z;
        bool validPath = false;
        List<MapLocation> path = new List<MapLocation>();

        Debug.Log("Entry point at location: (" + x + ", " + z + ")");

        path.Add(new MapLocation(x, z));

        int loopCounter = 0;
        while (x > mapBorderSize && x < width - mapBorderSize && z > mapBorderSize && z < depth - mapBorderSize && loopCounter < maxLoopRepetition && !validPath)
        {
            mapData[x, z].SetData(0,0);
            if(CountSquareNeighbours(x, z, 2) > 1)
            {
                break;
            }
            
            int randomDirection = Random.Range(0, possibleSquareDirecions.Count);
            int xCandidate = x + possibleSquareDirecions[randomDirection].x;
            int zCandidate = z + possibleSquareDirecions[randomDirection].z;
            Debug.Log("Next point at location: (" + x + ", " + z + ")");

            if (CountSquareNeighbours(xCandidate, zCandidate) < 2){
                x = xCandidate;
                z = zCandidate;
                Debug.Log("Candidate is valid! (" + x + ", " + z + ")");
                path.Add(new MapLocation(x, z));
            }
            validPath = CountSquareNeighbours(x, z, 2) == 1;

            loopCounter++;
        }

        if (validPath)
        {
            mapData[x, z].SetData(0,0);
            path.Add(new MapLocation(x, z));
            Debug.Log("Path linked to maze corridors");

            foreach(MapLocation loc in path)
            {
                if (mapData[loc.x, loc.z].SetData(2, trapChance))
                {
                    trapChance = 0;
                }
                else
                {
                    trapChance += trapChanceIncrement;
                }
            }
            path.Clear();
        }
        else
        {
            foreach(MapLocation loc in path)
            {
                mapData[loc.x, loc.z].SetData(1,0);
            }
            path.Clear();
        }
    }
}
