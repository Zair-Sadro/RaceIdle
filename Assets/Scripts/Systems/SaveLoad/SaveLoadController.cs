using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class SaveLoadController : MonoBehaviour
{
    public bool isEnable = true;

    [Zenject.Inject] private RaceIdleGame game;
    //read data when open the app
    private void OnEnable()
    {

        if (!isEnable)
            return;

        var gameData = (RaceIdleData) FileIOUtility.ReadFromJson<RaceIdleData>("save.data");
        if (gameData != null)
        {
            game.Initialize(gameData);
            Debug.Log("Save data has loaded successfully!");
        }
        else Debug.Log("Save data has not loaded!");

    }

    // save data when close the app
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            FileIOUtility.WriteToJson("save.data", game.GetData());
            Debug.Log("Game data has saved successfully!");
        }

       
    }

    private void OnApplicationQuit()
    {
        FileIOUtility.WriteToJson("save.data", game.GetData());
        Debug.Log("Game data has saved successfully!");
        //DOTween.KillAll();
    }
}