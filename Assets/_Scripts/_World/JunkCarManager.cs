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
        int rX = Random.Range(-4, 4);
        int rZ = Random.Range(-4, 4);
        tile.transform.position = car.transform.position;
        tile.gameObject.SetActive(true);
        var pos = new Vector3(car.transform.position.x+rX, car.transform.position.y + 2f, car.transform.position.z + rZ);
        tile.ThrowTo(pos, 0.5f);
        
    }

    public void DestroyCar(JunkCar car)
    {
        StartCoroutine(CarRespawn(spawnCoolDown, car));
    }

    private IEnumerator CarRespawn(float time, JunkCar car)
    {
        yield return new WaitForSeconds(time);
        var parts = car.GetCarParts();

        //Parts
        for (int i = 0; i < parts.Count; i++)
        {
            parts[i].gameObject.SetActive(true);
        }

        //Rotation and RandomPosition
        car.RandomPosition();
        var eulr = car.transform.eulerAngles;
        car.transform.Rotate(eulr.x, eulr.y+Random.Range(0f,180f), eulr.z);
       

        //SetActive
        car.gameObject.SetActive(true);
        car.transform.DOScale(1, car.RespawnNoDamageTime-0.1f);

        yield return new WaitForSeconds(car.RespawnNoDamageTime);
        car.OnRespawn();
    }
}
