using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLocation : MonoBehaviour
{
    public int locationX;
    public int locationZ;

    public void SetLocation(int x, int z)
    {
        locationX = x;
        locationZ = z;
    }

    public Vector3 GetLocation()
    {
        return new Vector3(locationX, 0, locationZ);
    }

}
