using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _accelerationFactor;
    [SerializeField] private float _turnSpeed;

    private float _rotationAngle;
    private float steeringInput;
    private float accelInput;

    private Rigidbody _rb;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        accelInput = Input.GetAxis("Vertical");
        steeringInput = Input.GetAxis("Horizontal");
        ApplyEngineForce();
        ApplySteering();
    }
    private void ApplyEngineForce()
    {
        Vector3 force = transform.forward * _accelerationFactor * _maxSpeed * Time.fixedDeltaTime * accelInput;
        _rb.AddForce(force, ForceMode.Force);
    }
    private void ApplySteering()
    {
        _rotationAngle += steeringInput * _turnSpeed * Time.fixedDeltaTime;
        _rb.MoveRotation(Quaternion.Euler(0, _rotationAngle, 0));
    }
}
