using UnityEngine;
using YG;

public class SaveLoadController : MonoBehaviour
{
    private bool _isEnable = true;

    private RaceIdleGame game => InstantcesContainer.Instance.RaceIdleGame;

    public bool IsEnable { get => _isEnable; set => _isEnable = value; }

    //read _data when open the app
    private void OnEnable()
    {
        YandexGame.GetDataEvent += LoadData;
        GameEventSystem.NeedToSaveProgress += _SaveProgress;

        // var gameData = (RaceIdleData) FileIOUtility.ReadFromJson<RaceIdleData>("save._data");
        //if (gameData != null)
        //{

        //    Debug.Log("Save _data has loaded successfully!");
        //}
        //else Debug.Log("Save _data has not loaded!");

    }

    private void _SaveProgress()
    {
        if (!IsEnable)
        {
            YandexGame.savesData.SetMainGameData(new RaceIdleData());
            YandexGame.SaveProgress();
        }
        else
        {
            YandexGame.savesData.SetMainGameData(game.GetData());
            YandexGame.SaveProgress();
        }



    }

    private void LoadData()
    {

        RaceIdleData raceIdleData = new RaceIdleData();
        raceIdleData = YandexGame.savesData.GetMainGameData();

        game.Initialize(raceIdleData);
    }
    private void OnApplicationQuit()
    {
        _SaveProgress();
    }
}