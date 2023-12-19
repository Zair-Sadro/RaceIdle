using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : UIPanel
{
    [SerializeField] private List<GameObject> _objectsActivatePanel;
    [SerializeField] private List<UpgradeUI> _panels;
    [SerializeField] private List<Button> _buttons;
    [SerializeField] private Button _backButton;

    private int _current = 0;
    [SerializeField] private StatsValuesInformator _statsInfromator;

    protected override void Awake()
    {
        base.Awake();
        PanelInit(GetPanelAnimation());

        foreach (var obj in _objectsActivatePanel)
        {
            var iobj = obj.GetInterface<IObjectClickedActivateNextObject>();
            iobj.OnObjectClicked += SelectPanelByObj;

        }

        for (int i = 0; i < _buttons.Count; i++)
        {
            var indx = i;
            _buttons[i].onClick.AddListener(() => SelectPanel(indx));
        }
        _backButton.onClick.AddListener(Close);
    }

    protected override void Start()
    {

    }


    public void SelectPanel(int upgradeUI)
    {
        _panels[_current].Close();
        _panels[upgradeUI].Open();
        _current = upgradeUI;
    }
    public void SelectPanelByObj(int upgradeUI)
    {
        CheckBuilds();
        Open(); // Open UpgradePanel - parent of all _panels

        _panels[_current].Close();
        _panels[upgradeUI].Open();
        _current = upgradeUI;
    }
    private void CheckBuilds()
    {
        var list = _statsInfromator.GetBuiltMachines();
        for (int i = 0; i < list.Count; i++)
        {
            _buttons[list[i]].interactable = true;
        }
    }

    [SerializeField, Tooltip("Скорость появления панели")]
    private float _showSpeed = 0.5f;
    private Tween GetPanelAnimation()
    {
        return s_canvasGroup.DOFade(1, _showSpeed).Pause();

    }

}


