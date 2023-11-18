using System;
using UnityEngine;
using UnityEngine.Playables;
using YG;

public class SaveLoadController : MonoBehaviour
{
    public bool isEnable = true;

    private RaceIdleGame game => InstantcesContainer.Instance.RaceIdleGame;
    //read _data when open the app
    private void OnEnable()
    {
        YandexGame.GetDataEvent += LoadData;
        GameEventSystem.NeedToSaveProgress += SaveProgress;

       // var gameData = (RaceIdleData) FileIOUtility.ReadFromJson<RaceIdleData>("save._data");
        //if (gameData != null)
        //{
           
        //    Debug.Log("Save _data has loaded successfully!");
        //}
        //else Debug.Log("Save _data has not loaded!");

    }

    private void SaveProgress()
    {
        YandexGame.savesData.SetMainGameData(game.GetData());
        YandexGame.SaveProgress();
    }

    private void LoadData()
    {

        if (!isEnable)
            return;

        RaceIdleData raceIdleData = new RaceIdleData();
        raceIdleData = YandexGame.savesData.GetMainGameData();

        game.Initialize(raceIdleData);
    }
    private void OnApplicationQuit()
    {
        SaveProgress();
    }
}