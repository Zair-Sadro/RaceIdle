using UnityEngine;

public class ProductView : MonoBehaviour
{
    [SerializeField] private ProductStorage _storage;
    [SerializeField] private TileMachine _machine;


    [SerializeField] protected Animator productCounterAnimator;
    [SerializeField] protected TMPro.TMP_Text productCountText;



    public virtual void ProductTileView(int current)
    {
        productCountText.text = $"{_storage.TilesInStorage.Count}/{_machine.machineFields.MaxTiles}";
        productCounterAnimator.SetTrigger("Plus");
    }


}




