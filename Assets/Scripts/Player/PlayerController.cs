using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;


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

    #region Properties

    public Animator Animator => CurrentAnimator(_skinAnimatorID);
    public Vector3 PlayerDirection { get; set; }
    public Transform Transform => this.transform;

    #endregion

    private void Start()
    {
        TakeHummer(false);
    }

    private void OnEnable()
    {
        CheckSkin();
    }

    private void Update()
    {
        
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
            if(body.velocity != Vector3.zero)
            {
                var vel = Quaternion.LookRotation(body.velocity.normalized);
                var q = new Quaternion(0,vel.y,0,vel.w);

                transform.rotation = q;

                Debug.Log(Quaternion.LookRotation(body.velocity.normalized).ToString());
                CurrentAnimator(_skinAnimatorID).SetBool("Run", true);
                // dustParticle.gameObject.SetColliderActive(true);
            }
        }
        else
        {
            CurrentAnimator(_skinAnimatorID).SetBool("Run", false);
           // dustParticle.gameObject.SetColliderActive(false);
        }
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


    public void Stop()
    {
        body.velocity = Vector3.zero;
        CurrentAnimator(_skinAnimatorID).SetBool("Run", false);
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

   private Animator CurrentAnimator(int id)
   {
        return skins[id];
   }

    public void DieAnimation()
    {
        CurrentAnimator(_skinAnimatorID).SetTrigger("Die");
    }
}
