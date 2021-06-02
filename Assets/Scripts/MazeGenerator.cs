using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Helper class that holds coordinates of map cell
/// </summary>
public class MapLocation
{
    public int x;
    public int z;

    public MapLocation(int x, int z)
    {
        this.x = x;
        this.z = z;
    }
}

/// <summary>
/// Base class for maze generation algorithms
/// </summary>
public class MazeGenerator : MonoBehaviour
{
    public List<MapLocation> possibleSquareDirecions = new List<MapLocation>() {
        new MapLocation(1,0), new MapLocation(0,1), new MapLocation(-1,0), new MapLocation(0,-1) };

    public List<MapLocation> possibleDiagonalDirecions = new List<MapLocation>() {
        new MapLocation(1,1), new MapLocation(1,-1), new MapLocation(-1,-1), new MapLocation(-1,1) };

    [SerializeField] public int maxLoopRepetition = 4000;
    [SerializeField] public int mapBorderSize = 1;
    [SerializeField] int mapScale = 6;
    [SerializeField] public int width = 30; //along with x axis
    [SerializeField] public int depth = 30; //along with z axis

    [SerializeField] GameObject wall;
    [SerializeField] GameObject tile;
    [SerializeField] GameObject mazeTile;

    public byte[,] mapData;
    

    // Start is called before the first frame update
    void Start()
    {
        InitialiseMap();
        GenerateMap();
        DrawMap();
    }

    /// <summary>
    /// Fill map data wit solid wall
    /// </summary>
    private void InitialiseMap()
    {
        mapData = new byte[width, depth];
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                mapData[x, z] = 1; //1 = wall, 0 - empty cell
            }
        }
    }

    /// <summary>
    /// Virtual class to be replaced by potential algorithm
    /// </summary>
    public virtual void GenerateMap()
    {
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                if (Random.Range(0, 100) < 50)
                    mapData[x, z] = 0;
            }
        }
    }

    /// <summary>
    /// Draw every cell in Unity based on provided mapData. Adjust scale based on scale factor (Please keep mapScale as integer value)
    /// </summary>
    private void DrawMap()
    {
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 pos;
                switch (mapData[x, z])
                {
                    case 0:
                        pos = new Vector3(x * mapScale, 0, z * mapScale);
                        Instantiate(tile, pos, Quaternion.identity);
                        tile.transform.position = pos;
                        tile.transform.localScale = new Vector3(mapScale, mapScale, mapScale);
                        tile.name = "[" + x + ", " + z + "]Empty";
                        break;
                    case 1:
                        pos = new Vector3(x * mapScale, 0, z * mapScale);
                        Instantiate(wall, pos, Quaternion.identity);
                        wall.transform.position = pos;
                        wall.transform.localScale = new Vector3(mapScale, mapScale, mapScale);
                        wall.name = "[" + x + ", " + z + "]Wall";
                        break;
                    case 2:
                        pos = new Vector3(x * mapScale, 0, z * mapScale);
                        Instantiate(mazeTile, pos, Quaternion.identity);
                        mazeTile.transform.position = pos;
                        mazeTile.transform.localScale = new Vector3(mapScale, mapScale, mapScale);
                        mazeTile.name = "[" + x + ", " + z + "]Maze";
                        break;
                    default:
                        Console.WriteLine("Tile reference not found! " + mapData[x, z]);
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Check Neighbours for existing corridors - square only
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public int CountSquareNeighbours(int x, int z)
    {
        int count = 0;
        if(x <= mapBorderSize || x >= width - mapBorderSize || z <= mapBorderSize || z >= depth - mapBorderSize)
        {
            Debug.Log("Exceeding map border");
            return 5;
        }

        for(int dir = 0; dir < possibleSquareDirecions.Count; dir++)
        {
            int xPos = x + possibleSquareDirecions[dir].x;
            int zPos = z + possibleSquareDirecions[dir].z;
            if(mapData[xPos, zPos] == 0)
            {
                count++;
            }
        }

        //Debug.Log("Square count: " + count);
        return count;
    }

    /// <summary>
    /// Check Neighbours for existing cell type based on provided data - square only
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <param name="cellValue">Cell type that needs to be checked</param>
    /// <returns></returns>
    public int CountSquareNeighbours(int x, int z, int cellValue)
    {
        int count = 0;
        if (x <= mapBorderSize || x >= width - mapBorderSize || z <= mapBorderSize || z >= depth - mapBorderSize)
        {
            Debug.Log("Exceeding map border");
            return 5;
        }

        for (int dir = 0; dir < possibleSquareDirecions.Count; dir++)
        {
            int xPos = x + possibleSquareDirecions[dir].x;
            int zPos = z + possibleSquareDirecions[dir].z;
            if (mapData[xPos, zPos] == cellValue)
            {
                count++;
            }
        }

        Debug.Log("Square count: " + count);
        return count;
    }

    /// <summary>
    /// Check Neighbours for existing corridors - diagonal only
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public int CountDiagonalNeighbours(int x, int z)
    {
        int count = 0;
        if (x <= mapBorderSize || x > width - mapBorderSize || z <= mapBorderSize || z >= depth - mapBorderSize)
        {
            return 5;
        }

        for (int dir = 0; dir < possibleSquareDirecions.Count; dir++)
        {
            int xPos = x + possibleDiagonalDirecions[dir].x;
            int zPos = z + possibleDiagonalDirecions[dir].z;
            if (mapData[xPos, zPos] == 0)
            {
                count++;
            }
        }

        return count;
    }

    /// <summary>
    /// Check all Neighbours for existing corridors
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public int CountAllNeighbours(int x, int z)
    {
        int count = 0;
        count += CountSquareNeighbours(x, z);
        count += CountDiagonalNeighbours(x, z);

        return count;
    }
}
