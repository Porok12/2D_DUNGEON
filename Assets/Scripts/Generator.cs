using System;
using System.Collections.Generic;
using UnityEngine;

public class Generator
{
    public int dungeonWidth = 50;
    public int dungeonHeight = 50;
    public int roomMinSize = 8;
    public int roomMaxSize = 16;

    private bool[,] dungeonGrid;
    private List<Tuple<Vector2, Vector2, float>> edges;

    public bool[,] GetDungeonGrid()
    {
        return dungeonGrid;
    }

    public List<Tuple<Vector2, Vector2, float>> GetEdges()
    {
        return edges;
    }

    public void GenerateDungeon()
    {
        // Inicjalizacja siatki
        dungeonGrid = new bool[dungeonWidth, dungeonHeight];

        // Task 1: Implement BinarySpacePartitioning
        var rooms = BinarySpacePartitioning.GenerateGrid(dungeonWidth, dungeonHeight, roomMinSize);
        foreach (var room in rooms)
        {
            Debug.Log($"Room: {room}");
            for (int x = room.xMin; x < room.xMax; x++)
            {
                for (int y = room.yMin; y < room.yMax; y++)
                {
                    if (x >= 0 && x < dungeonWidth && y >= 0 && y < dungeonHeight) {
                        dungeonGrid[x, y] = true;
                    }
                }
            }
        }
        // Old code
        // for (int i = 0; i < 10; i++)
        // {
        //     GenerateRoom();
        // }

        // Task 2: Implement DelaunayTriangulation and MinimumSpanningTree
        // var roomsCenters = rooms.ConvertAll(room => room.center);
        // var delay = DelaunayTriangulation.GenerateDelaunayTriangulation(roomsCenters);
        // var mst = MinimumSpanningTree.GenerateMinimumSpanningTree(delay, rooms.Count);
        // edges = mst.ConvertAll(edge => Tuple.Create(rooms[edge.Item1].center, rooms[edge.Item2].center, edge.Item3));

    }

    private void GenerateRoom()
    {
        // Losowanie pozycji i rozmiaru pokoju
        int roomWidth = UnityEngine.Random.Range(roomMinSize, roomMaxSize + 1);
        int roomHeight = UnityEngine.Random.Range(roomMinSize, roomMaxSize + 1);
        int startX = UnityEngine.Random.Range(0, dungeonWidth - roomWidth);
        int startY = UnityEngine.Random.Range(0, dungeonHeight - roomHeight);

        // Sprawdzenie, czy pokój nie koliduje z istniejącymi
        if (CanPlaceRoom(startX, startY, roomWidth, roomHeight))
        {
            PlaceRoom(startX, startY, roomWidth, roomHeight);
        }
    }

    private bool CanPlaceRoom(int startX, int startY, int roomWidth, int roomHeight)
    {
        for (int x = startX; x < startX + roomWidth; x++)
        {
            for (int y = startY; y < startY + roomHeight; y++)
            {
                if (dungeonGrid[x, y]) return false; // Kolizja z istniejącym pokojem
            }
        }
        return true;
    }

    private void PlaceRoom(int startX, int startY, int roomWidth, int roomHeight)
    {
        for (int x = startX; x < startX + roomWidth; x++)
        {
            for (int y = startY; y < startY + roomHeight; y++)
            {
                dungeonGrid[x, y] = true;
            }
        }
    }
}
