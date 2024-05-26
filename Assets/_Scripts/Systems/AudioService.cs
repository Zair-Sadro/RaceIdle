
using System;
using System.Collections;
using UnityEngine;


public class AudioService : MonoBehaviour
{
    [SerializeField] private AudioYB _audioPlayer1;
    [SerializeField] private AudioYB _audioPlayer2;
    [SerializeField] private AudioYB _musicPlayer;
    [SerializeField] private float _backGroundMusicVolume = 0.4f;
    [SerializeField] private float _speedOfVolumeChange = 0.4f;

    private const string BACK1 = "BACK1";
    private const string BACK2 = "BACK2";
    private const string BACK3 = "BACK3";

    private void Start()
    {
        _musicPlayer.volume = 0;
       StartCoroutine(LoopedBackgroundMusic());
    }

    IEnumerator LoopedBackgroundMusic()
    {
        var wait55sec = new WaitForSeconds(55f);
        
        _musicPlayer.Play(BACK3);
        
        while (true)
        {
            while (_musicPlayer.volume < _backGroundMusicVolume)
            {
                _musicPlayer.volume += _speedOfVolumeChange * Time.deltaTime;
                yield return null;
            }

            yield return wait55sec;
        
            while (_musicPlayer.volume > 0.05f)
            {
                _musicPlayer.volume -= _speedOfVolumeChange * Time.deltaTime;
                yield return null;
            }
            
            _musicPlayer.Play(NextBackGroundMusic());
            yield return null;
        }
        
    }

    private byte _indxOfBackMusic;

    private string NextBackGroundMusic()
    {
        switch (_indxOfBackMusic)
        {
            case 0:
                _indxOfBackMusic = 1;
                return BACK1;
            
            case 1:
                _indxOfBackMusic = 2;
                return BACK2;

            case 2:
                _indxOfBackMusic = 0;
                return BACK3;

        }
        return BACK1;
    }

    public void PlayAudo(AudioName audioName) 
    {
        if(_audioPlayer1.isPlaying)
        {
            _audioPlayer2.Play(audioName.ToString(), true);
        }
        else 
        {
            _audioPlayer1.Play(audioName.ToString(),true);
        }
    }

}
public enum AudioName
{
    HIT,
    MERGE,
    BUILD,
    FINISHRACE,
    FINISHRACE2,
    NEWZONE,
    SHOP,
    CAR,
    SKID,
    TILE,
    BACK1,
    BACK2,
    BACK3
}
