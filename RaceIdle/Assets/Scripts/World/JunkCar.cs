using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class JunkCar : MonoBehaviour, IDamageable
{
    [SerializeField] private UserData data;// temp
    [SerializeField] private float maxHealth;
    [SerializeField] private Image hpFillImage;

    private float _currentHealth;
    private Transform _transform;

    private void Start()
    {
        Init();
        _transform = transform;
    }

    public void Init()
    {
        _currentHealth = maxHealth;
        hpFillImage.gameObject.SetActive(false);
    }

    public void TakeDamage()
    {
        _transform.DORewind();
        _transform.DOShakeScale(0.3f, 1);

        hpFillImage.gameObject.SetActive(true);
        _currentHealth -= data.HammerDamage;
        hpFillImage.fillAmount = _currentHealth / maxHealth;

        if (_currentHealth <= 0)
            DestroyCar();
    }

    private void DestroyCar()
    {
        _transform.DOScale(0, 0.5f).OnComplete(() => this.gameObject.SetActive(false));//need to do list 
    }
}
