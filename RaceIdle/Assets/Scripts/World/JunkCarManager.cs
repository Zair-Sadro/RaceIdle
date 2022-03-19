using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkCarManager : MonoBehaviour
{
    [SerializeField] private JunkTilesSpawn tileSpawn;
    [SerializeField] private List<JunkCar> junkCars = new List<JunkCar>();

    private void Start()
    {
        InitCars();
    }

    private void InitCars()
    {
        for (int i = 0; i < junkCars.Count; i++)
            junkCars[i].Init(this);
    }

    public void ExplodeTiles(JunkCar car)
    {
        var tileList = tileSpawn.GetRandomJunkTiles(3, 5);

        for (int i = 0; i < tileList.Count; i++)
        {
            tileList[i].transform.localPosition = car.transform.position;
            tileList[i].gameObject.SetActive(true);
        }
    }
}
