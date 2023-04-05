using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _accelerationFactor;
    [SerializeField] private float _turnSpeed;

    [SerializeField] private float _driftFactor;

    private float _rotationAngle;

    private float steeringInput;
    private float accelInput;


    private Rigidbody _rb;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rotationAngle = 180;
    }

    private void FixedUpdate()
    {
        ApplyEngineForce();
        DicreaseSideVelocity();
        ApplySteering();
        Drift();

    }

    private void ApplyEngineForce()
    {
        var forwardV = Vector3.Dot(_rb.velocity, transform.forward);

        if (forwardV > _maxSpeed)
            return;

        Vector3 force = transform.forward * _accelerationFactor * Time.fixedDeltaTime * accelInput;
        _rb.AddForce(force, ForceMode.Force);
    }

    private void ApplySteering()
    {
        if (_rb.velocity.sqrMagnitude < 0.1f) 
        {
            _rotationAngle += 0;
        }
        else
        {
            _rotationAngle += steeringInput * _turnSpeed * Time.fixedDeltaTime;
        }

        _rb.MoveRotation(Quaternion.Euler(0, _rotationAngle, 0));

    }

    private void Drift()
    {
        if (steeringInput != 0) 
            return;
       
    }

    private void DicreaseSideVelocity()
    {
       var forwardV = transform.forward * Vector3.Dot(_rb.velocity, transform.forward);
       var rightV = transform.right * Vector3.Dot(_rb.velocity, transform.right);
       _rb.velocity = forwardV + (rightV * _driftFactor);
    }

    private float GetLateralVel()
    {
        return Vector3.Dot(transform.right,_rb.velocity);
    }

    public bool IsTireSkr(out float lateralVelocity)
    {
       
        lateralVelocity = GetLateralVel();
        if (math.abs(GetLateralVel()) > 4f)
            return true;

        return false;
    }

    public void GetInput(float steerValue, float accelValue)
    {
        steeringInput = steerValue;
        accelInput = accelValue;
    }

}


