
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _accelerationFactor;
    [SerializeField] private float _turnSpeed;

    [Header("Drift")]
    [SerializeField] private float _driftFactor;
    [SerializeField] private ParticleSystem _smoke1, _smoke2;
    [SerializeField] private TrailRenderer _leftSkid, _rightSkid;
    [SerializeField] private Transform _frontLeftWheel, _frontRightsWheel;

    private float _rotationAngle;

    private float steeringInput;
    private float accelInput;
  
    public float breakforce;
    private Rigidbody _rb;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rotationAngle = 180;
        
    }
    private Vector3 lasVelocity;

    private bool _stop;
    public bool Stopped => _stop;
    public void StopRB()
    {

        lasVelocity = _rb.velocity;
        _rb.velocity = Vector3.zero;
        _stop = true;
    }
    public void StartRB()
    {
        _stop = false;
        _rb.velocity = lasVelocity;
    }
    private void FixedUpdate()
    {
        if (_stop)
            return;
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

        Vector3 force = transform.forward * (_accelerationFactor * Time.fixedDeltaTime * accelInput);
        Vector3 breakF = transform.forward * breakforce * -0.6f;
        _rb.AddForce(force, ForceMode.Force);
        _rb.AddForce(breakF, ForceMode.Force);
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
        WheelRotate();

        void WheelRotate()
        {
            var degree = Quaternion.Euler(0, 58 * steeringInput, 0);

            _frontRightsWheel.localRotation = degree;
            _frontLeftWheel.localRotation = degree;
        }
    }
    private float laterallValye;
    private void Drift()
    {
        if (steeringInput == 0)
            return;

        // Calculate the lateral velocity of the car
        float lateralVelocity = GetLateralVel();

        // Check if the car is skidding
        if (Mathf.Abs(lateralVelocity) > laterallValye)
        {
            Skid(true);
            // Apply a sideways force to the car to continue the drift
            Vector3 driftForce = -transform.right * lateralVelocity * _driftFactor * Time.fixedDeltaTime;
            _rb.AddForce(driftForce, ForceMode.Force);
        }
        else
        {
            Skid(false);
        }

        void Skid(bool value)
        {
            _leftSkid.emitting = value;
            _rightSkid.emitting = value;

            if (value)
            {
                _smoke1.Emit(1);
                _smoke2.Emit(1);
            }
          
        }

    }

    private void DicreaseSideVelocity()
    {
        var forwardV = transform.forward * Vector3.Dot(_rb.velocity, transform.forward);
        var rightV = transform.right * Vector3.Dot(_rb.velocity, transform.right);
        _rb.velocity = forwardV + (rightV * _driftFactor);
    }

    private float GetLateralVel()
    {
        return Vector3.Dot(transform.right, _rb.velocity);
    }

    public void SetDrive(float steerValue, float accelValue)
    {
        steeringInput = steerValue;
        accelInput = accelValue;
    }

}


