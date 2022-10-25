using DG.Tweening;
using System.Collections;
using UnityEngine;

public class TweenTest : MonoBehaviour
{
    Tween t;
    UIController ui;
    void Start()
    {
        DOTween.defaultAutoPlay = AutoPlay.None;
       t = transform.DORotate(Vector3.right, 2f);
        ui = GetComponent<UIController>();
        ui.PanelInit(t);
       

    }
    IEnumerator test()
    {

      
      //  t.SetLoops(-1, LoopType.Yoyo).OnStepComplete(() => { Debug.Log($"AfterLoop:{t.ElapsedDirectionalPercentage()}"); });

       // Debug.Log($"Before Start:{ t.ElapsedDirectionalPercentage()}");
        t.Play();

        yield return new WaitForSeconds(1f);
        Debug.Log($"After 1sec:{t.ElapsedDirectionalPercentage()}");

        yield return new WaitForSeconds(1f);
        Debug.Log($"After 2sec:{t.ElapsedPercentage()}");
        t.PlayBackwards();
        yield return new WaitForSeconds(2f);
        Debug.Log($"After back:{t.ElapsedPercentage()}");
        //  yield return new WaitForSeconds(1.1f);
        //  t.PlayBackwards();

        //  yield return new WaitForSeconds(0.2f);
        // Debug.Log($"After Rot to back:{t.position}");
    }
    
}
