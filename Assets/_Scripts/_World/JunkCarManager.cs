using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class JunkCarManager : MonoBehaviour
{
    [SerializeField] private float spawnCoolDown;
    [SerializeField] private List<JunkCar> junkCars = new List<JunkCar>();

    [Inject] private ResourceTilesSpawn tileSpawn;


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
        var tileList = tileSpawn.GetRandomTiles(3, 5);
        var pos = new Vector3(car.transform.position.x, car.transform.position.y + 1f, car.transform.position.z);

        for (int i = 0; i < tileList.Count; i++)
        {
            tileList[i].transform.localPosition = pos;
            tileList[i].gameObject.SetActive(true);
        }
    }
    public void ExplodeTile(JunkCar car)
    {
        var tile = tileSpawn.GetTile(TileType.Junk);
        var pos = new Vector3(car.transform.position.x, car.transform.position.y + 2f, car.transform.position.z);
        tile.transform.localPosition = pos;
        tile.gameObject.SetActive(true);
    }

    public void DestroyCar(JunkCar car)
    {
        StartCoroutine(CarRespawn(spawnCoolDown, car));
    }

    private IEnumerator CarRespawn(float time, JunkCar car)
    {
        yield return new WaitForSeconds(time);
        var parts = car.GetCarParts();

        for (int i = 0; i < parts.Count; i++)
        {
            parts[i].gameObject.SetActive(true);
        }
        car.gameObject.SetActive(true);
        car.transform.DOScale(1, car.RespawnNoDamageTime-0.1f);

        yield return new WaitForSeconds(car.RespawnNoDamageTime);
        car.OnRespawn();
    }
}
