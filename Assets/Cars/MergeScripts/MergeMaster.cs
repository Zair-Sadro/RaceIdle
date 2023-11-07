using System.Collections;
using UnityEngine;

public class MergeMaster : MonoBehaviour
{
    private MergeDetect bottomCar, topCar;
    [SerializeField] private CarSpawner _carSpawner;
    [SerializeField] private ParticleSystem _mergeVfx;
    private int currentCarPoint;
    private RaceTrackManager _raceTrackManager;
    void Start()
    {
        _raceTrackManager = InstantcesContainer.Instance.RaceTrackManager;
    }
    public void SetBottomCar(MergeDetect car)
    {
        bottomCar = car;
       
       
        StartCoroutine(MergeProcess());
    }
    public void SetTopCar(MergeDetect car, int currentPoint)
    {
        topCar = car;
        currentCarPoint = currentPoint;
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

        if (top.CarAI.CarLevel == bot.CarAI.CarLevel)
            StartCoroutine(CreateProcess(top, bot));
        else
            yield break;

    }
    IEnumerator CreateProcess(MergeDetect top, MergeDetect bot)
    {
        _raceTrackManager.DeleteCar(bot.CarAI);
        _raceTrackManager.DeleteCar(top.CarAI);

        var nextLevel = bot.CarAI.CarLevel + 1;
        var topTransform = top.CarAI.transform;

        Destroy(bot.CarAI.gameObject, 0.1f);
        Destroy(top.CarAI.gameObject, 0.1f);
       
        _carSpawner.Spawn(nextLevel,topTransform.position, topTransform.rotation, currentCarPoint);

        _mergeVfx.transform.position = topTransform.position;
        _mergeVfx.Play();
        InstantcesContainer.Instance.AudioService.PlayAudo(AudioName.MERGE);

        _raceTrackManager.StartCars();
        yield break;
    }

}
