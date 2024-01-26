using System.Collections.Generic;
using UnityEngine;

public class TilesLoaderInStorages : MonoBehaviour, ISaveLoad<TilesInAllStoragesData>
{
    [SerializeField] private TilesInAllStoragesData _data;

    [SerializeField] private List<GameObject> _storagesAsGameObjs;
    private Dictionary<int, ITilesSave> _storageDict = new();

    private void Awake()
    {
        for (int i = 0; i < _storagesAsGameObjs.Count; i++)
        {
            ITilesSave item = _storagesAsGameObjs[i].GetInterface<ITilesSave>();
            _storageDict.Add(i, item);
        }

    }
    private void Start()
    {
        if (_data.storagesList.Count > 0)
        {
            for (int i = 0; i < _data.storagesList.Count; i++)
            {
                var item = _data.storagesList[i];
                _storageDict[item.ID].SetTiles(item.tilesList);
            }
        }
    }
    public TilesInAllStoragesData GetData()
    {
        TilesInAllStoragesData data = new();

        for (int i = 0; i < _storageDict.Count; i++)
        {
            TilesInStorageData item = new();
            item.ID = i;
            item.tilesList = new(_storageDict[i].GetTiles());

            data.storagesList.Add(item);
        }
        return data;
    }

    public void Initialize(TilesInAllStoragesData data)
    {
        _data = data;
    }
}
