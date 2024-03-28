using System;
using System.Diagnostics.Tracing;
using UnityEngine;
using YG;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float angleToExitFight;
    
    [SerializeField] private Rigidbody body;
    [SerializeField] private FloatingJoystick joystick;

    [SerializeField] private Animator _animator;

    private float _curSpeed;
    private Vector3 curRot = Vector3.zero;
    private Vector3 velocity = Vector3.zero;

    private bool isMobile;
    private bool movedByKey;
    private bool isDestroyableInFront;
    #region Properties

    public Animator Animator => _animator;
    public float CurrentSpeed => _curSpeed;
    public float MaxSpeed => speed;

    #endregion

    private void Awake()
    {
        YandexGame.GetDataEvent += CheckPlatform;
    }
    private void CheckPlatform()
    {
        isMobile = YandexGame.EnvironmentData.isMobile;
    }

    public void SetSpeed(float speed)
    {
        if (this.speed >= 17.5f)
            return;
        this.speed = speed;
    }
    public void SetSpeedByBonus(float speed)
    {
        this.speed = speed;
    }



    private void FixedUpdate()
    {
        if (!isMobile)
        {
            movedByKey = MoveByKeys();
        }

        if (movedByKey)
            return;

        JoySkickMove();
    }

    private bool MoveByKeys()
    {
        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");

        if (xMove == 0 && zMove == 0)
        {
            velocity = Vector3.zero;
            _animator.SetBool("Run", false);
            return false;
        }



        Vector3 lookDirection = Camera.main.transform.forward;
        Vector3 moveDirection = new Vector3(xMove, 0, zMove).normalized;

        // Определяем угол между направлением взгляда и направлением движения
        float angle = Vector3.Angle(lookDirection, moveDirection);

        CheckFrontAndDirectrion(angle);


        // Применяем поворот
        var rotation = Vector3.SmoothDamp(curRot, moveDirection, ref velocity, 0.01f);
        transform.rotation = Quaternion.LookRotation(rotation);

        _curSpeed = MathF.Round(body.velocity.magnitude, 2);


        return true;

        void CheckFrontAndDirectrion(float angle)
        {
            if (isDestroyableInFront)
            {
                if (angle >= angleToExitFight)
                    _animator.SetBool("Run", true);
                else
                    _animator.SetBool("Run", false);
            }
            else
            {
                _animator.SetBool("Run", true);
            }
        }
    }

    private void JoySkickMove()
    {
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            Vector3 lookDirection = Camera.main.transform.forward;
            Vector3 moveDirection = new Vector3(joystick.Horizontal, 0, joystick.Vertical).normalized;

            // Определяем угол между направлением взгляда и направлением движения
            float angle = Vector3.Angle(lookDirection, moveDirection);
            CheckFrontAndDirectrion(angle);

            // Применяем поворот
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
        else
        {
            _curSpeed = MathF.Round(body.velocity.magnitude, 2);
            _animator.SetBool("Run", false);
            return;
        }

        _curSpeed = MathF.Round(body.velocity.magnitude, 2);

        void CheckFrontAndDirectrion(float angle)
        {
            if (isDestroyableInFront)
            {
                if (angle >= angleToExitFight)
                    _animator.SetBool("Run", true);
                else
                    _animator.SetBool("Run", false);
            }
            else
            {
                _animator.SetBool("Run", true);
            }
        }
    }
    [SerializeField] private float _maxDistRay = 5f;

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _maxDistRay))
        {
            // Если луч столкнулся с объектом с указанным тегом
            if (hit.collider.CompareTag("Destroyable"))
            {
                isDestroyableInFront = true;
            }
            else 
            {
                isDestroyableInFront = false;
            }
        }

    }


}

[Serializable]
public class PlayerData
{
    public float SpeedValue;
    public int SpeedLevel;

    public float CapacityValue;
    public int CapacityLevel;

    public int DamageValue;
    public int DamageLevel;

    public float SpeedPriceValue;
    public float CapacityPriceValue;
    public float DamagePriceValue;
}
