using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

class Renderer
{
    public Transform dungeonParent;
    public Tilemap tilemap;
    public TileBase tile;

    public Renderer()
    {
        var gameObject = new GameObject("Dungeon");
        this.dungeonParent = dungeonParent = gameObject.transform;
    }

    public void RenderDungeon(
        bool[,] dungeonGrid,
        List<Tuple<Vector2, Vector2, float>> edges,
        int dungeonWidth,
        int dungeonHeight
    )
    {
        for (int x = 0; x < dungeonWidth; x++)
        {
            for (int y = 0; y < dungeonHeight; y++)
            {
                if (dungeonGrid[x, y])
                {
                    //Vector3 position = new Vector3(x, y, 0);
                    //GameObject.Instantiate(tilePrefab, position, Quaternion.identity, dungeonParent);

                    // Create a new GameObject for the tile
                    GameObject tile = new GameObject($"Tile_{x}_{y}");
                    tile.transform.position = new Vector3(x, y, 0);
                    tile.transform.parent = dungeonParent;

                    // Add a SpriteRenderer component to display the tile
                    var renderer = tile.AddComponent<SpriteRenderer>();
                    renderer.color = Color.white; // Default color for tiles
                    renderer.sprite = CreateTileSprite(); // Generate the tile sprite
                }
            }
        }

        if (edges == null)
        {
            return;
        }

        // Draw L-shaped corridors
        foreach (var edge in edges)
        {
            Vector2 start = edge.Item1;
            Vector2 end = edge.Item2;

            // Convert to integer coordinates
            Vector2Int startInt = Vector2Int.FloorToInt(start);
            Vector2Int endInt = Vector2Int.FloorToInt(end);

            // Step 1: Move horizontally along the X-axis
            int currentX = startInt.x;
            int currentY = startInt.y;

            while (currentX != endInt.x)
            {
                // Only place corridor tiles if the cell is not part of a room
                if (!dungeonGrid[currentX, currentY])
                {
                    dungeonGrid[currentX, currentY] = true; // Mark as corridor
                    PlaceTile(currentX, currentY, Color.gray); // Corridor tile
                }

                currentX += (currentX < endInt.x) ? 1 : -1;
            }

            // Step 2: Move vertically along the Y-axis
            while (currentY != endInt.y)
            {
                // Only place corridor tiles if the cell is not part of a room
                if (!dungeonGrid[currentX, currentY])
                {
                    dungeonGrid[currentX, currentY] = true; // Mark as corridor
                    PlaceTile(currentX, currentY, Color.gray); // Corridor tile
                }

                currentY += (currentY < endInt.y) ? 1 : -1;
            }
        }
    }

    public void RenderEdges(List<Tuple<Vector2, Vector2, float>> edges)
    {
        if (edges == null)
        {
            return;
        }

        foreach (var edge in edges)
        {
            var start = edge.Item1;
            var end = edge.Item2;

            // Create a new GameObject for the edge
            GameObject edgeObject = new GameObject($"Edge_{start}_{end}");
            edgeObject.transform.position = new Vector3(0, 0, 0);
            edgeObject.transform.parent = dungeonParent;

            // Add a LineRenderer component to display the edge
            var lineRenderer = edgeObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.5f;
            lineRenderer.endWidth = 0.5f;
            lineRenderer.SetPosition(0, new Vector3(start.x, start.y, 0));
            lineRenderer.SetPosition(1, new Vector3(end.x, end.y, 0));
        }
    }

    private void PlaceTile(int x, int y, Color color)
    {
        // Create a new GameObject for the corridor tile
        GameObject tile = new GameObject($"CorridorTile_{x}_{y}");
        tile.transform.position = new Vector3(x, y, 0);
        tile.transform.parent = dungeonParent;

        // Add a SpriteRenderer component to display the tile
        var renderer = tile.AddComponent<SpriteRenderer>();
        renderer.color = color; // Set color for corridor
        renderer.sprite = CreateTileSprite(); // Generate the tile sprite
    }

    private Sprite CreateTileSprite()
    {
        // Create a 100x100 pixel texture
        var width = 100;
        var height = 100;
        Texture2D texture = new Texture2D(width, height);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var color = Color.white;
                //if (Math.Abs(x - y) == 0) {
                //    color = Color.black;
                //}
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();

        // Create a sprite from the texture
        return Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0, 0));
    }
}
