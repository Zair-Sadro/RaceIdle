using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CarAI :MonoBehaviour 
{
    [SerializeField] private List<GameObject> _points = new();
    [SerializeField] private float _pointRange=20f;

    private CarController _carControll;

    private int _currentPoint;
    private int _pointLenght;

    private void Start()
    {
      _pointLenght = _points.Count;
      _carControll= GetComponent<CarController>();
    }
    private void FixedUpdate()
    {
        _carControll.GetInput(CalcuateAngle(), 0.5f);
        LapControll();
    }
    private float CalcuateAngle()
    {
        Vector3 point = _points[_currentPoint].transform.position;
        Vector3 target = point - transform.position;
        target.Normalize();

        float angleToTarget = Vector3.SignedAngle(transform.forward, target, transform.up); //??
        float steeramount = angleToTarget / 45f;
        Debug.Log(angleToTarget);
        steeramount = Mathf.Clamp(steeramount, -1.0f, 1.0f);

        return steeramount;

    }
    private void LapControll()
    {
        if (Vector3.Distance(transform.position, _points[_currentPoint].transform.position) < _pointRange) 
        {
            NextPoint();
        }
    }
    void NextPoint()
    {
        if (_currentPoint == _pointLenght - 1) 
            _currentPoint = 0;
        else 
            _currentPoint++;
        Debug.Log($"Point{_currentPoint}");
    }
}


