using UnityEngine;
using UnityEngine.UI;

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
        _upgradeUI.OnIncomeUpgraded += FillBar;

        float startLevel = levelsArrayIndex == 1 ? 0 : _maxLevel[levelsArrayIndex - 1];
        fillDelta = 1f / (_maxLevel[levelsArrayIndex] - startLevel);
    }

    //For detection which max level should show after Init
    public void DetectMaxLevel(int currentLevel)
    {
        _currentLevel = currentLevel;

        for (int i = 0; i < _maxLevel.Length; i++)
        {
            if (currentLevel < _maxLevel[i])
            {
                levelsArrayIndex = i;
                finishLevel.text = _maxLevel[i].ToString();
                break;
            }
        }

    }


    private void FillBar(int level)
    {
        _currentLevel = level;
        _fillImage.fillAmount += fillDelta;
        LevelCheck();

    }

    private void LevelCheck()
    {
        if (_currentLevel == _maxLevel[levelsArrayIndex])
        {
            _fillImage.fillAmount = 0;

            ++levelsArrayIndex;
            float startLevel = _maxLevel[levelsArrayIndex - 1];
            fillDelta = 1f / (_maxLevel[levelsArrayIndex] - startLevel);

            string min = startLevel.ToString();
            string max = _maxLevel[levelsArrayIndex].ToString();
            TextChange(min, max);
        }
    }

    private void TextChange(string min, string max)
    {
        finishLevel.text = level + max.ToString(); 
        startLevel.text = level + min.ToString();
    }

    private string level;
    [SerializeField] private string ru = "�����";
    [SerializeField] private string en;
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
