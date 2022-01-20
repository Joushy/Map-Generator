using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Range(5, 15)]
    [SerializeField] int regionSize;
    [Range(1, 6)]
    [SerializeField] int spawnLayers;
    [Range(0.0f,1.0f)]
    [SerializeField] float structureSpawnChance;
    [Range(0, 2)]
    [SerializeField] int maxStructuresPerRegion;



    // Start is called before the first frame update
    void Start()
    {
        Generate(regionSize, spawnLayers, structureSpawnChance);
    }


    // Iterates through the number of layers around the origin (0,0) that will be created.
    public void Generate(int size, int layers, float spawnchance)
    {
        spawnLayers = Mathf.Abs(spawnLayers);
        for (int i = -layers; i <= layers; i++)
        {
            for (int j = layers; j >= -layers; j--)
            {
                // Relative tile positions
                var x = i * size;
                var z = j * size;

                Region new_region = new Region(x, z, size, spawnchance, 1);
            }
        }
    }
}
