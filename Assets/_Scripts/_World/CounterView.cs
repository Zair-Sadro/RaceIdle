using UnityEngine;


public class CounterView : MonoBehaviour
{

    [SerializeField] protected Animator counterAnimator;
    [SerializeField] protected TMPro.TMP_Text counter;
    [SerializeField] protected Sprite tileType;
    [SerializeField] protected SpriteRenderer _sp;
    private void Awake()
    {
        if(_sp!=null)
        _sp.sprite = tileType;
    }

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
    public void InitText(int max,int current=0)
    {
        _max = max;
        counter.text = $"{current}/{max}";
    }
  
  
}




