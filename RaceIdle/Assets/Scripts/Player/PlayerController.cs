using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody body;
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private ParticleSystem dustParticle;

    [Header("Skins")]
    [SerializeField] private List<Animator> skins = new List<Animator>();


    private int _skinAnimatorID;

    #region Properties

    public Vector3 PlayerDirection { get; set; }

    public Transform Transform => this.transform;

    #endregion

    private void OnEnable()
    {
        CheckSkin();
    }

    
    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        body.velocity = new Vector3(joystick.Horizontal * speed, body.velocity.y, joystick.Vertical * speed);

        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            if(body.velocity != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(body.velocity);
                CurrentAnimator(_skinAnimatorID).SetBool("Run", true);
                // dustParticle.gameObject.SetActive(true);
            }
        }
        else
        {
            CurrentAnimator(_skinAnimatorID).SetBool("Run", false);
           // dustParticle.gameObject.SetActive(false);
        }
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
