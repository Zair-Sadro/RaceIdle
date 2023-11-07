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


       // var gameData = (RaceIdleData) FileIOUtility.ReadFromJson<RaceIdleData>("save._data");
        //if (gameData != null)
        //{
           
        //    Debug.Log("Save _data has loaded successfully!");
        //}
        //else Debug.Log("Save _data has not loaded!");

    }
    private void LoadData()
    {

        if (!isEnable)
            return;

        RaceIdleData raceIdleData = new RaceIdleData();
        raceIdleData = YandexGame.savesData.GetMainGameData();

        game.Initialize(raceIdleData);
    }

}