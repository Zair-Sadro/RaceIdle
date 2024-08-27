
using UnityEngine;
using UnityEngine.UI;

public class SoundMuteButton : MonoBehaviour
{
    [SerializeField] private Button soundButton;
    [SerializeField] private GameObject _muteIcon, _soundIcon;
    [SerializeField] private AudioSource[] _audioSources;
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
            foreach (var audioSource in _audioSources)
                audioSource.mute = false;


            _carsManager.MuteCarSound(false);

        }
        else 
        {
            _soundIcon.SetActive(false);
            _muteIcon.SetActive(true);

            _muted = true;
            foreach (var audioSource in _audioSources)
                audioSource.mute = true;

            _carsManager.MuteCarSound(true);

        }
    }
}
