using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkBorderRenderer : MonoBehaviour
{
    public bool chunkBorders = false;
    public Tilemap tilemap;

    void Update()
    {
        if (chunkBorders) DrawChunkBorders();
    }

    // Draws a border around each chunks, enable gizmos to view in game view
    // Doesn't work in build
    public void DrawChunkBorders()
    {
        var gap = (ChunkLoader.chunkSize - 1);

        foreach (var chunk in ChunkLoader.Instance.loadedChunks)
        {
            var startPos = tilemap.CellToWorld((Vector3Int)ChunkLoader.Instance.ChunkToGridCoords(chunk.x, chunk.y));
            var endPos = tilemap.CellToWorld((Vector3Int)ChunkLoader.Instance.ChunkToGridCoords(chunk.x, chunk.y) + new Vector3Int(gap, gap));

            Debug.DrawLine(new Vector3(startPos.x, startPos.y), new Vector3(endPos.x, startPos.y), Color.red);
            Debug.DrawLine(new Vector3(startPos.x, startPos.y), new Vector3(startPos.x, endPos.y), Color.red);
            Debug.DrawLine(new Vector3(startPos.x, endPos.y), new Vector3(endPos.x, endPos.y), Color.red);
            Debug.DrawLine(new Vector3(endPos.x, startPos.y), new Vector3(endPos.x, endPos.y), Color.red);
        }
    }

}
