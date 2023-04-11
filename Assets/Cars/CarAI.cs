using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CarAI : MonoBehaviour
{

    [SerializeField] private float _pointRange = 20f;

    private CarController _carControll;

    private List<Transform> _trackPoints = new();       
    private List<Transform> _toTrackPoint = new();

    private List<Transform> _currentList;
    private int _currentPoint;

    private float _gasPower;


    private void Start()
    {
        _carControll = GetComponent<CarController>();
    }
    private void FixedUpdate()
    {
        _carControll.GetInput(CalcuateAngle(), _gasPower);
        PointControll();
    }

    public void SetPointsList(List<Transform> trackpoints, List<Transform> toTrackPoints)
    {
        _trackPoints = trackpoints;
        _toTrackPoint = toTrackPoints;
    }
    public void RideFromRepair()
    {
        StartCoroutine(RidingFromRep());
    }
    IEnumerator RidingFromRep()
    {
        var rang = _pointRange;
        _pointRange = 7f;
        _currentList = _toTrackPoint;
        _gasPower = 0.85f;

        while (_currentPoint <= _currentList.Count-2 )
        {
            yield return null;
        }

        _currentPoint = 0;
        _currentList = _trackPoints;
        _pointRange = rang;
        _gasPower = 1f;
    }

    private float CalcuateAngle()
    {
        Vector3 point = _currentList[_currentPoint].position;
        Vector3 target = point - transform.position;
        target.Normalize();

        float angleToTarget = Vector3.SignedAngle(transform.forward, target, transform.up); //??
        float steeramount = angleToTarget / 45f;

        steeramount = Mathf.Clamp(steeramount, -1.0f, 1.0f);

        CalcuateBreak(steeramount);

        return steeramount;

    }
    private void CalcuateBreak(float angle)
    {
        if (math.abs(angle) > 0.6f)
            _carControll.breakforce = 1;
        else
            _carControll.breakforce = -1;


    }
    private void PointControll()
    {

        if (Vector3.Distance(transform.position, _currentList[_currentPoint].position) < _pointRange)
        {
            NextPoint();
        }
    }
    private void NextPoint()
    {
        if (_currentPoint == _currentList.Count - 1)
            _currentPoint = 0;
        else
            _currentPoint++;

    }

}


