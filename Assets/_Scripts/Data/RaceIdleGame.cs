using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class RaceIdleGame : MonoBehaviour,ISaveLoad<RaceIdleData>
{
    #region Injected controllers/managers

    [Inject] private TileSetter _tileSetter;

    #endregion

    #region Data fields
    public TileSetterData TileSetterData
    {
        get => _tileSetter.GetData();
        set => _tileSetter.Initialize(value);
    }
    #endregion

    #region DataSaveLoad
    [SerializeField] private RaceIdleData _data;
    public RaceIdleData GetData()
    {

        _data.tileSetterData = TileSetterData;



        return _data;
    }

    public void Initialize(RaceIdleData data)
    {
        _data = data;

        TileSetterData = data.tileSetterData;
    }
    #endregion

   


}
