using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioService : MonoBehaviour
{
    [SerializeField] private AudioYB _audioPlayer1;
    [SerializeField] private AudioYB _audioPlayer2;
    public void PlayAudo(AudioName name) 
    {
        if(_audioPlayer1.isPlaying) 
        {
            _audioPlayer2.Play(name.ToString(), true);
        }
        else 
        {
            _audioPlayer1.Play(name.ToString(),true);
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
    TILE
}
