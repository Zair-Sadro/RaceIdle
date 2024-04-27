using UnityEngine;


public class CounterView : CounterViewbase
{
    [SerializeField] private CounterViewElement _junk, _iron, _rubber, _plastic, _gold;
    public override void ChangeCount(TileType type, int currentCount)
    {
        switch (type)
        {
            case TileType.Junk:
                _junk.ChangeCount(currentCount);
                break;

            case TileType.Iron:
                _iron.ChangeCount(currentCount);
                break;

            case TileType.Rubber:
                _rubber.ChangeCount(currentCount);
                break;

            case TileType.Plastic:
                _plastic.ChangeCount(currentCount);
                break;

            case TileType.Gold:
                _gold.ChangeCount(currentCount);
                break;
        }
    }

    public override void InitCounerValues(TileType type, int currentCount, int max)
    {
        switch (type)
        {
            case TileType.Junk:
                _junk.InitCount(currentCount, max);
                break;

            case TileType.Iron:
                _iron.InitCount(currentCount, max);
                break;

            case TileType.Rubber:
                _rubber.InitCount(currentCount, max);
                break;

            case TileType.Plastic:
                _plastic.InitCount(currentCount, max);
                break;

            case TileType.Gold:
                _gold.InitCount(currentCount, max);
                break;
        }
    }
}
public abstract class CounterViewbase : MonoBehaviour
{
    public abstract void ChangeCount(TileType type, int currentCount);
    public abstract void InitCounerValues(TileType type, int currentCount, int max);
    
}





