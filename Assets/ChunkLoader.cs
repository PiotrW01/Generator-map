using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.EventSystems.EventTrigger;

public class ChunkLoader : MonoBehaviour
{
    public static ChunkLoader Instance { get; private set; }
    public float scale = 4.0f;
    public int seed = 222;

    public Tilemap tilemap;
    public bool showNoise = false;
    public NoiseMap selectedNoiseMap = NoiseMap.Continentality;
    public float tileHNoiseValue;
    public float tileCNoiseValue;
    public float tileTNoiseValue;
    public Terrain tileTerrainType;

    public static int chunkSize = 16;
    public static int renderDistance = 3;
    public Vector2Int currentChunk;
    public Vector3Int currentGridPos;
    public List<Vector2Int> loadedChunks;

    private void Awake()
    {
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
        //tile = ScriptableObject.CreateInstance<CustomTile>();

        loadedChunks = new List<Vector2Int>(((renderDistance * 2) + 1 + 2) * 2);
        // Current position on grid
        var gridPos = tilemap.WorldToCell(Camera.main.transform.position);
        // Current chunk
        currentChunk = GridToChunkCoords(gridPos.x, gridPos.y);
        RenderChunks(currentChunk);

        tileHNoiseValue = 1f;
        tileCNoiseValue = 1f;
        tileTNoiseValue = 1f;
    }
    private void Update()
    {
        currentGridPos = tilemap.WorldToCell(Camera.main.transform.position);

        var currentTile = tilemap.GetTile<CustomTile>(currentGridPos);
        try {
        tileHNoiseValue = currentTile.heightValue;
        tileCNoiseValue = currentTile.continentalityValue;
        tileTNoiseValue = currentTile.temperatureValue;
        tileTerrainType = currentTile.terrainType;
        } catch { }
        
        var newChunk = GridToChunkCoords(currentGridPos.x, currentGridPos.y);
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

        float[,] continentalityMap = NoiseGenerator.GenerateNoiseMap(chunkSize, scale, WaveManager.Instance.continentalityWaves, new Vector2(gridPos.x, gridPos.y), seed);
        float[,] heightMap = NoiseGenerator.GenerateNoiseMap(chunkSize, scale, WaveManager.Instance.heightWaves, new Vector2(gridPos.x, gridPos.y), seed);
        float[,] temperatureMap = NoiseGenerator.GenerateNoiseMap(chunkSize, scale, WaveManager.Instance.temperatureWaves, new Vector2(gridPos.x, gridPos.y), seed);
        float[,] humidityMap = NoiseGenerator.GenerateNoiseMap(chunkSize, scale, WaveManager.Instance.humidityWaves, new Vector2(gridPos.x, gridPos.y), seed);


        GenerateChunkTiles(gridPos, continentalityMap, heightMap, temperatureMap, humidityMap);
        loadedChunks.Add(chunkPos);
        return true;
    }
    private void GenerateChunkTiles(Vector2Int gridPos, float[,] continentalityMap, float[,] heightMap, float[,] temperatureMap, float[,] humidityMap)
    {
        for (int y = 0; y < chunkSize; y++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                CustomTile ct = ScriptableObject.CreateInstance<CustomTile>();
                ct.continentalityValue = continentalityMap[x, y];
                ct.heightValue = heightMap[x, y];
                ct.temperatureValue = temperatureMap[x, y];
                ct.humidityValue = humidityMap[x, y];

                if (showNoise)
                {
                    ct.EnableDebug(selectedNoiseMap);
                }
                else
                {
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

    public void SwitchNoise()
    {
        showNoise = !showNoise;
        ReloadChunks();
    }
}
