using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class JunkCar : MonoBehaviour, IDamageable
{
    [SerializeField] private float damagePerHit;
    [SerializeField] private float maxHealth;
    [SerializeField] private Image hpFillImage;

    private JunkCarManager _junkCarManager;
    private float _currentHealth;


    public void Init(JunkCarManager carManager)
    {
        _currentHealth = maxHealth;
        _junkCarManager = carManager;
        hpFillImage.gameObject.SetActive(false);
    }

    public void TakeDamage()
    {
        Debug.Log("Hey");
        transform.DORewind();
        transform.DOShakeScale(0.3f, 1);

        hpFillImage.gameObject.SetActive(true);
        _currentHealth -= damagePerHit;
        hpFillImage.fillAmount = _currentHealth / maxHealth;

        if (_currentHealth <= 0)
            DestroyCar();
    }

    private void DestroyCar()
    {
        _junkCarManager.ExplodeTiles(this);
        transform.DOScale(0, 0.5f).OnComplete(() => this.gameObject.SetActive(false));//need to do list 
    }
}
