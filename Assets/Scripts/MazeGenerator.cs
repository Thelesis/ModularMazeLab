using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
    [SerializeField] GameObject startPoint;
    [SerializeField] GameObject exitPoint;

    public byte[,] mapData;
    

    // Start is called before the first frame update
    void Start()
    {
        InitialiseMap();
        GenerateMap();
        DrawMap();
    }

    private void InitialiseMap()
    {
        mapData = new byte[width, depth];
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                mapData[x, z] = 1; //1 = wall, 0 - corridor
            }
        }
    }

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

    private void DrawMap()
    {
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                if(mapData[x, z] == 1)
                {
                    Vector3 pos = new Vector3(x * mapScale, 0, z * mapScale);
                    Instantiate(wall, pos, Quaternion.identity);
                    wall.transform.position = pos;
                    wall.transform.localScale = new Vector3(mapScale, mapScale, mapScale);
                    wall.name = "[" + x + ", " + z + "]Wall";
                }

                if(mapData[x, z] == 0)
                {
                    Vector3 pos = new Vector3(x * mapScale, 0, z * mapScale);
                    Instantiate(tile, pos, Quaternion.identity);
                    tile.transform.position = pos;
                    tile.transform.localScale = new Vector3(mapScale, mapScale, mapScale);
                    tile.name = "[" + x + ", " + z + "]Empty";
                }

                if (mapData[x, z] == 2)
                {
                    Vector3 pos = new Vector3(x * mapScale, 0, z * mapScale);
                    Instantiate(startPoint, pos, Quaternion.identity);
                    startPoint.transform.position = pos;
                    startPoint.transform.localScale = new Vector3(mapScale, mapScale, mapScale);
                    startPoint.name = "[" + x + ", " + z + "]Start";
                }
            }
        }
    }

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

        Debug.Log("Square count: " + count);
        return count;
    }

    public int CountSquareNeighbours(int x, int z, int cellValue)
    {
        int count = 0;
        //if (x <= mapBorderSize || x >= width - mapBorderSize || z <= mapBorderSize || z >= depth - mapBorderSize)
        //{
        //    Debug.Log("Exceeding map border");
        //    return 5;
        //}

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

    public int CountAllNeighbours(int x, int z)
    {
        int count = 0;
        count += CountSquareNeighbours(x, z);
        count += CountDiagonalNeighbours(x, z);

        return count;
    }
}
