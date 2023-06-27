using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Terrain
{
    Debug,
    Water,
    Land,
    Mountain,
    Desert,
    Jungle,
    Forest,
}
public class CustomTile : Tile
{
    public Terrain terrainType;
    public float heightValue;
    public float temperatureValue;
    public float continentalityValue;
    public float humidityValue;
    public void SetTerrain(Terrain terrainType)
    {
        this.terrainType = terrainType;
        sprite = TerrainSprites.GetSprite(terrainType);
    }
    
    public void EnableDebug(NoiseMap wave)
    {
        switch (wave)
        {
            case NoiseMap.Height:
                color = new Color(heightValue, heightValue, heightValue);
                break;
            case NoiseMap.Temperature:
                color = new Color(temperatureValue, temperatureValue, temperatureValue);
                break;
            case NoiseMap.Continentality:
                color = new Color(continentalityValue, continentalityValue, continentalityValue);
                break;
            case NoiseMap.Humidity:
                color = new Color(humidityValue, humidityValue, humidityValue);
                break;
            default:
                return;
        }
        sprite = TerrainSprites.GetSprite(Terrain.Debug);
    }

/*    public void DisableDebug()
    {
        sprite = TerrainSprites.GetSprite(terrainType);
        color = Color.white;
    }*/

}
