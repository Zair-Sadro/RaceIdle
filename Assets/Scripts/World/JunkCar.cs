using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;

public class JunkCar : MonoBehaviour, IDamageable
{
    [SerializeField] private float damagePerHit;
    [SerializeField] private float maxHealth;
    [SerializeField] private float respawnNoDamageTime;
    [SerializeField] private Image hpFillImage;

    [SerializeField] private GameObject[] carParts;

    private JunkCarManager _junkCarManager;
    private float _currentHealth;
    private int _partsIndex;

    public float RespawnNoDamageTime => respawnNoDamageTime;
    public bool CanBeDamaged { get; private set; }
    private void Start()
    {
        Shuffle(carParts);
    }
    public void Init(JunkCarManager carManager)
    {
        _currentHealth = maxHealth;
        _partsIndex =0;
        _junkCarManager = carManager;
        CanBeDamaged = true;
        HPBarHide();
    }

    public void OnRespawn()
    {
        CanBeDamaged = true;
        _currentHealth = maxHealth;
        _partsIndex = 0;
        HPBarHide();
    }

    public void TakeDamage(float delay)
    {
        transform.DORewind();
        HPBarShow();

        transform.DOShakeScale(0.2f, 0.2f)
            .SetDelay(delay)
            .OnComplete(() => 
            {

             //Шатаем часть машины и отключаем ее
             var carpart = carParts[_partsIndex++];
             carpart.transform
                .DOPunchScale(CarPartChangedSize(carpart.transform), 0.3f)
                .OnComplete(() =>
                {
                    carpart.gameObject.SetActive(false);
                });


             _currentHealth -= damagePerHit;
             hpFillImage.fillAmount = _currentHealth / maxHealth;
             _junkCarManager.ExplodeTile(this);

             if (_currentHealth <= 0)
          
                DestroyCar();
            });

     
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
       // _junkCarManager.ExplodeTiles(this);
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
            (part.localScale.x+ randDelta,
            part.localScale.y+ randDelta,
            part.localScale.z+ randDelta);
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
