using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class JunkCar : MonoBehaviour, IDamageable
{
    [SerializeField] private float damagePerHit;
    [SerializeField] private float maxHealth;
    [SerializeField] private float respawnNoDamageTime;
    [SerializeField] private Image hpFillImage;

    private JunkCarManager _junkCarManager;
    private float _currentHealth;

    public float RespawnNoDamageTime => respawnNoDamageTime;
    public bool CanBeDamaged { get; private set; }

    public void Init(JunkCarManager carManager)
    {
        _currentHealth = maxHealth;
        _junkCarManager = carManager;
        CanBeDamaged = true;
        hpFillImage.gameObject.SetActive(false);
    }

    public void OnRespawn()
    {
        CanBeDamaged = true;
        _currentHealth = maxHealth;
        hpFillImage.gameObject.SetActive(false);
    }

    public void TakeDamage()
    {
        transform.DORewind();
        transform.DOShakeScale(0.2f, 0.2f);

        hpFillImage.gameObject.SetActive(true);
        _currentHealth -= damagePerHit;
        hpFillImage.fillAmount = _currentHealth / maxHealth;

        if (_currentHealth <= 0)
            DestroyCar();
    }

    private void DestroyCar()
    {
        CanBeDamaged = false;
        transform.DOScale(0, 0.3f).OnComplete(OnCarDestroyed);
    }

    private void OnCarDestroyed()
    {
        this.gameObject.SetActive(false);
        _junkCarManager.DestroyCar(this);
        _junkCarManager.ExplodeTiles(this);
    }
}
