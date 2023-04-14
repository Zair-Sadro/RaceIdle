using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RaceTrackManager : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameObject raceTrackCamera;
    [SerializeField] private GameObject joystick;
    [SerializeField] private GameObject uiCamera;

    [Header("UI")] 
    [SerializeField] private Toggle toggle;
    [SerializeField] private Image toggleImage;

    [SerializeField] private Sprite topArrow;
    [SerializeField] private Sprite downArrow;

    [Header("Events")] 
    public UnityEvent startDragEvent;
    public UnityEvent endDragEvent;

    private void Awake()
    {
        if (toggle) toggle.onValueChanged.AddListener(GoToTheRaceTrack);
    }

    private void GoToTheRaceTrack(bool isOn)
    {
        raceTrackCamera.SetActive(isOn);
        joystick.SetActive(!isOn);
        uiCamera.SetActive(!isOn);

        toggleImage.sprite = isOn ? downArrow : topArrow;
    }
}
