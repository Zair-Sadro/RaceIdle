using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Policy;
using UnityEngine;

public class MathKnowledge : MonoBehaviour
{
    public GameObject testCube;
    private void Start()
    {
        MathKnowledge.SuroundObj(transform, 10, 3f, testCube);
    }
    #region Objects around target
    public static void SuroundObj(Transform target,int count,float distance, GameObject EnemyPrefab)
    {
        var angle = 360 * Mathf.Deg2Rad; // 360 deg means object will be in full circle (try type 180)

    

        for (int i = 1; i <= count; i++)
        {
            Vector3 point = target.transform.position;

            float z = point.z + Mathf.Cos(angle/count*i) * distance;
            float x = point.x + Mathf.Sin(angle/count*i) * distance;

            point.x = x;
            point.z = z;
            Instantiate(EnemyPrefab, point, Quaternion.identity);
        }
       
    }
    #endregion
}
