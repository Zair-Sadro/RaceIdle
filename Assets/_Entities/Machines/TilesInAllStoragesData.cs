using System;
using System.Collections.Generic;

[Serializable]
public class TilesInAllStoragesData
{
    public List<TilesInStorageData> storagesList = new();
    public TilesInAllStoragesData()
    {
        storagesList = new();
    }
}
