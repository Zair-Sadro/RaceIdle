using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CarAI : MonoBehaviour
{
    [SerializeField] protected CarData _carData;
    public CarData CarData => _carData;
    public int CarLevel => _carData.ValueNumber;

    [SerializeField] private float _pointRange = 20f;
    [SerializeField] private Collider _collider;

    [SerializeField] private MergeDetect _topDetector, _botDetector;
    [SerializeField] private DragAndDrop _dragAndDrop;
    [SerializeField] private CarController _carControll;

    private bool _drive = true;

    #region Lap Controll
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "LapFinish")
        {
            if (!_carControll.Stopped)
            {
                InstantcesContainer.Instance.LapControll.LapFinished(_carData);
            }
        }
        else if (other.tag == "NoCarZone")
        {
            if (_drive)
            {
                transform.position = _currentList[_currentPoint].position;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "NoCarZone")
        {
            if (_drive)
            {
                transform.position = _currentList[_currentPoint].position;
            }
        }
    }
    #endregion

    #region Direction calculation

    private float _gasPower;
    private void FixedUpdate()
    {
        if (!_drive)
            return;

        _carControll.SetDrive(CalcuateAngle(), _gasPower);
        PointControll();
    }
    private float CalcuateAngle()
    {
        Vector3 point = _currentList[_currentPoint].position;
        Vector3 target = point - transform.position;
        target.Normalize();

        float angleToTarget = Vector3.SignedAngle(transform.forward, target, transform.up);
        float steeramount = angleToTarget / 45f;

        steeramount = Mathf.Clamp(steeramount, -1.0f, 1.0f);

        CalcuateBreak(steeramount);

        return steeramount;

    }
    private void CalcuateBreak(float angle)
    {
        if (math.abs(angle) > 0.3f)
            _carControll.breakforce = 1.5f;
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
        {
            _currentPoint = 0;
        }
        else
        {
            _currentPoint++;
        }

    }
    #endregion

    #region PointsSet

    private List<Transform> _trackPoints = new();
    private List<Transform> _toTrackPoint = new();

    private List<Transform> _currentList;
    private int _currentPoint;
    public int CurrentPoint => _currentPoint;

    public void SetPointsList(List<Transform> trackpoints, List<Transform> toTrackPoints)
    {
        _trackPoints = trackpoints;
        _toTrackPoint = toTrackPoints;
    }
    public void RideFromRepair()
    {
        _dragAndDrop.NoDragNow = true;
        StartCoroutine(RidingFromRep());
    }
    private IEnumerator RidingFromRep()
    {
        var rang = _pointRange;
        _pointRange = 7f;
        _currentList = _toTrackPoint;
        _gasPower = 0.85f;

        while (_currentPoint <= _currentList.Count - 2)
        {
            yield return null;
        }

        _currentPoint = firstTrackPoint;
        _currentList = _trackPoints;
        _pointRange = rang;
        _gasPower = 1f;
        _dragAndDrop.NoDragNow = false;

    }
    public void RideAfterMerge(int pointNumber)
    {
        _currentPoint = pointNumber;
        _currentList = _trackPoints;
        _gasPower = 1f;
    }


    #endregion


    public void StopDrive()
    {
        _drive = false;
        _carControll.StopRB();
    }
    public void StartDrive()
    {
        _drive = true;
        _carControll.StartRB();
    }
    public void SetMergeMaster(MergeMaster mm)
    {
        _topDetector.SetMergeMaster(mm);
        _botDetector.SetMergeMaster(mm);

        _carControll.EngineSound(true);
    }
    public void SetRaceCamera(Camera cam)
    {
        _dragAndDrop.RaceCamera = cam;
    }

    private int firstTrackPoint;
    internal void SetFirstTrackPoint(int v)
    {
        firstTrackPoint = v;
    }

    [SerializeField] private AudioSource _audioEngine,_audioSkid;
    internal void Mute(bool mute)
    {
        _audioEngine.mute = mute;
        _audioSkid.mute = mute;
    }
}



