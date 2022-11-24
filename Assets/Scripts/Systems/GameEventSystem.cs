using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEventSystem
{
    public enum Events
    {
        None,
        BridgeBuilt

    }


    public static Action CloseAllPanels;

    public static Action<GameObject> ObjectTaped;

    public static Action<TileType> TileSold;
    public static Action<TileType> TileBought;


}
