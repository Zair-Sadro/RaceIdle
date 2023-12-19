using UnityEngine;

public class CounterViewElement : MonoBehaviour 
{
    [SerializeField] protected Animator counterAnimator;
    [SerializeField] protected TMPro.TMP_Text counter;

    private string max;

    public void ChangeCount(int currentCount) 
    {
        counter.text = $"{currentCount}/{max}";
        counterAnimator.SetTrigger("Plus");
    }
    public void InitCount(int currentCount,int max) 
    {
        this.max = max.ToString();
        counter.text = $"{currentCount}/{this.max}";
    }

}





