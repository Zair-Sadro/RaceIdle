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

    public void RegisterCar(CarAI car)
    {
        _carsOnTrack.Add(car);
        _data.Cars.Add(car.CarData);

    }
    
    public void RegisterCarByData(CarAI car)
    {
        _carsOnTrack.Add(car);

    }

    public void DeleteCar(CarAI car)
    {
        _carsOnTrack.Remove(car);
        _data.Cars.Remove(car.CarData);
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
        return _data;
    }

    public void Initialize(RaceData data)
    {
        _data = data;
        SpawnByData();
    }
    private void SpawnByData() 
    {
        if (_data.Cars.Count < 1)
            return;
        CarData[] cachedData = new CarData[_data.Cars.Count];
         _data.Cars.CopyTo(cachedData);
        foreach (var item in cachedData)
        {
            _carSpawner.Spawn(item.Level, item.CurrentPoint);
        }
       
    }
}
[Serializable]
public class RaceData 
{
    public List<CarData> Cars;
}

