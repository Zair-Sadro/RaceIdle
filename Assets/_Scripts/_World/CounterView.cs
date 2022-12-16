using UnityEngine;


public class CounterView : MonoBehaviour
{

    [SerializeField] protected Animator counterAnimator;
    [SerializeField] protected TMPro.TMP_Text counter;
    [SerializeField] protected Sprite tileType;
    

    public virtual void TextCountVisual(int currentCount,int max)
    {
        counter.text = $"{currentCount}/{max}";
        counterAnimator.SetTrigger("Plus");

    }
    public virtual void TextCountVisual(int currentCount)
    {
        counter.text = $"{currentCount}/{_max}";
        counterAnimator.SetTrigger("Plus");

    }

    private int _max;
    public void InitText(int max)
    {
        _max = max;
        counter.text = $"0/{max}";
    }
  
  
}




