using UnityEngine;

public class SaveLoadController : MonoBehaviour
{
    public bool isEnable = true;

    private RaceIdleGame game => InstantcesContainer.Instance.RaceIdleGame;
    //read _data when open the app
    private void OnEnable()
    {

        if (!isEnable)
            return;

        var gameData = (RaceIdleData) FileIOUtility.ReadFromJson<RaceIdleData>("save._data");
        if (gameData != null)
        {
            game.Initialize(gameData);
            Debug.Log("Save _data has loaded successfully!");
        }
        else Debug.Log("Save _data has not loaded!");

    }

    // save _data when close the app
    private void OnApplicationPause(bool pauseStatus)
    {

        if (!isEnable)
            return;
        if (pauseStatus)
        {
            FileIOUtility.WriteToJson("save._data", game.GetData());
            Debug.Log("Game _data has saved successfully!");
        }

       
    }

    private void OnApplicationQuit()
    {

        if (!isEnable)
            return;
        FileIOUtility.WriteToJson("save._data", game.GetData());
        Debug.Log("Game _data has saved successfully!");
        //DOTween.KillAll();
    }
}