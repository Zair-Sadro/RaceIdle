using UnityEngine;


public class CounterView : CounterViewbase
{
    [SerializeField] private CounterViewElement _junk, _iron, _rubber, _plastic, _gold;
    public override void ChangeCount(TileType type, int currentCount, int max)
    {
        switch (type)
        {
            case TileType.Junk:
                _junk.ChangeCount(currentCount, max);
                break;

            case TileType.Iron:
                _iron.ChangeCount(currentCount, max);
                break;

            case TileType.Rubber:
                _rubber.ChangeCount(currentCount, max);
                break;

            case TileType.Plastic:
                _plastic.ChangeCount(currentCount, max);
                break;

            case TileType.Gold:
                _gold.ChangeCount(currentCount, max);
                break;
        }
    }

}
public abstract class CounterViewbase : MonoBehaviour
{
    public abstract void ChangeCount(TileType type, int currentCount, int max);
}





