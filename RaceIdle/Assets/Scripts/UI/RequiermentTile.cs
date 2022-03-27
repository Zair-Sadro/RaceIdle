using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequiermentTile : MonoBehaviour
{
    [SerializeField] private Image _iconImg;
    [SerializeField] private TMP_Text _tileTypeText;
    [SerializeField] private TMP_Text _amountText;

    private MachineLevel.ProductRequierment _requierment;
    private RequiermentContent _content;

    public void Init(MachineLevel.ProductRequierment req, RequiermentContent content)
    {
        _requierment = req;
        _content = content;

        _tileTypeText.text = _requierment.Type.ToString();
        _amountText.text = _requierment.Amount.ToString();
    }
}
