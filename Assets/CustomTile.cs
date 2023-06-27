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
    
    public void EnableDebug(WaveType wave)
    {
        switch (wave)
        {
            case WaveType.Height:
                color = new Color(heightValue, heightValue, heightValue);
                break;
            case WaveType.Temperature:
                color = new Color(temperatureValue, temperatureValue, temperatureValue);
                break;
            case WaveType.Continentality:
                color = new Color(continentalityValue, continentalityValue, continentalityValue);
                break;
            case WaveType.Humidity:
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
