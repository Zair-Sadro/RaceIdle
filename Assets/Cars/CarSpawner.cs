using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("ComponentsForCar")]
    [SerializeField] private MergeMaster _mergeMaster;
    [SerializeField] private Camera _raceCamera;



    public void Spawn(int level)
    {
        var car =
        Instantiate(_cars[level], transform.position, Quaternion.Euler(_rotation), _parent);

        car.SetMergeMaster(_mergeMaster);
        car.SetRaceCamera(_raceCamera);
        car.SetPointsList(_points, _toTrackPoint);

        car.RideFromRepair();


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

        _raceTrackManager.RegisterCar(car);

    }

    [Button]
    private void Spawn()
    {
        var car =
        Instantiate(_cars[0], transform.position, Quaternion.Euler(_rotation), _parent);

        car.SetPointsList(_points, _toTrackPoint);
        car.RideFromRepair();

    }
}


