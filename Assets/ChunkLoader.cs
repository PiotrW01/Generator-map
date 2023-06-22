using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkLoader : MonoBehaviour
{
    public Tilemap tilemap;
    public Sprite sprite;
    private Tile tile;

    public static int chunkSize = 16;
    public static int renderDistance = 3;
    public Vector2Int currentChunk;
    public Vector3Int currentGridPos;
    private List<Vector2Int> loadedChunks;

    private void Start()
    {
        tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = sprite;
        loadedChunks = new List<Vector2Int>(((renderDistance * 2) + 1 + 2) * 2);
        
        // Current position on grid
        var gridPos = tilemap.WorldToCell(transform.position);
        // Current chunk
        currentChunk = GridToChunkCoords(gridPos.x, gridPos.y);
        RenderChunks(currentChunk);
    }

    private void Update()
    {
        var gridPos = tilemap.WorldToCell(transform.position);
        currentGridPos = gridPos;
        var newChunk = GridToChunkCoords(gridPos.x, gridPos.y);


        if (currentChunk != newChunk)
        {
            currentChunk = newChunk;
            // Unload chunks that are too far away
            UnloadChunks(currentChunk);
            StartCoroutine(RenderChunks2(currentChunk));
        }
    }

    public IEnumerator RenderChunks2(Vector2Int centerChunkPos)
    {
        for (int i = 0; i < renderDistance * 2 + 1; i++)
        {
            for (int j = 0; j < renderDistance * 2 + 1; j++)
            {
                // Renders chunk, if returned value is false then the chunk is already rendered / being redered, so one loop is skipped
                if (!RenderChunk(new Vector2Int(centerChunkPos.x - renderDistance + j, centerChunkPos.y - renderDistance + i))) continue;
                // Stops coroutine if rendering is happening more than 3 chunks behind main camera
                if (Mathf.Abs(centerChunkPos.x - currentChunk.x) > 3 || Mathf.Abs(centerChunkPos.y - currentChunk.y) > 3) yield break;
                yield return new WaitForSeconds(0.03f);
            }
        }
    }

    public void RenderChunks(Vector2Int centerChunkPos)
    {
        // Goes through all chunks around the current chunk and renders them
        for (int i = 0; i < renderDistance * 2 + 1; i++)
        {
            for (int j = 0; j < renderDistance * 2 + 1; j++)
            {
                RenderChunk(new Vector2Int(centerChunkPos.x - renderDistance + j, centerChunkPos.y - renderDistance + i));
            }
        }
    }

/*    public IEnumerator UnloadChunks2(Vector2Int centerChunkPos)
    {
        for (int i = loadedChunks.Count - 1; i >= 0; i--)
        {
            if (Mathf.Abs(centerChunkPos.x - loadedChunks[i].x) > renderDistance || Mathf.Abs(centerChunkPos.y - loadedChunks[i].y) > renderDistance)
            {
                UnloadChunk(loadedChunks[i]);
                loadedChunks.RemoveAt(i);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }*/


    public void UnloadChunks(Vector2Int centerChunkPos)
    {
        for (int i = loadedChunks.Count - 1; i >= 0; i--)
        {
            // Goes through all loaded chunks.
            // If the chunks is too far from the current chunk it gets unloaded
            // and removed from the loadedChunks list
            if (Mathf.Abs(centerChunkPos.x - loadedChunks[i].x) > renderDistance + 1 || Mathf.Abs(centerChunkPos.y - loadedChunks[i].y) > renderDistance + 1)
            {
                UnloadChunk(loadedChunks[i]);
                loadedChunks.RemoveAt(i);
            }
        }
    }

    public bool UnloadChunk(Vector2Int chunkPos)
    {
        var gridPos = ChunkToGridCoords(chunkPos.x, chunkPos.y);
        if (tilemap.GetTile(new Vector3Int(gridPos.x, gridPos.y, 0)) == null)
        {
            return false;
        } else
        {
            for (int i = 0; i < chunkSize; i++)
            {
                for (int j = 0; j < chunkSize; j++)
                {
                    tilemap.SetTile(new Vector3Int(gridPos.x + j, gridPos.y + i, 0), null);
                }
            }
            return true;
        }

    }

    public bool RenderChunk(Vector2Int chunkPos)
    {
        var gridPos = ChunkToGridCoords(chunkPos.x, chunkPos.y);
        if (tilemap.GetTile(new Vector3Int(gridPos.x, gridPos.y, 0)) != null)
        {
            return false;
        }

        // TODO: Generate noise maps

        Wave[] temperatureWaves = { 
            new Wave(10, 0.03f, 0.05f),
            new Wave(12, 0.03f, 0.06f),
            new Wave(7, 0.04f, 0.07f),
            new Wave(22, 0.03f, 0.08f),
            new Wave(32, 0.05f, 0.11f),
        };
        Wave[] humidityWaves = {
            new Wave(4, 0.03f, 0.08f),
            new Wave(22, 0.03f, 0.16f),
            new Wave(37, 0.04f, 0.07f),
            new Wave(72, 0.03f, 0.08f),
            new Wave(11, 0.02f, 0.11f),
        };


        float[,] temperatureMap = NoiseGenerator.GenerateNoiseMap(chunkSize, 1f, temperatureWaves, new Vector2(gridPos.x, gridPos.y));
        //float[,] humidityMap = NoiseGenerator.GenerateNoiseMap(chunkSize, 0.6f, humidityWaves, new Vector2(gridPos.x, gridPos.y));


        for (int x = chunkSize - 1; x >= 0; x--)
        {
            for (int y = chunkSize - 1; y >= 0; y--)
            {
                tile.color = new Color(temperatureMap[x,y], temperatureMap[x,y], temperatureMap[x,y]);
                //if (temperatureMap[x, y] < 0.4f && humidityMap[x,y] > 0.4) tile.color = Color.blue; else tile.color = Color.green;

                tilemap.SetTile(new Vector3Int(gridPos.x + x, gridPos.y + y, 0), tile);
                tilemap.RefreshTile(new Vector3Int(gridPos.x + x, gridPos.y + y, 0));
            }
        }

        loadedChunks.Add(chunkPos);
        return true;
    }



    /// <summary>
    /// Returns the chunk coordinates of the chunk in which the (x,y) point is present.
    /// </summary>
    public Vector2Int GridToChunkCoords(int x, int y)
    {
        return new Vector2Int(x / chunkSize, y / chunkSize);
    }

    /// <summary>
    /// <para>Returns the starting coordinates of a chunk.</para>
    /// To get the end coordinates of a chunk add (chunkSize - 1) to x and y.
    /// </summary>
    public Vector2Int ChunkToGridCoords(int chunkX, int chunkY)
    {
        return new Vector2Int(chunkX * chunkSize, chunkY * chunkSize);
    }


}
