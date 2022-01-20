using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //Inspector fields for testing and observations.
    [SerializeField] Region region;
    [SerializeField] Vector2 localPosition;
    [SerializeField] GameObject occupiedBy;

    //Assigns the tiles their configurations. 
    public void UpdateProperties(Region region, Vector2 position, GameObject obj = null)
    {
        this.region = region;
        this.localPosition = position;
        this.occupiedBy = obj;
    }

    //Returns the region specific location on the tilemap for each region.
    public Vector2 getLocalPosition()
    {
        return this.localPosition;
    }

}
