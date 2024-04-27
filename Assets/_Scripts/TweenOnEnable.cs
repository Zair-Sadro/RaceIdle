using DG.Tweening;
using UnityEngine;

public class TweenOnEnable : MonoBehaviour
{

    private void OnEnable()
    {
        transform.DOMoveY(5, 0.7f).SetLoops(-1,LoopType.Yoyo);
    }
    private void OnDisable()
    {
        transform.DOKill();
        this.gameObject.SetActive(false);
    }
}
