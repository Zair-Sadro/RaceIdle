using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private List<CarAI> _cars;

    [SerializeField] private Transform _parent;
    [SerializeField] private Vector3 _rotation;

    [SerializeField] private CollisionIngore _collisionIngore;

    [Header("Points")]
    [SerializeField] private List<Transform> _points = new();
    [SerializeField] private List<Transform> _toTrackPoint = new();


    public void Spawn(int level)
    {
        var car =
        Instantiate(_cars[level], transform.position, Quaternion.Euler(_rotation), _parent);

        car.SetPointsList(_points, _toTrackPoint);
        car.RideFromRepair();

        _collisionIngore.AddToIgnoreList(car.Collider);


    }
    public void Spawn(int level, Vector3 pos, Quaternion rot, MergeMaster mm, int currentCarPoint)
    {
        var car =
      Instantiate(_cars[level], pos, rot, _parent);

        car.SetPointsList(_points, _toTrackPoint);
        car.SetMergeMaster(mm);
        car.RideAfterMerge(currentCarPoint);


        _collisionIngore.AddToIgnoreList(car.Collider);
    }

    [Button]
    private void Spawn()
    {
        var car =
        Instantiate(_cars[0], transform.position, Quaternion.Euler(_rotation), _parent);

        car.SetPointsList(_points, _toTrackPoint);
        car.RideFromRepair();

        _collisionIngore.AddToIgnoreList(car.GetComponent<Collider>()); // GetComponent ='(


    }
}


