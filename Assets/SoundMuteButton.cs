using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundMuteButton : MonoBehaviour
{
    [SerializeField] private Button soundButton;
    [SerializeField] private GameObject _muteIcon, _soundIcon;
    [SerializeField] private AudioSource _source1, _source2;
    [SerializeField] private RaceTrackManager _carsManager;
    private bool _muted;

    void Start()
    {
        soundButton.onClick.AddListener(SoundAction);
    }

    private void SoundAction()
    {
        if(_muted) 
        {
            _muteIcon.SetActive(false);
            _soundIcon.SetActive(true);

            _muted = false;
            _source1.mute = false;
            _source2.mute = false;

            _carsManager.MuteCarSound(false);

        }
        else 
        {
            _soundIcon.SetActive(false);
            _muteIcon.SetActive(true);

            _muted = true;
            _source1.mute = true;
            _source2.mute = true;

            _carsManager.MuteCarSound(true);

        }
    }
}
