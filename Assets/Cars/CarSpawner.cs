using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private List<CarAI> _cars;
    private RaceTrackManager _raceTrackManager 
        => InstantcesContainer.Instance.RaceTrackManager;

    [SerializeField] private Transform _parent;
    [SerializeField] private Vector3 _rotation;

    [Header("Points")]
    [SerializeField] private List<Transform> _points = new();
    [SerializeField] private List<Transform> _toTrackPoint = new();
    [SerializeField] private List<Transform> _fromShopPoints = new();
    [SerializeField] private List<Transform> _0saveSpawnPoints = new();
    [SerializeField] private List<Transform> _1saveSpawnPoints = new();
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _shopPoint;

    [Header("ComponentsForCar")]
    [SerializeField] private MergeMaster _mergeMaster;
    [SerializeField] private Camera _raceCamera;


    private void Awake()
    {
        GameEventSystem.OnCarBought += SpawnByShop;
    }
    public void Spawn(int level)
    {
        var car =
        Instantiate(_cars[level], _startPoint.position, Quaternion.Euler(_rotation), _parent);

        car.SetMergeMaster(_mergeMaster);
        car.SetRaceCamera(_raceCamera);
        car.SetPointsList(_points, _toTrackPoint);

        car.RideFromRepair();

        car.CarData.Level = level;
        _raceTrackManager.RegisterCar(car);


    }
    public void Spawn(int level, Vector3 pos, Quaternion rot, int currentCarPoint)
    {
        var car =
      Instantiate(_cars[level], pos, rot, _parent);

        car.SetPointsList(_points, _toTrackPoint);
        car.SetMergeMaster(_mergeMaster);
        car.SetRaceCamera(_raceCamera);

        car.RideAfterMerge(currentCarPoint);
        car.CarData.Level = level;
        _raceTrackManager.RegisterCar(car);

    }


    public void SpawnByShop(int level) 
    {
        var car =
       Instantiate(_cars[level], _shopPoint.position, _shopPoint.rotation, _parent);

        car.SetMergeMaster(_mergeMaster);
        car.SetRaceCamera(_raceCamera);
        car.SetPointsList(_points, _fromShopPoints);
        car.SetFirstTrackPoint(12);

        car.RideFromRepair();


        _raceTrackManager.RegisterCar(car);


    }
    public void SpawnByData(List<CarData> data)
    {
        int lastindx;
        int savePointIndx = 0;

        if (data.Count <= 6)
        {
            FillFistPoints();
        }
        else
        {
            FillFistPoints();
            FillSecondPoints();
        }

        void FillFistPoints()
        {
            int lastIndexForFirstPoints = 0;

            if (data.Count > 6)
            {
                lastIndexForFirstPoints = data.Count - 6;
            }

            for (lastindx = data.Count - 1; lastindx >= lastIndexForFirstPoints; lastindx--)
            {
                SpawnCarByDataFirstPoints(data, lastindx, 25, savePointIndx);
                savePointIndx++;
            }
        }
        void FillSecondPoints()
        {
            savePointIndx = 0;

            for (; lastindx >= 0; lastindx--)
            {
                SpawnCarByDataSecondPoints(data, lastindx, 5, savePointIndx);
                savePointIndx++;
            }
        }
    }

    private void SpawnCarByDataFirstPoints(List<CarData> data, int lastindx, int pointToFollow, int savePointIndx)
    {
        var car = Instantiate(_cars[data[lastindx].Level], _parent);

        car.SetPointsList(_points, _toTrackPoint);
        car.SetMergeMaster(_mergeMaster);
        car.SetRaceCamera(_raceCamera);

        car.transform.position = _0saveSpawnPoints[savePointIndx].position;
        car.transform.rotation = _0saveSpawnPoints[savePointIndx].rotation;


        car.RideAfterMerge(pointToFollow);

        _raceTrackManager.RegisterCarByData(car);

    }
    private void SpawnCarByDataSecondPoints(List<CarData> data, int lastindx, int pointToFollow, int savePointIndx)
    {
        var car = Instantiate(_cars[data[lastindx].Level], _parent);

        car.SetPointsList(_points, _toTrackPoint);
        car.SetMergeMaster(_mergeMaster);
        car.SetRaceCamera(_raceCamera);

        car.transform.position = _1saveSpawnPoints[savePointIndx].position;
        car.transform.rotation = _1saveSpawnPoints[savePointIndx].rotation;


        car.RideAfterMerge(pointToFollow);

        _raceTrackManager.RegisterCarByData(car);

    }

}


