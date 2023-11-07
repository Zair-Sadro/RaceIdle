using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioService : MonoBehaviour
{
    [SerializeField] private AudioYB _audioPlayer1;
    [SerializeField] private AudioYB _audioPlayer2;
    public void PlayAudo(AudioName name) 
    {
        if(_audioPlayer1.isPlaying) 
        {
            _audioPlayer2.Play(name.ToString());
        }
        else 
        {
            _audioPlayer1.Play(name.ToString());
        }
    }
}
public enum AudioName
{
    MERGE,
    BUILD,
    FINISHRACE,
    FINISHRACE2,
    NEWZONE,
    SHOP,
    TILE
}
