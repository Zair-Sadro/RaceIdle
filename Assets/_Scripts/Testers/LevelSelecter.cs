using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelecter : MonoBehaviour
{
    [SerializeField] private Transform[] _levels;
    [SerializeField] private Transform _player;


    public void SelectLevel(int level)
    {
        if (_levels?[level] == null)
            return;

        _player.transform.position = _levels[level].position;

    }
}
