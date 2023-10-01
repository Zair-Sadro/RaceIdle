using System.Collections;
using UnityEngine;

public class MergeMaster : MonoBehaviour
{
    private MergeDetect bottomCar, topCar;
    [SerializeField] private CarSpawner _carSpawner;
    private int currentCarPoint;
    public void SetBottomCar(MergeDetect car, int currentPoint)
    {
        bottomCar = car;
        currentCarPoint = currentPoint;
    }
    public void SetTopCar(MergeDetect car)
    {
        topCar = car;
    }

    IEnumerator MergeProcess()
    {
        if (!bottomCar || !topCar)
        {
            yield return null;
        }
        var top = topCar;
        var bot = bottomCar;
        topCar = null;
        bottomCar = null;

        yield return new WaitForSeconds(1f);
        StartCoroutine(CreateProcess(top, bot));
       
    }
    IEnumerator CreateProcess(MergeDetect top, MergeDetect bot)
    {
        Destroy(bot.gameObject,0.1f);
        _carSpawner.Spawn(1, bot.transform.position, bot.transform.rotation,this,currentCarPoint);
        yield return null;
    }

}
