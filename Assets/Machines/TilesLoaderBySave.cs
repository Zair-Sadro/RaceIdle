using System.Collections.Generic;
using UnityEngine;

public class TilesLoaderBySave : MonoBehaviour, ITilesSave
{
    [SerializeField] private ResourceTilesSpawn _resourceTilesSpawn;
    [SerializeField] private AutoLayout3D.GridLayoutGroup3D _layoutGroup;
    [SerializeField] private ProductStorage _storage;
    [SerializeField] private TileType _type;
    public List<ProductRequierment> GetTiles()
    {
        List<ProductRequierment> list = new();
        list.Add(new(_type, _storage.TilesInStorage.Count));

        return list;
    }

    public void SetTiles(List<ProductRequierment> tilesList)
    {
        if (tilesList.Count > 0)
            if (tilesList[0].Amount > 0)
            {

                for (int i = 0; i < tilesList[0].Amount; i++)
                {
                    var tile = _resourceTilesSpawn.GetTile(_type);
                    _storage.TileToStorage(tile);
                }
                _layoutGroup.UpdateLayout();

            }
    }
}
