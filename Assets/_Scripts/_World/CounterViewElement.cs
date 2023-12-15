using UnityEngine;

public class CounterViewElement : MonoBehaviour 
{
    [SerializeField] protected Animator counterAnimator;
    [SerializeField] protected TMPro.TMP_Text counter;
    public void ChangeCount(int currentCount, int max) 
    {
        counter.text = $"{currentCount}/{max}";
        counterAnimator.SetTrigger("Plus");
    }

}





