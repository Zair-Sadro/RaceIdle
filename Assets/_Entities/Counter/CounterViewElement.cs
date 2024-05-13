using UnityEngine;

public class CounterViewElement : MonoBehaviour 
{
    [SerializeField] protected Animator counterAnimator;
    [SerializeField] protected TMPro.TMP_Text counter;

    private string max;
    private static readonly int Plus = Animator.StringToHash("Plus");

    public void ChangeCount(int currentCount) 
    {
        counter.text = $"{currentCount}/{max}";
        
        if(counterAnimator!=null && counterAnimator.runtimeAnimatorController!=null) counterAnimator.SetTrigger(Plus);
    }
    public void InitCount(int currentCount,int max) 
    {
        this.max = max.ToString();
        if(max==0) 
        {
            counter.text = "";
        }
        else
        {
            counter.text = $"{currentCount}/{this.max}";
        }

    }

}





