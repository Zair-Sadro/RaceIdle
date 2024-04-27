using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqCarsManager :MonoBehaviour
{
    [SerializeField] private float spawnCoolDown;
    [SerializeField] private float spawnCoolDownForFirstCar;
    [SerializeField] private List<UniqCar> uniqCars = new List<UniqCar>();

    private ResourceTilesSpawn tileSpawn => InstantcesContainer.Instance.ResourceTilesSpawn;


    private void Start()
    {
        InitCars();
    }

    private void InitCars()
    {
        for (int i = 0; i < uniqCars.Count; i++)
            uniqCars[i].Init(this);
    }

    public void ExplodeTile(UniqCar car)
    {
        var tile = tileSpawn.GetRandomTile();
        int rX = Random.Range(-4, 4);
        int rZ = Random.Range(-4, 4);
        tile.transform.position = car.transform.position;
        tile.gameObject.SetActive(true);
        var pos = new Vector3(car.transform.position.x + rX, car.transform.position.y + 2f, car.transform.position.z + rZ);
        tile.ThrowTo(pos, 0.5f,true);
    }
    public void DestroyCar(UniqCar car)
    {
        if (car.isFirst)
        {
            StartCoroutine(CarRespawn(spawnCoolDownForFirstCar, car));
        }
        else
        {
            StartCoroutine(CarRespawn(spawnCoolDown, car));
        }
  
    }

    private IEnumerator CarRespawn(float time, UniqCar car)
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
        car.transform.Rotate(eulr.x, eulr.y + Random.Range(0f, 180f), eulr.z);


        //SetActive
        car.gameObject.SetActive(true);
        car.transform.DOScale(1, car.RespawnNoDamageTime - 0.1f);

        yield return new WaitForSeconds(car.RespawnNoDamageTime);
        car.OnRespawn();
    }
}
