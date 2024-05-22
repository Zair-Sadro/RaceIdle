using DG.Tweening;
using UnityEngine;

public class JunkCar : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float respawnNoDamageTime;

    [SerializeField] private ParticleSystem _boomEffect;
    [SerializeField] private ParticleSystem _hitEffect;

    [SerializeField] private Transform _partParent;
    [SerializeField] private GameObject[] carParts; public GameObject[] GetCarParts() => carParts;


    [SerializeField] private PlayerSlash playerSlash;
    [SerializeField] private AudioService audioService;

    private JunkCarManager _junkCarManager;
    private int _partsDestroyed;

    private Vector3 _startPos;

    public float RespawnNoDamageTime => respawnNoDamageTime;

    private void Start()
    {

        shakecartween = _partParent.DOPunchScale(CarPartChangedSize(_partParent, -0.01f, 0.02f), 0.2f, 100, 100).OnComplete(() => _partParent.DORewind());

        _startPos = transform.position;
        _maxDamageForDestroy = maxHealth / 5f;
        _partsDestroyed = 1;

        Shuffle(carParts);
    }

    public void Init(JunkCarManager carManager)
    {
        _junkCarManager = carManager;
    }
    
    public void RandomPosition()
    {
        transform.position = new Vector3(
            _startPos.x + Random.Range(-1f, 1f),
            _startPos.y,
            _startPos.z + Random.Range(-1f, 1f)
        );
    }

    public void OnRespawn()
    {
        _partsDestroyed = 1;

        nextpartIndx = 0;
        damageCountForPartDestroy = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("weapon")) 
        {
            TakeDamage(playerSlash.Damage);
            var pos = other.transform.position;
            _hitEffect.transform.position= pos;
            _hitEffect.Play();
        }
           
    }

    private int nextpartIndx;
    private float damageCountForPartDestroy;
    private float _maxDamageForDestroy;

    private Tween shakecartween;
    public void TakeDamage(int damage)
    {
        damageCountForPartDestroy += damage;
        bool damageIsGained = damageCountForPartDestroy >= _maxDamageForDestroy;

        audioService.PlayAudo(AudioName.HIT);
 
        if(!shakecartween.IsPlaying())
            shakecartween = _partParent.DOPunchScale(CarPartChangedSize(_partParent,-0.01f,0.02f), 0.2f,50,0.1f).OnComplete(()=> _partParent.DORewind());

        if (damageIsGained)
            for (int i = 0; i < (int)(damageCountForPartDestroy / _maxDamageForDestroy); i++)
            {
                if (_partsDestroyed >= carParts.Length)
                {
                    _junkCarManager.ExplodeTile(this);
                    DestroyCar();
                    return;
                }

                var carpart = carParts[nextpartIndx];
                if (!carpart.activeSelf)
                    return;

                _junkCarManager.ExplodeTile(this);
                ShakeCarPart(carpart);

                ++nextpartIndx;
                ++_partsDestroyed;
                damageCountForPartDestroy = 0;
            }
    }

    private void DestroyCar()
    {
        _boomEffect.Play();
        _junkCarManager.DestroyCar(this);
    }

    private void ShakeCarPart(GameObject carpart)
    {


        carpart.transform
            .DOPunchScale(CarPartChangedSize(carpart.transform), 0.2f)
            .OnComplete(() =>
            {
                carpart.gameObject.SetActive(false);
            });
    }

    private Vector3 CarPartChangedSize(Transform part, float min = 0.3f, float max = 0.75f)
    {
        float randDelta = Random.Range(min, max);
        var localScale = part.localScale;
        var newSize = new Vector3(
            localScale.x + randDelta,
            localScale.y + randDelta,
            localScale.z + randDelta
        );
        return newSize;
    }

    private void Shuffle(GameObject[] arr)
    {
        for (int i = arr.Length - 2; i >= 0; i--)
        {
            int j = Random.Range(1, i + 1);
            (arr[j], arr[i]) = (arr[i], arr[j]);
        }
        
    }

}
