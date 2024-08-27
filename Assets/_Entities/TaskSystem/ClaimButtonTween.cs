using System;
using UnityEngine;
using DG.Tweening;

public class ClaimButtonTween : MonoBehaviour
{
    [SerializeField] private RectTransform imageRectTransform;
    [SerializeField] private float scaleDuration = 1.5f;
    [SerializeField] private float rotationDuration = 2f;
    [SerializeField] private float scaleMultiplier = 1.2f;
    [SerializeField] private float rotationAngle = 15f;

    private void OnEnable()
    {
        AnimateScale();
        AnimateRotation();
    }

    private void OnDisable()
    {
        this.DOKill();
    }

    private void AnimateScale()
    {
        // Start the infinite scaling animation
        imageRectTransform.DOScale(scaleMultiplier, scaleDuration)
            .SetEase(Ease.InOutBounce)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void AnimateRotation()
    {
        // Start the infinite rotation animation
        imageRectTransform.DORotate(new Vector3(0, 0, rotationAngle), rotationDuration, RotateMode.LocalAxisAdd)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
}