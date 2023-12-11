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
    public static Action SoldALl;
    public static Action<int> LevelUp;

    public static Action NeedToSaveProgress;
    public static Action<string> OnLanguageChange;

    public static Action<int> OnCarBought;
}
