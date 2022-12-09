using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeSystem : MonoBehaviour
{
    public delegate void OnSwipeInput(Vector2 direction);
    public static event OnSwipeInput SwipeEvent;

    private Vector2 _tapPosition;
    private Vector2 _swipeDelta;

    private float _deadZone = 80;

    private bool _isSwiping;
    private bool _isMobile;

    void Start()
    {
        _isMobile = Application.isMobilePlatform;
    }

    void Update()
    {
        if (!_isMobile)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isSwiping = true;
                _tapPosition = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
                ResetSwipe();
        }
        else
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    _isSwiping = true;
                    _tapPosition = Input.GetTouch(0).position;
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Ended ||
                    Input.GetTouch(0).phase == TouchPhase.Canceled)
                {
                    ResetSwipe();
                }
            }
        }

        CheckSwipe();
    }

    private void CheckSwipe()
    {
        _swipeDelta = Vector2.zero;

        if (_isSwiping)
        {
            if (!_isMobile && Input.GetMouseButton(0))
                _swipeDelta = (Vector2)Input.mousePosition - _tapPosition;
            else if (Input.touchCount > 0)
                _swipeDelta = Input.GetTouch(0).position - _tapPosition;
        }

        if (_swipeDelta.magnitude > _deadZone)
        {
            if (SwipeEvent != null)
            {
                if (Mathf.Abs(_swipeDelta.x) > Mathf.Abs(_swipeDelta.y))
                    SwipeEvent?.Invoke(_swipeDelta.x > 0 ? Vector2.right : Vector2.left);
                else
                    SwipeEvent?.Invoke(_swipeDelta.y > 0 ? Vector2.up : Vector2.down);
            }

            ResetSwipe();
        }
    }

    private void ResetSwipe()
    {
        _isSwiping = false;
        _tapPosition = Vector2.zero;
        _swipeDelta = Vector2.zero;
    }
}
