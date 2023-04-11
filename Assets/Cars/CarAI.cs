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

    private float _gasPower;

    private void Start()
    {
      _pointLenght = _points.Count;
      _carControll= GetComponent<CarController>();
    }
    private void FixedUpdate()
    {
        _carControll.GetInput(CalcuateAngle(), 1f);
        PointControll();
    }
    private float CalcuateAngle()
    {
        Vector3 point = _points[_currentPoint].transform.position;
        Vector3 target = point - transform.position;
        target.Normalize();

        float angleToTarget = Vector3.SignedAngle(transform.forward, target, transform.up); //??
        float steeramount = angleToTarget / 45f;

        steeramount = Mathf.Clamp(steeramount, -1.0f, 1.0f);

        CalcuateBreak(steeramount);

        return steeramount;

    }
    void CalcuateBreak(float angle)
    {
        if (math.abs(angle) > 0.7f)
            _carControll.breakforce = 1;
        else
            _carControll.breakforce = -1;
            

    }
    private void PointControll()
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

    }
}


