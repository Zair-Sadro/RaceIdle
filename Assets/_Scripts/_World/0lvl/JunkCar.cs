using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JunkCar : MonoBehaviour, IDamageable
{
    [SerializeField] private float damagePerHit;
    [SerializeField] private float maxHealth;
    [SerializeField] private float respawnNoDamageTime;
    [SerializeField] private Image hpFillImage;
    [SerializeField] private ParticleSystem _boomEffect;

    [SerializeField] private GameObject[] carParts;

    [SerializeField] private PlayerSlash playerSlash;
    [SerializeField] private AudioService audioService;
    private JunkCarManager _junkCarManager;
    private float _currentHealth;
    private int _partsIndex;

    private Vector3 _startPos;
    public float RespawnNoDamageTime => respawnNoDamageTime;
    private void Start()
    {
        _startPos = transform.position;
        Shuffle(carParts);
    }
    public void Init(JunkCarManager carManager)
    {
        _currentHealth = maxHealth;
        _partsIndex = 0;
        _junkCarManager = carManager;

        HPBarHide();


    }
    void FixedUpdate()
    {
        hpFillImage.transform.LookAt(Camera.main.transform.position);
    }
    [SerializeField] private float _randDeltaX, _randDeltaZ;
    public void RandomPosition()
    {
        transform.position = new Vector3(
            _startPos.x + Random.Range(-_randDeltaX, _randDeltaX)
            , _startPos.y,
            _startPos.z + Random.Range(-_randDeltaZ, _randDeltaZ));


    }

    public void OnRespawn()
    {
        _currentHealth = maxHealth;
        _destroyed = false;
        _partsIndex = 0;
        HPBarHide();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "weapon")
            TakeDamage(0);
    }

    public void TakeDamage(int damage)
    {
        if (_partsIndex == 5)
            return;
        StartCoroutine(TakeDmg(playerSlash.Damage));
    }
    IEnumerator TakeDmg(int damage)
    {

        carParts[4].transform.DORewind();
        HPBarShow();

        carParts[4].transform.DOShakeScale(0.2f, 1f);

        for (int i = 0; i < damage; i++)
        {

            //Шатаем часть машины и отключаем ее
            var carpart = carParts[_partsIndex++];
            _junkCarManager.ExplodeTile(this);

            ShakeCarPart(carpart);
            audioService.PlayAudo(AudioName.HIT);
            CountDamage();

            if (_currentHealth <= 0)
            {
               // yield return new WaitForSeconds(damage * 0.2f);
                DestroyCar();
                yield break;


            }

        }

        void ShakeCarPart(GameObject carpart)
        {
            carpart.transform
               .DOPunchScale(CarPartChangedSize(carpart.transform), 0.2f)
               .OnComplete(() =>
               {
                   carpart.gameObject.SetActive(false);


               });
        }
    }

    private void CountDamage()
    {
        _currentHealth -= damagePerHit;
        hpFillImage.fillAmount = Mathf.Clamp(_currentHealth, 0, maxHealth) / maxHealth;

    }
    private bool _destroyed;
    private void DestroyCar()
    {
        if (_destroyed) return;
        _boomEffect.Play();
        transform.DOScale(0, 0.4f).OnComplete(OnCarDestroyed);
        _destroyed = true;
    }

    private void OnCarDestroyed()
    {
        this.gameObject.SetActive(false);
        _junkCarManager.DestroyCar(this);
    }
    public List<GameObject> GetCarParts()
    {
        List<GameObject> carParts = new List<GameObject>(this.carParts);
        return carParts;
    }

    private Vector3 CarPartChangedSize(Transform part)
    {
        float randDelta = Random.Range(0.3f, 0.75f);
        var newSize = new Vector3
            (part.localScale.x + randDelta,
            part.localScale.y + randDelta,
            part.localScale.z + randDelta);
        return newSize;
    }

    public static GameObject[] Shuffle(GameObject[] arr)
    {

        for (int i = arr.Length - 2; i >= 0; i--)
        {
            int j = Random.Range(1, i + 1);
            var temp = arr[j];
            arr[j] = arr[i];
            arr[i] = temp;
        }

        return arr;

    }

    #region HPBar visibility (Видимость хп машины)
    bool show;
    private void HPBarShow()
    {
        hpFillImage.gameObject.SetActive(true);

        StopCoroutine(ShowTimer());
        StartCoroutine(ShowTimer());
    }
    private void HPBarHide()
    {
        hpFillImage.gameObject.SetActive(false);
    }
    private IEnumerator ShowTimer()
    {
        yield return new WaitForSeconds(1f);

        if (hpFillImage.isActiveAndEnabled)
            HPBarHide();
    }
    #endregion

}
