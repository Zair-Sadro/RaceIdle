using System;
using UnityEngine;
using UnityEngine.UI;
using YG;

[Serializable]
public class SimpleComics : IComics
{
    [SerializeField] private string _comicsID; 
    [SerializeField] private Sprite _image;
    [SerializeField,TextArea] private string _ruComicsText;
    [SerializeField,TextArea] private string _engComicsText;

    public Sprite ComicsPicture => _image;
    public string ComicsText
    {
        get { return YandexGame.EnvironmentData.language == "ru" ? _ruComicsText : _engComicsText; }
    }
}