using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarShopUI : UIPanel
{
    [SerializeField] private ScrollRect _scroll;
    [SerializeField] private List<CarButton> _carButtons;
    [SerializeField] private GameObject _waitBlockImage;
    [SerializeField] private Button _closeButt;
    [SerializeField] private float _showSpeed=0.9f;
    public void OpenPanel()
    {
        Open();
        _scroll.verticalScrollbar.value = 1;
    
    }

    protected override void Awake()
    {
        base.Awake();
        PanelInit(GetPanelAnimation());
    }
    protected override void Start()
    {
        base.Start();
        _closeButt.onClick.AddListener(Close);

        GameEventSystem.OnCarBought += (s) => WaitForCar();

        foreach (var button in _carButtons)
            button.Init();
     
    }

    private void WaitForCar()
    {
        StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {
        _waitBlockImage.SetActive(true);
        yield return new WaitForSeconds(1f);
        _waitBlockImage.SetActive(false);
    }
    private Tween GetPanelAnimation()
    {
        return s_canvasGroup.DOFade(1, _showSpeed).Pause();

    }
}
