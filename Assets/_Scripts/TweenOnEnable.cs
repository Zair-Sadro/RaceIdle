using DG.Tweening;
using UnityEngine;

public class TweenOnEnable : MonoBehaviour
{

    [SerializeField] private float _endYValue = 5f;
    private void OnEnable()
    {
        transform.DOMoveY(_endYValue, 0.7f).SetLoops(-1,LoopType.Yoyo);
    }
    private void OnDisable()
    {
        transform.DOKill();
        this.gameObject.SetActive(false);
    }
}
