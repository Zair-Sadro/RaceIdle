using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UniqCar : MonoBehaviour, IDamageable
{
    [SerializeField] private float damagePerHit;
    [SerializeField] private float maxHealth;
    [SerializeField] private float respawnNoDamageTime;
    [SerializeField] private Image hpFillImage;
    [SerializeField] private ParticleSystem _boomEffect;
    [SerializeField] private ParticleSystem _hitEffect;

    [SerializeField] private GameObject[] carParts;
    [SerializeField] private Transform _partParent;

    [SerializeField] private PlayerSlash playerSlash;
    [SerializeField] private AudioService audioService;
    private UniqCarsManager _uniqCarManager;

    private float _currentHealth;
    private int _partsDestroyed;
    private int _partsIndex;

    private Vector3 _startPos;
    public float RespawnNoDamageTime => respawnNoDamageTime;
    public bool isFirst;
    private void Start()
    {
        shakecartween = _partParent.DOPunchScale(CarPartChangedSize(_partParent, -0.01f, 0.02f), 0.2f, 100, 100).OnComplete(() => _partParent.DORewind());

        _startPos = transform.position;
        _currentHealth = maxHealth;

        _maxDamageForDestroy = maxHealth / 5f;
        _partsDestroyed = 1;

        Shuffle(carParts);
        HPBarShow();
    }
    public void Init(UniqCarsManager carManager)
    {
        _uniqCarManager = carManager;

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
        _partsDestroyed = 1;

        nextpartIndx = 0;
        damageCountForPartDestroy = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "weapon")
        {
            TakeDamage(playerSlash.Damage);
            var pos = other.transform.position;
            _hitEffect.transform.position = pos;
            _hitEffect.Play();
        }
    }

    private int nextpartIndx;
    private float damageCountForPartDestroy;
    private float _maxDamageForDestroy;

    private Tween shakecartween;
    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        damageCountForPartDestroy += damage;
        bool damageIsGained = damageCountForPartDestroy >= _maxDamageForDestroy;

        audioService.PlayAudo(AudioName.HIT);

        if (!shakecartween.IsPlaying())
            shakecartween = _partParent.DOPunchScale(CarPartChangedSize(_partParent, -0.01f, 0.02f), 0.2f, 50, 0.1f).OnComplete(() => _partParent.DORewind());

        if (damageIsGained)
            for (int i = 0; i < (int)(damageCountForPartDestroy / _maxDamageForDestroy); i++)
            {
                if (_partsDestroyed >= carParts.Length)
                {
                    DestroyCar();
                    return;
                }

                var carpart = carParts[nextpartIndx];
                if (!carpart.activeSelf)
                    return;

                _uniqCarManager.ExplodeTile(this);
                ShakeCarPart(carpart);

                ++nextpartIndx;
                ++_partsDestroyed;
                damageCountForPartDestroy = 0;
            }

        hpFillImage.fillAmount = Mathf.Clamp(_currentHealth / maxHealth, 0f, 1f);

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
    private void DestroyCar()
    {
        _boomEffect.Play();
        transform.DOScale(0, 0.4f).OnComplete(OnCarDestroyed);
    }

    private void OnCarDestroyed()
    {
        this.gameObject.SetActive(false);
        _uniqCarManager.DestroyCar(this);

    }
    public List<GameObject> GetCarParts()
    {
        List<GameObject> carParts = new List<GameObject>(this.carParts);
        return carParts;
    }

    private Vector3 CarPartChangedSize(Transform part, float min = 0.3f, float max = 0.75f)
    {
        float randDelta = Random.Range(min, max);
        var newSize = new Vector3(
            part.localScale.x + randDelta,
            part.localScale.y + randDelta,
            part.localScale.z + randDelta
        );
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

    private void HPBarShow()
    {
        hpFillImage.gameObject.SetActive(true);

    }

}
