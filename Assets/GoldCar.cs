using DG.Tweening;
using System.Collections;
using UnityEngine;

public class GoldCar : MonoBehaviour
{
    [SerializeField] private float maxHealth;

    [SerializeField] private ParticleSystem _boomEffect;
    [SerializeField] private AudioService audioService => InstantcesContainer.Instance.AudioService;
    [SerializeField] private Transform _goldCoin;
    WalletSystem _wallet => InstantcesContainer.Instance.WalletSystem;
    [SerializeField] private CarAI _carAI;
    [SerializeField] private Transform arrow;
     
    private float _currentHealth;
    public CarSpawner carSpawner;
    public Transform _startCoinPos;
    public bool isFirst;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "weapon")
            TakeDamage(0);
    }
    private void OnEnable()
    {
        _currentHealth = maxHealth;
        arrow.DOMoveY(5, 0.7f).SetLoops(-1, LoopType.Yoyo);
    }
    private void OnDisable()
    {
        arrow.DOKill();
    }
    private void LateUpdate()
    {
        arrow.LookAt(Camera.main.transform.position);
    }
    public void TakeDamage(int damage)
    {
        StartCoroutine(TakeDmg(1));
    }
    IEnumerator TakeDmg(int damage)
    {

        this.transform.DORewind();
        this.transform.DOShakeScale(0.2f, 1f);
        GoldCoinAnim();

        for (int i = 0; i < damage; i++)
        {
            _wallet.Income(2f);

            audioService.PlayAudo(AudioName.HIT);
            CountDamage();

            if (_currentHealth <= 0)
            {
                yield return new WaitForSeconds(damage * 0.2f);
                DestroyCar();
                yield break;


            }

        }
        void GoldCoinAnim()
        {

            _goldCoin.transform.localPosition = _startCoinPos.localPosition;
            _goldCoin.gameObject.SetActive(true);

            _goldCoin.transform.DOMoveY(_goldCoin.transform.localPosition.y + 7f, 0.5f).OnComplete(() =>
            {
                _goldCoin.gameObject.SetActive(false);
            });
        }
    }


    private void CountDamage()
    {
        _currentHealth -= 1;

    }
    private bool _destroyed;


    private void DestroyCar()
    {
        if (_destroyed) return;
        _boomEffect.Play();
        transform.DOScale(0, 0.4f).OnComplete(() => StartCoroutine(OnCarDestroyed()));
        _carAI.StopDrive();
        _destroyed = true;
    }

    private IEnumerator OnCarDestroyed()
    {
        yield return new WaitForSeconds(10f);

        carSpawner.SpawnGoldCar();
        yield return new WaitForSeconds(0.2f);
        Destroy(this.gameObject);

    }

}
