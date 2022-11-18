using UnityEngine;


public class CounterView : MonoBehaviour
{

    [SerializeField] protected Animator counterAnimator;
    [SerializeField] protected TMPro.TMP_Text counter;

    public virtual void TextCountVisual(int maxValue,int current)
    {
        counter.text = $"{maxValue}/{maxValue}";
        counterAnimator.SetTrigger("Plus");

    }
  
  
}




