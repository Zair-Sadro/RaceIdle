using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILanguageChange 
{
    public void SubscribeToChange();
    public void ChangeLanguage(string key);
}
