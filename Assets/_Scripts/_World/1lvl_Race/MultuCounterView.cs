using UnityEngine;

public class MultuCounterView : MonoBehaviour
{
    [SerializeField] private CounterView _goldView;
    [SerializeField] private CounterView _junkView;
    [SerializeField] private CounterView _ironView;
    [SerializeField] private CounterView _plasticView;
    [SerializeField] private CounterView _rubberView;

    public void InfoToUI(TileType type,int count)
    {
        switch (type)
        {
            case TileType.Junk:
                _junkView.TextCountVisual(count);
                break;
            case TileType.Iron:
                _ironView.TextCountVisual(count);
                break;
            case TileType.Rubber:
                _rubberView.TextCountVisual(count);
                break;
            case TileType.Plastic:
                _plasticView.TextCountVisual(count);
                break;
        }
    }
    public void InitUI(TileType type,int max)
    {
        switch (type)
        {
            case TileType.Junk:
                _junkView.InitText(max);
                break;
            case TileType.Iron:
                _ironView.InitText(max);
                break;
            case TileType.Rubber:
                _rubberView.InitText(max);
                break;
            case TileType.Plastic:
                _plasticView.InitText(max);
                break;
        }
    }
    public void InitGoldUI(int max)
    {
        _goldView.InitText(max);
    }
    public void InfoGoldUI(int gold)
    {
        _goldView.TextCountVisual(gold);
    }
}

