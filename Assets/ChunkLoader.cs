using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkLoader : MonoBehaviour
{
    public static ChunkLoader Instance { get; private set; }
    public float scale = 4.0f;
    public int seed = 222;

    public Tilemap tilemap;
    public NoiseMap selectedNoiseMap = NoiseMap.Continentality;
    public bool showNoise = false;

    public static int chunkSize = 16;
    public static int renderDistance = 3;
    public Vector2Int currentChunk;
    public Vector3Int currentGridPos;
    public List<Vector2Int> loadedChunks;

    private void Awake()
    {
        // Make singleton instance
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        seed = 222;

        // Set loadedChunks list size according to selected renderDistance
        loadedChunks = new List<Vector2Int>(((renderDistance * 2) + 1 + 2) * 2);
        // Current position on grid
        var gridPos = tilemap.WorldToCell(Camera.main.transform.position);
        // Set current chunk
        currentChunk = GridToChunkCoords(gridPos.x, gridPos.y);
        RenderChunks(currentChunk);
    }
    private void Update()
    {
        currentGridPos = tilemap.WorldToCell(Camera.main.transform.position);
        var newChunk = GridToChunkCoords(currentGridPos.x, currentGridPos.y);

        // Checks if the currentChunk has changed
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
                yield return new WaitForSeconds(0.015f * renderDistance);
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
    public void UnloadChunks(Vector2Int centerChunkPos)
    {
        // Goes through all loaded chunks.
        // If the chunks is too far from the current chunk it gets unloaded
        // and removed from the loadedChunks list
        for (int i = loadedChunks.Count - 1; i >= 0; i--)
        {
            if (Mathf.Abs(centerChunkPos.x - loadedChunks[i].x) > renderDistance + 1 || Mathf.Abs(centerChunkPos.y - loadedChunks[i].y) > renderDistance + 1)
            {
                UnloadChunk(loadedChunks[i]);
                loadedChunks.RemoveAt(i);
            }
        }
    }
    public bool UnloadChunk(Vector2Int chunkPos)
    {
        // Goes through all tiles in a chunk and unloads them
        // Sets each of them to null
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

        // Generate noisemaps for this chunk
        float[,] continentalityMap = NoiseGenerator.GenerateNoiseMap(chunkSize, scale, LayerManager.Instance.CLayers, new Vector2(gridPos.x, gridPos.y), seed);
        float[,] heightMap = NoiseGenerator.GenerateNoiseMap(chunkSize, scale, LayerManager.Instance.HLayers, new Vector2(gridPos.x, gridPos.y), seed);
        float[,] temperatureMap = NoiseGenerator.GenerateNoiseMap(chunkSize, scale, LayerManager.Instance.TLayers, new Vector2(gridPos.x, gridPos.y), seed);
        float[,] humidityMap = NoiseGenerator.GenerateNoiseMap(chunkSize, scale, LayerManager.Instance.HMLayers, new Vector2(gridPos.x, gridPos.y), seed);

        // Generate tiles in a chunk based on the noisemaps
        GenerateChunkTiles(gridPos, continentalityMap, heightMap, temperatureMap, humidityMap);
        // Add chunk to loadedChunks
        loadedChunks.Add(chunkPos);
        return true;
    }
    private void GenerateChunkTiles(Vector2Int gridPos, float[,] continentalityMap, float[,] heightMap, float[,] temperatureMap, float[,] humidityMap)
    {
        // Goes through each tile 
        for (int y = 0; y < chunkSize; y++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                // Creates a CustomTile Object
                CustomTile ct = ScriptableObject.CreateInstance<CustomTile>();
                // Assigns corresponding noisemap values to the tile
                ct.continentalityValue = continentalityMap[x, y];
                ct.heightValue = heightMap[x, y];
                ct.temperatureValue = temperatureMap[x, y];
                ct.humidityValue = humidityMap[x, y];
                // Chooses the terrain type for this tile
                if (continentalityMap[x, y] < 0.35f)
                {
                    if (continentalityMap[x, y] < 0.17f && heightMap[x, y] > 0.5f) ct.SetTerrain(Terrain.Land);
                    else ct.SetTerrain(Terrain.Water);
                }
                else
                {
                    switch (heightMap[x, y])
                    {
                        case < 0.3f:
                            ct.SetTerrain(Terrain.Mountain);
                            break;
                        case > 0.68f:
                            ct.SetTerrain(Terrain.Water);
                            break;
                        default:
                            switch(temperatureMap[x, y])
                            {
                                case < 0.25f:
                                    ct.SetTerrain(Terrain.Desert);
                                    break;
                                case >= 0.25f and <= 0.35f:
                                    if (humidityMap[x, y] < 0.35f) ct.SetTerrain(Terrain.Jungle);
                                    else if (humidityMap[x, y] < 0.45f) ct.SetTerrain(Terrain.Forest);
                                    else ct.SetTerrain(Terrain.Land);
                                    break;
                                case > 0.35f and < 0.8f:
                                    if (humidityMap[x, y] < 0.35f) ct.SetTerrain(Terrain.Forest);
                                    else ct.SetTerrain(Terrain.Land);
                                    break;
                                default:
                                    ct.SetTerrain(Terrain.Land);
                                    break;
                            }
                            break;
                    }
                }
                if (showNoise)
                {
                    ct.ShowNoise(selectedNoiseMap);
                }
                tilemap.SetTile(new Vector3Int(gridPos.x + x, gridPos.y + y, 0), ct);
                tilemap.RefreshTile(new Vector3Int(gridPos.x + x, gridPos.y + y, 0));
            }
        }
    }
    public void ReloadChunks()
    {
        tilemap.ClearAllTiles();
        loadedChunks.Clear();
        StartCoroutine(RenderChunks2(currentChunk));
    }

    // Returns the chunk coordinates of the chunk in which the (x,y) point is present.
    public Vector2Int GridToChunkCoords(int x, int y)
    {
        return new Vector2Int(x / chunkSize, y / chunkSize);
    }


    // Returns the starting coordinates of a chunk
    // To get the end coordinates of a chunk add (chunkSize - 1) to x and y
    public Vector2Int ChunkToGridCoords(int chunkX, int chunkY)
    {
        return new Vector2Int(chunkX * chunkSize, chunkY * chunkSize);
    }

    public void SwitchNoise()
    {
        showNoise = !showNoise;
        ReloadChunks();
    }
}
