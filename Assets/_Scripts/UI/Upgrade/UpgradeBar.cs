using System;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ProBuilder.AutoUnwrapSettings;

public class UpgradeBar : MonoBehaviour,ILanguageChange
{
    [SerializeField] private int[] _maxLevel;

    [SerializeField] private Image _fillImage;
    [SerializeField] TMPro.TMP_Text startLevel, finishLevel;

    [SerializeField] private MachineUpgrade _upgradeUI;


    private int _currentLevel;
    private int levelsArrayIndex = 1;

    private float fillDelta;

    private void Awake()
    {
        SubscribeToChange();
        _upgradeUI.OnSpeedUpgraded += FillBar;
        _upgradeUI.OnDataInit += FillBarFromData;

        float startLevel = levelsArrayIndex == 1 ? 0 : _maxLevel[levelsArrayIndex - 1];
        fillDelta = 1f / (_maxLevel[levelsArrayIndex] - startLevel);
    }

    private void FillBarFromData()
    {

        _currentLevel = _upgradeUI.SpeedUpgradeFields.Level;
        if (_currentLevel == 0)
            return;
        if (_currentLevel < 10) 
        {
            _fillImage.fillAmount = fillDelta * _currentLevel;
        }
        else 
        {
            if (_currentLevel >= 100)
            {

                _fillImage.fillAmount = 1;
                _upgradeUI.OnSpeedUpgraded -= FillBar;
                TextChange(90.ToString(), 100.ToString());
                return;
            }

            var secondDigit = _currentLevel % 10;
            var noSecond = _currentLevel - secondDigit;
            var firstDigit = noSecond / 10;
            levelsArrayIndex = firstDigit + 1;
            var fillAm = secondDigit;
            _fillImage.fillAmount = fillDelta * fillAm;


            float startLevel = _maxLevel[levelsArrayIndex - 1];
            string min = startLevel.ToString();
            string max = _maxLevel[levelsArrayIndex].ToString();
            TextChange(min, max);
        }


    }
    private void FillBar(int level)
    {
        _currentLevel = level;
        _fillImage.fillAmount += fillDelta;
        LevelCheck();

    }
    private bool MaxLevel() 
    {
        if (levelsArrayIndex == _maxLevel.Length)
        {
            _fillImage.fillAmount = 1;
            _upgradeUI.OnSpeedUpgraded -= FillBar;
            return true;
        }

        return false;


    }
    private void LevelCheck()
    {
        if (_currentLevel == _maxLevel[levelsArrayIndex])
        {
            ++levelsArrayIndex;

            if (MaxLevel())
                return;

            _fillImage.fillAmount = 0;

            float startLevel = _maxLevel[levelsArrayIndex - 1];
            fillDelta = 1f / (_maxLevel[levelsArrayIndex] - startLevel);

            string min = startLevel.ToString();
            string max = _maxLevel[levelsArrayIndex].ToString();
            TextChange(min, max);
        }
    }

    private void TextChange(string min, string max)
    {
        finishLevel.text = level + max; 
        startLevel.text = level + min;
    }
    private string level;
    [SerializeField] private string ru = "левел";
    [SerializeField] private string en = "level";
    public void SubscribeToChange()
    {
        GameEventSystem.OnLanguageChange += ChangeLanguage;
    }
    public void ChangeLanguage(string key)
    {
        switch (key)
        {
            case "ru":
                level = ru;

                break;

            case "en":
                level = en;

                break;
        }

    }
}
