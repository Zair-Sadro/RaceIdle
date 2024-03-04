using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectForMessage : PlayerDetector
{
    [SerializeField] private Transform _messageObj;
    [SerializeField] private Vector3 _scale;
    // Start is called before the first frame update
    void Start()
    {
        OnPlayerEnter += ShowMessage;
        OnPlayerExit += HideMessage;
    }

    private void ShowMessage() 
    {
        _messageObj.DOScale(_scale,0.8f);
    }
    private void HideMessage()
    {
        _messageObj.DOScale(Vector3.zero, 0.8f);
    }
}
