using System.Collections.Generic;
using UnityEngine;

public class TerrainSprites : MonoBehaviour
{
    private static Dictionary<Terrain, Sprite> spriteDictionary;

    [SerializeField]
    private Sprite noise;
    [SerializeField]
    private Sprite water;
    [SerializeField]
    private Sprite land;
    [SerializeField]
    private Sprite mountain;
    [SerializeField]
    private Sprite desert;
    [SerializeField]
    private Sprite jungle;
    [SerializeField]
    private Sprite forest;

    void Awake()
    {
        spriteDictionary = new Dictionary<Terrain, Sprite>
        {
            [Terrain.Water] = water,
            [Terrain.Land] = land,
            [Terrain.Mountain] = mountain,
            [Terrain.Desert] = desert,
            [Terrain.Jungle] = jungle,
            [Terrain.Forest] = forest,
            [Terrain.Noise] = noise,
        };
    }

    public static Sprite GetSprite(Terrain type)
    {
        return spriteDictionary[type];
    } 
}
