using System;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody body;
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private MeshRenderer _hummerr;

    [SerializeField] private List<Animator> skins = new List<Animator>();

    private float _curSpeed;

    private bool isMobile;
    #region Properties

    public Animator Animator => skins[0];
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

    private void Start()
    {
        TakeHummer(false);
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


    bool movedByKey;

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
    private Vector3 curRot = Vector3.zero;
    private Vector3 velocity = Vector3.zero;
    private bool MoveByKeys()
    {
        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");

        if (xMove == 0 && zMove == 0)
        {
            velocity = Vector3.zero;
            return false;

        }


        var rotation = Vector3.SmoothDamp(curRot, new Vector3(xMove, 0, zMove), ref velocity, 0.01f);

        transform.rotation = Quaternion.LookRotation(rotation);

        _curSpeed = MathF.Round(body.velocity.magnitude, 2); ;
        body.velocity = new Vector3(xMove * speed, body.velocity.y,
            zMove * speed);
        return true;
    }
    private void JoySkickMove()
    {
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {

            if (body.velocity != Vector3.zero)
            {

                transform.rotation = Quaternion.LookRotation(new Vector3(joystick.Horizontal, 0, joystick.Vertical));

            }
        }
        _curSpeed = MathF.Round(body.velocity.magnitude, 2);
        body.velocity = new Vector3(joystick.Horizontal * speed, body.velocity.y, joystick.Vertical * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "hummerzone")
            TakeHummer(true);

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "hummerzone")
            TakeHummer(false);
    }

    private void TakeHummer(bool value)
    {

        _hummerr.enabled = value;


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
