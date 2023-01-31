using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System;

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
    #region Properties

    public Animator Animator => CurrentAnimator(_skinAnimatorID);
    public float CurrentSpeed=>_curSpeed;

    #endregion

    private void Start()
    {
        TakeHummer(false);
    }

    private void OnEnable()
    {
        CheckSkin();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "hummerzone")
       TakeHummer(true);

    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "hummerzone")
        TakeHummer(false);
    }

    private void Move()
    {

       
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {

            if (body.velocity != Vector3.zero)
            {

                transform.rotation = Quaternion.LookRotation(new Vector3(joystick.Horizontal * speed,0, joystick.Vertical * speed));

               
                Debug.Log(_curSpeed);
                // dustParticle.gameObject.SetColliderActive(true);
            }
        }
         _curSpeed =MathF.Round( body.velocity.magnitude,2);
        body.velocity = new Vector3(joystick.Horizontal * speed, body.velocity.y, joystick.Vertical * speed);
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
