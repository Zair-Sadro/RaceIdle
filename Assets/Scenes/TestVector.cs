using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVector : MonoBehaviour
{
   
    public float speed;






    public GameObject box, plane;
    private void Start()
    {
        StartCoroutine(JumpToPos_Cor(box.transform,1f));
    }


    public IEnumerator JumpToPos_Cor(Transform target,float journeyTime = 1.0f)
    {
        Vector3 centerPoint;
        Vector3 startRelCenter;
        Vector3 endRelCenter;

        var start = transform.position;

        yield return new WaitForSeconds(2f);

        var startTime = Time.time;
        while (Vector3.Distance(transform.position, target.position) > 0.5f)
        {
             GetCenter(Vector3.up);

              float fracComplete = (Time.time - startTime) / journeyTime * speed;
            Debug.Log(fracComplete);
              transform.position = Vector3.Slerp(startRelCenter, endRelCenter, fracComplete);
              transform.position += centerPoint;

            yield return null;
            
        }


        void GetCenter(Vector3 direction)
        {
            centerPoint = (start + target.position) * .5f;
            centerPoint -= direction;
            startRelCenter = start - centerPoint;
            endRelCenter = target.position - centerPoint;
        }
    }


}
