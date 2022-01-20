using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Region
{
    public Vector2 localPosition { get; set; }
    public string regionType { get; set; }
    public List<GameObject> tilesInRegion { get; set; }
    private int size;
    private GameObject region;
    GameObject structuresParent, instance;

    // Temporary Data Entries. To Be Transfered into a config file. (JSON formatted)
    Dictionary<string, Dictionary<string, List<string>>> zones = new Dictionary<string, Dictionary<string, List<string>>>()
    {
        {
            "Pine_Forest", new Dictionary<string, List<string>>(){
                { "Materials", new List<string>(){"brown_0", "dark_green_0", "brown_1", "dark_green_0" } },
                { "Structures", new List<string>(){"temple" } },
                { "Objects", new List<string>(){"pine_cones" } },
            }
        },
        {
            "Meadows", new Dictionary<string, List<string>>(){
                { "Materials", new List<string>(){"dark_green_0", "green_1", "green_2" } },
                { "Structures", new List<string>(){"hermit_hut" } },
                { "Objects", new List<string>(){"flowers" } },
            }
        },
        {
            "Spooky_Woods", new Dictionary<string, List<string>>(){
                { "Materials", new List<string>(){"brown_1", "dark_green_0" } },
                { "Structures", new List<string>(){"graveyard" } },
                { "Objects", new List<string>(){"dead_trees" } },
            }
        },
        {
            "Mucky_Swamp", new Dictionary<string, List<string>>(){
                { "Materials", new List<string>(){"brown_1", "dark_green_0", "blue_0", "blue_1" } },
                { "Structures", new List<string>(){"fishing_spot" } },
                { "Objects", new List<string>(){"dead_trees" } },
            }
        }

    };

    // Temporary Data Entries. To Be Transfered into a config file. (JSON formatted)
    // Weights for all corresponding zones.
    Dictionary<string, int> zoneWeights = new Dictionary<string, int>()
    {
        {"Pine_Forest", 1 },
        {"Meadows", 1 },
        {"Spooky_Woods", 2 },
    };


    // Generates region with the area of (size * size)
    public Region(int xPos, int zPos, int size, float chance, int maxStructs)
    {
        // Collects the gameobjects for organization in the inspector
        instance = GameObject.Find("Instance");
        structuresParent = GameObject.Find("Structures");

        // Renames and organizes the Regions for higher accessiblity 
        regionType = getRandomRegionType();
        region = new GameObject("Zone: [" + xPos + "," + zPos + "] {" + regionType + "}" );
        region.transform.parent = instance.transform;
        tilesInRegion = new List<GameObject>();
        this.size = size;
        
        // Calls to create the map then place structures in the loaded regions.
        MakeRegion(xPos,zPos, size);
        if (Random.Range(0.0f, 1.0f) <= chance)
        {
            GenerateStructure();
        }
    }



    private void MakeRegion(int xPos, int zPos, int size)
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {

                // Acquires a random texture from the available textures provided in the data set
                var textures = zones[regionType]["Materials"];
                GameObject defaultTile = Resources.Load("tile", typeof(GameObject)) as GameObject;
                var num = Random.Range(0, zones[regionType]["Materials"].Count);

                // Creates the tile in the scene, renames it, and reorganizes it.
                var my_tile = GameObject.Instantiate(defaultTile, new Vector3(i+ xPos, 0, j + zPos), Quaternion.identity);
                my_tile.name = "Tile: [" + i + "," + j + "]";
                my_tile.GetComponent<Tile>().UpdateProperties(this, new Vector2(i, j), null);
                my_tile.transform.parent = region.transform;


                // Safety net for if the material is not found.
                try
                {
                    string path = textures.ElementAt(num);
                    Material material = Resources.Load(path, typeof(Material)) as Material;
                    my_tile.GetComponent<Renderer>().material = material;
                }
                catch { Debug.Log("Error Loading Mat."); };

                // Saves the tile in a List for access later on.
                tilesInRegion.Add(my_tile);
                
            }
        }
    }

    // Uses the center of a region as a midpoint to find and select a valid open tile.
    private GameObject GetRandomTileInGrid()
    {
        // Using scalars, along with the midpoint, picks a random x and z location away from the center.
        float midpoint = size / 2;
        float xPos, zPos;
        int xScalar = 1;
        int zScalar = 1;

        if (Random.Range(0, 2) == 0)
        {
            xScalar = -1;
        }
        if (Random.Range(0, 2) == 0)
        {
            zScalar = -1;
        }

        // Handles if the center is 2 tiles wide.
        if (size % 2 == 0)
        {
            float evenScalar = .5f;
            if (Random.Range(0, 2) == 0)
            {
                evenScalar = -.5f;
            }
            midpoint -= .5f;
            int randNum1 = Random.Range(0,(int)(midpoint-1.5f) + 1);
            int randNum2 = Random.Range(0, (int)(midpoint - 1.5f) + 1);
            xPos = midpoint + (randNum1 * xScalar) + evenScalar;
            zPos = midpoint + (randNum2 * zScalar) + evenScalar;
            
        }
        // Handles if the center is a single tile wide.
        else
        {
            int randNum1 = Random.Range(0, (int)midpoint);
            int randNum2 = Random.Range(0, (int)midpoint);
            xPos = midpoint + (randNum1 * xScalar);
            zPos = midpoint + (randNum2 * zScalar);
        }
        int index = (int)(xPos * size + zPos);
        return tilesInRegion[index];
    }

    // Chooses a random tile in each region to spawn a structure at.
    private void GenerateStructure()
    {
        var randTile = GetRandomTileInGrid();
        var structures = zones[regionType]["Structures"];
        var num = Random.Range(0, structures.Count);

        try
        {
            GameObject structure = Resources.Load(structures[num], typeof(GameObject)) as GameObject;
            Vector3 localPos = randTile.transform.position;
            var obj = GameObject.Instantiate(structure, new Vector3(localPos.x, .5f, localPos.z), Quaternion.Euler(new Vector3(0, Random.Range(0f, 360f),0)));
            obj.transform.parent = structuresParent.transform;
        }
        catch { Debug.Log("Error Generating."); }
    }


    // Generates a random Region based on regions in the data set. (Can be adjusted with weights later on.)
    private string getRandomRegionType()
    {
        var num = Random.Range(0, zones.Count);
        string region = zones.ElementAt(num).Key;
        return region;
    }

}
