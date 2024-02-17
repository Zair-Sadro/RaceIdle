using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceTrackManager : MonoBehaviour,ISaveLoad<RaceData>
{
    [Header("Objects")]
    [SerializeField] private GameObject raceTrackCamera;
    [SerializeField] private GameObject joystick;

    [SerializeField] private CarSpawner _carSpawner;

    [SerializeField] private List<CarAI> _carsOnTrack=new();

    [SerializeField] private Button _toRaceCameraButt;
    [SerializeField] private Button _toDefaultCamera;
    [SerializeField] private AudioListener _audioListenerForOff;

    private RaceData _data=new();

    public event Action<bool> NoSpaceOnTrack;
    private void Awake()
    {
        _toRaceCameraButt?.onClick.AddListener(() => GoToTheRaceTrack(true));
        _toDefaultCamera?.onClick.AddListener(() => GoToTheRaceTrack(false));
        _toDefaultCamera.gameObject.SetActive(false);
        _data.Cars = new();
    }

    private void GoToTheRaceTrack(bool toRace)
    {
        _audioListenerForOff.enabled = !toRace;
        joystick.SetActive(!toRace);

        raceTrackCamera.SetActive(toRace);      
        _toDefaultCamera.gameObject.SetActive(toRace);

    }

    private bool audioMute;
    public void MuteCarSound(bool mute) 
    {
        audioMute = mute;
        foreach (var item in _carsOnTrack)
        {
            item.Mute(mute);
        }
    }

    public void RegisterCar(CarAI car)
    {
        _carsOnTrack.Add(car);
        car.Mute(audioMute);
        CheckCount();
    }
    
    public void RegisterCarByData(CarAI car)
    {
        _carsOnTrack.Add(car);
        CheckCount();
    }

    public void DeleteCar(CarAI car)
    {
        _carsOnTrack.Remove(car);
        _data.Cars.Remove(car.CarData);
        CheckCount();
    }

    public void StopCars() 
    {
        foreach (var car in _carsOnTrack)
        {
            car.StopDrive();
        }
        
    }
    public void StartCars() 
    {
        foreach (var car in _carsOnTrack)
        {
            car.StartDrive();
        }
    }

    public RaceData GetData()
    {
        _data = new();
        foreach (var item in _carsOnTrack)
        {
            _data.Cars.Add(item.CarData);
        }
        return _data;
    }

    public void Initialize(RaceData data)
    {
        _data = data;
        SpawnByData();
    }
    private void SpawnByData() 
    {
        if (_data.Cars.Count == 0)
        {
            _carSpawner.SpawnFirstCar();
        }
        else if (_data.Cars.Count == 1)
        {
            if (_data.Cars[0].LapReward==0)
                _carSpawner.SpawnFirstCar();
            else
                _carSpawner.SpawnByData(_data.Cars);

        }
        else
        {
            _carSpawner.SpawnByData(_data.Cars);
        }
        
       
    }
    private void CheckCount() 
    {
       bool nospace = _carsOnTrack.Count >= 20 ? true : false;
        NoSpaceOnTrack.Invoke(nospace);
    }
}
[Serializable]
public class RaceData 
{
    public List<CarData> Cars = new();

}

