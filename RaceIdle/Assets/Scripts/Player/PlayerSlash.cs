using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlash : MonoBehaviour
{
    [SerializeField,Min(0.1f)] private float doubleTapTime;
    [SerializeField] private float activeHammerCollTime;
    [SerializeField] private PlayerController controller;
    [SerializeField] private Collider hammerColl;

    private bool _isFirstTap = false;
    private bool _canSlash = true;

//    private void Update()
//    {
//#if UNITY_EDITOR
//        if(Input.GetMouseButtonUp(0) && !_isFirstTap)
//            StartCoroutine(DoubleTap());
//#elif UNITY_ANDROID
//        if(Input.touchCount > 0)
//        {
//            if(Input.GetTouch(0).phase == TouchPhase.Ended && !_isFirstTap)
//                StartCoroutine(DoubleTap());
//        }
//#endif
//    }

    private void Slash()
    {
        if(_canSlash)
            StartCoroutine(SlashRoutine(activeHammerCollTime));
    }

    private IEnumerator DoubleTap()
    {
        _isFirstTap = true;
        float tapTime = 0;
        
        while(tapTime < doubleTapTime)
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                Slash();
                break;
            }
#elif UNITY_ANDROID
            if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Slash();
                break;
            }
#endif
            tapTime += Time.deltaTime;
            yield return null;
        }
        _isFirstTap = false;
    }

    private IEnumerator SlashRoutine(float time)
    {
        _canSlash = false;
        controller.Animator.SetTrigger("Slash");
        hammerColl.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        hammerColl.gameObject.SetActive(false);
        _canSlash = true;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out JunkCar junkcar))
            Slash();
    }

}
