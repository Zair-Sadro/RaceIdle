using System;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody body;
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private ParticleSystem dustParticle;

    [SerializeField] private MeshRenderer _hummerr;


    [Header("Skins")]
    [SerializeField] private List<Animator> skins = new List<Animator>();


    private int _skinAnimatorID;
    private float _curSpeed;

    private bool isMobile;
    #region Properties

    public Animator Animator => CurrentAnimator(0);
    public float CurrentSpeed => _curSpeed;

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

    private void OnEnable()
    {
        //CheckSkin();
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
    private Vector3 vel  = Vector3.zero;
    private bool MoveByKeys()
    {
        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");

        if (xMove == 0 && zMove == 0)
        {
            vel = Vector3.zero;
            return false;
         
        }
               

        var rotation = Vector3.SmoothDamp(curRot,new Vector3(xMove,0, zMove),ref vel,0.01f);
        
        transform.rotation = Quaternion.LookRotation(rotation);
        
        _curSpeed = MathF.Round(body.velocity.magnitude, 2); ;
        body.velocity = new Vector3(xMove*speed, body.velocity.y,
            zMove * speed );
        return true;
    }

    private void JoySkickMove()
    {
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {

            if (body.velocity != Vector3.zero)
            {

                transform.rotation = Quaternion.LookRotation(new Vector3(joystick.Horizontal , 0, joystick.Vertical ));

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

    private void CheckSkin()
    {
        if (!PlayerPrefs.HasKey("BodySkin_ID"))
            PlayerPrefs.SetInt("BodySkin_ID", 1);
        else
        {
            for (int i = 1; i < skins.Count; i++)
                if (i == PlayerPrefs.GetInt("BodySkin_ID"))
                    skins[i].gameObject.SetActive(true);
                else
                    skins[i].gameObject.SetActive(false);
        }
        _skinAnimatorID = PlayerPrefs.GetInt("BodySkin_ID");
    }

    private Animator CurrentAnimator(int id)
    {
        return skins[id];
    }

}
public class PlayerSave : MonoBehaviour, ISaveLoad<PlayerData>
{
    [SerializeField] private PlayerData _data;
    [SerializeField] private PlayerController _player;
    public PlayerData GetData()
    {
        _data.position = new(_player.transform.position);
        return _data;
    }

    public void Initialize(PlayerData data)
    {
        _player.transform.position = _data.position.UnityVector;
    }
}
[Serializable]
public class PlayerData
{
    public SerializableVector3 position;
}
