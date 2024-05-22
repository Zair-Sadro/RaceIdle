
using UnityEngine;


public class AudioService : MonoBehaviour
{
    [SerializeField] private AudioYB _audioPlayer1;
    [SerializeField] private AudioYB _audioPlayer2;
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
    TILE
}
