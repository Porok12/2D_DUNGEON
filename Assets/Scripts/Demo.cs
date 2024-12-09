using UnityEngine;

public class Demo : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var generator = new Generator();
        var renderer = new Renderer();

        generator.GenerateDungeon();
        var dungeonGrid = generator.GetDungeonGrid();
        var edges = generator.GetEdges();
        renderer.RenderEdges(edges);
        renderer.RenderDungeon(dungeonGrid, edges, generator.dungeonWidth, generator.dungeonHeight);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
