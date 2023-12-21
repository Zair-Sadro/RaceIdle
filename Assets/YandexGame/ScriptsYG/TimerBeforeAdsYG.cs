using System;
using System.Collections;
using UnityEngine;
using YG;

public class TimerBeforeAdsYG : MonoBehaviour
{
    [SerializeField] private GameObject secondsPanelObject;
    [SerializeField] private GameObject[] secondObjects;

    private Action reward;
    private void Start()
    {
        #region FullScreen
        YandexGame.Instance.CloseFullscreenAd.AddListener(() =>
        {
            CloseAllObj();
            Time.timeScale = 1;
            AudioListener.volume = 1;
        });

        YandexGame.Instance.ErrorFullscreenAd.AddListener(() =>
        {
            CloseAllObj();
            Time.timeScale = 1;
            AudioListener.volume = 1;
        });
        #endregion

        #region RewardAdd
        YandexGame.Instance.ErrorVideoAd.AddListener(() =>
        {

            Time.timeScale = 1;
            AudioListener.volume = 1;
        });
        YandexGame.Instance.CloseVideoAd.AddListener(() =>
        {

            Time.timeScale = 1;
            AudioListener.volume = 1;
        });
        YandexGame.Instance.RewardVideoAd.AddListener(() =>
        {

            Time.timeScale = 1;
            AudioListener.volume = 1;
            reward.Invoke();
        });
        
        #endregion
    }
    public void TryToShowAdd()
    {
        if (YandexGame.timerShowAd >= YandexGame.Instance.infoYG.fullscreenAdInterval)
        {
            StartCoroutine(TimerAdShow());
        }


    }
    public void TryRewardVideo(Action reward) 
    {
        this.reward = reward;
        YandexGame.RewVideoShow(0);
    }

    IEnumerator TimerAdShow()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        Time.timeScale = 0;
        AudioListener.volume = 0;

        secondsPanelObject.SetActive(true);

        for (int i = 0; i < secondObjects.Length; i++)
        {
            secondObjects[i].SetActive(true);

            yield return new WaitForSecondsRealtime(1f);
            secondObjects[i].SetActive(false);
        }

        YandexGame.FullscreenShow();
        yield return new WaitForSecondsRealtime(2.5f);
        CloseAllObj();
    }
    private void CloseAllObj()
    {
        if (!secondsPanelObject.activeInHierarchy)
            return;

        secondsPanelObject.SetActive(false);
        for (int i = 0; i < secondObjects.Length; i++)
        {
            secondObjects[i].SetActive(false);

        }
    }

}
