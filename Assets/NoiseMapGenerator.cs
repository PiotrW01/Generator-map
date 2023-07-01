/*using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class NoiseMapGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public Sprite sprite;
    [Range(0.1f, 0.9f)]
    public float colorTreshold = 0.4f;
    private Tile tile;

    [Header("Dimensions")]
    public int width = 100;
    public int height = 100;
    public float scale = 1.0f;
    public Vector2 offset;

    [Header("Height Map")]
    public Wave[] heightWaves;
    public float[,] heightMap;
    *//*    [Header("Moisture Map")]
        public Wave[] moistureWaves;
        private float[,] moistureMap;
        [Header("Heat Map")]
        public Wave[] heatWaves;
        private float[,] heatMap;*//*
    private bool isColorEnabled = false;

*//*    Wave[] testWaves = {
            new Wave(10, 0.03f, 0.05f),
            new Wave(12, 0.03f, 0.06f),
            new Wave(7, 0.04f, 0.07f),
            new Wave(22, 0.03f, 0.08f),
            new Wave(32, 0.05f, 0.11f),
        };*//*

    private void Start()
    {
        // stworzenie tile do wype³niania tileMapy
        tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = sprite;
        // wyœrodkowanie tilemapy na ekranie
        //tilemap.transform.position = new Vector3(-width / 2, -height / 2, 0);

        // generacja noiseMapy na tileMapie
        GenerateMap();
    }
*//*
    public static float[,] Generate(int width, int height, float scale, Wave[] waves, Vector2 offset)
    {
        float[,] noiseMap = new float[width, height];

        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                float samplePosX = (float)x * scale + offset.x;
                float samplePosY = (float)y * scale + offset.y;

                float normalization = 0.0f;
                foreach (Wave wave in waves)
                {
                    noiseMap[x, y] += wave.amplitude * Mathf.PerlinNoise(samplePosX * wave.frequency
                                    + wave.seed, samplePosY * wave.frequency + wave.seed);
                    normalization += wave.amplitude;
                }
                noiseMap[x, y] /= normalization;
            }
        }

        return noiseMap;
    }*//*

    public void GenerateMap()
    {
        // wyczyszczenie ca³ej tileMapy
        tilemap.ClearAllTiles();
        // height map
        // generacja noiseMap 
        //heightMap = Generate(width, height, scale, testWaves, offset);
        // moisture map
        //moistureMap = Generate(width, height, scale, moistureWaves, offset);
        // heat map
        //heatMap = Generate(width, height, scale, heatWaves, offset);

        // wype³nienie tileMapy wartoœciami z wygenerowanej noiseMap
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                // ustawienie koloru kwadracika na podstawie wartoœci w noiseMapie
                if(isColorEnabled) tile.color = heightMap[x, y] < colorTreshold ? Color.blue : Color.green; 
                else tile.color = new Color(heightMap[x, y], heightMap[x, y], heightMap[x, y]);
                // ustawienie kwadracika w danym punkcie tileMapy i odœwie¿enie go
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);
                tilemap.RefreshTile(new Vector3Int(x, y, 0));
            }
        }
        
    }

    public void setColor(bool val)
    {
        isColorEnabled = val;
    }

    public bool isColor()
    {
        return isColorEnabled;
    }

}

*/