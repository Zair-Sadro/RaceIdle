using System.Collections;
using UnityEngine;
using YG;

public class Localization : MonoBehaviour
{

    private void Awake()
    {
        YandexGame.GetDataEvent += CheckTranslate;
    }
    private string current;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.3F);
        CheckTranslate();
    }

    void CheckTranslate()
    {

            var l = YandexGame.EnvironmentData.language;
            GameEventSystem.OnLanguageChange?.Invoke(l);
        
        current = YandexGame.EnvironmentData.language;
        YandexGame.GetDataEvent -= CheckTranslate;

    }
    private void OnEnable()
    {
        CheckTranslate();
    }
}

