using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicsService : MonoBehaviour
{
   [SerializeField] private ComicsPageView _comicsUI;

   public void ShowComics(IComics comics)
   {
      _comicsUI.SetComics(comics);
      _comicsUI.OpenComics();
   }
}
