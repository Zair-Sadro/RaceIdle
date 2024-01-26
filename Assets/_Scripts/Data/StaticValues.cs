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
    public static TileType TileGlobalIndex(int indx)
    {
        switch (indx)
        {
            case 0:
                return TileType.Junk;
            case 1:
                return TileType.Iron;
            case 2:
                return TileType.Rubber;
            case 3:
                return TileType.Plastic;
        }
        return 0;

    }
    public static T GetInterface<T>(this GameObject gameObject) where T : class
    {
        var interfaceComponent = gameObject.GetComponent(typeof(T)) as T;

        if (interfaceComponent is T)
        {
            return interfaceComponent;
        }

        Debug.LogError($"No interface like {typeof(T)} in {gameObject}");
        return null;
    }
}