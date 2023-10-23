using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticValues 
{
    [Tooltip("Timer in seconds after tile on ground will dissapear")]
    public static float tileDisapTimer = 10f;
    public static float tileThrowDelay = 0.12f;
    internal static string aes_password = "pass";

    public static int TileGlobalIndex(TileType type) 
    {
        switch (type)
        {
            case TileType.Junk:
                return 0;
            case TileType.Iron:
                return 1;
            case TileType.Rubber:
                return 2;
            case TileType.Plastic:
                return 3;
        }
        return 0;
    }

}
