using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceTrackManager : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameObject raceTrackCamera;
    [SerializeField] private GameObject joystick;

    [SerializeField] private List<CarAI> _carsOnTrack=new();

    [SerializeField] private Button _toRaceCameraButt;
    [SerializeField] private Button _toDefaultCamera;

    private void Awake()
    {
        _toRaceCameraButt?.onClick.AddListener(() => GoToTheRaceTrack(true));
        _toDefaultCamera?.onClick.AddListener(() => GoToTheRaceTrack(false));
        _toDefaultCamera.gameObject.SetActive(false);
    }

    private void GoToTheRaceTrack(bool isOn)
    {
        raceTrackCamera.SetActive(isOn);
        joystick.SetActive(!isOn);
        _toDefaultCamera.gameObject.SetActive(isOn);

    }

    public void RegisterCar(CarAI car)
    {
        _carsOnTrack.Add(car);

    }
    
    public void DeleteCar(CarAI car)
    {
        _carsOnTrack.Remove(car);
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
}
