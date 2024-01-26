using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class BonusSystem : MonoBehaviour
{
    [SerializeField] private BuildSaver buildSaver;
    [SerializeField] private byte lastBuildIDForActivateCapacity, lastBuildIDForActivateSpeed, lastBuildIDForActivateCar;


    private void Awake()
    {
        buildSaver.OnBuilt += CheckID;
        YandexGame.RewardVideoEvent += GetBonus;

        buttonForCarReward.onClick.AddListener(() => YandexGame.RewVideoShow(2));
        buttonForCarReward.gameObject.SetActive(false);
    }
    private void CheckID(byte obj)
    {
        if (obj == lastBuildIDForActivateCapacity)
        {
            ActivateCapacityBonus();
            return;
        }
        if (obj == lastBuildIDForActivateSpeed)
        {
            ActivateSpeedBonus();
            return;
        }
        if (obj == lastBuildIDForActivateCar)
        {
            ActivateCarBonus();
            return;
        }
    }
    private void GetBonus(int id)
    {
        if (id == 0)
        {
            GetSpeedBonus();
        }
        if (id == 1)
        {
            GetCapacityBonus();
        }
        if (id == 2)
        {
            GetCarBonus();
        }
    }


    #region CapacityBonus

    [Header("CapacityBonus"), Space(5f)]
    [SerializeField] private PlayerDetector capacityBonusInScene;
    [SerializeField] private float capacityCoolDown;
    [SerializeField] private float capacityBonusDuration;
    [SerializeField] private int capacityValue;
    [SerializeField] private TileSetter playerBackpack;

    private void ActivateCapacityBonus()
    {
        capacityBonusInScene.gameObject.SetActive(true);
        capacityBonusInScene.OnPlayerEnter += () => { capacityBonusInScene.gameObject.SetActive(false); YandexGame.RewVideoShow(1); };
    }
    IEnumerator CapacityBonusNextInstantiate()
    {
        yield return new WaitForSeconds(capacityCoolDown);
        capacityBonusInScene.gameObject.SetActive(true);
    }
    private void GetCapacityBonus()
    {
        capacityBonusInScene.gameObject.SetActive(false);
        StartCoroutine(CapacityBonusTimer());
    }
    IEnumerator CapacityBonusTimer()
    {
        var t = playerBackpack.MaxCapacity;
        playerBackpack.SetCapacity(capacityValue);
        print("Bonuse reward got it!");
        yield return new WaitForSeconds(capacityBonusDuration);
        playerBackpack.SetCapacity(t);

        StartCoroutine(CapacityBonusNextInstantiate());
    }
    #endregion

    #region SpeedBonus

    [Header("SpeedBonus"), Space(5f)]
    [SerializeField] private PlayerDetector speedBonusInScene;
    [SerializeField] private float speedCoolDown;
    [SerializeField] private float speedBonusDuration;
    [SerializeField] private float speedValue;
    [SerializeField] private PlayerController player;
    private void ActivateSpeedBonus()
    {
        speedBonusInScene.gameObject.SetActive(true);
        speedBonusInScene.OnPlayerEnter += () => { speedBonusInScene.gameObject.SetActive(false); YandexGame.RewVideoShow(0); };
    }
    IEnumerator SpeedBonusNextInstantiate()
    {
       yield return new WaitForSeconds(speedCoolDown);
        speedBonusInScene.gameObject.SetActive(true);
    }
    private void GetSpeedBonus()
    {
        speedBonusInScene.gameObject.SetActive(false);
        StartCoroutine(SpeedBonusTimer());
    }
    IEnumerator SpeedBonusTimer()
    {
        var t = player.MaxSpeed;
        player.SetSpeed(speedValue);
        print("Bonuse reward got it!");
        yield return new WaitForSeconds(speedBonusDuration);
        player.SetSpeed(t);

        StartCoroutine(SpeedBonusNextInstantiate());
    }
    #endregion

    #region CarBonus
    [Header("CarBonus"), Space(5f)]
    [SerializeField] private PlayerDetector carBonusTriggerInScene;
    [SerializeField] private Button buttonForCarReward;
    [SerializeField] private float carCoolDown;
    [SerializeField] private AutoRepair carSpawner;
    private void ActivateCarBonus()
    {
        carBonusTriggerInScene.gameObject.SetActive(true);
        carBonusTriggerInScene.OnPlayerEnter += () => buttonForCarReward.gameObject.SetActive(true);
        carBonusTriggerInScene.OnPlayerExit += () => buttonForCarReward.gameObject.SetActive(false);
    }
    IEnumerator CarBonusNextInstantiate()
    {
        buttonForCarReward.gameObject.SetActive(false);
        yield return new WaitForSeconds(carCoolDown);
        carBonusTriggerInScene.gameObject.SetActive(true);
    }
    private void GetCarBonus()
    {
        carBonusTriggerInScene.gameObject.SetActive(false);
        StartCoroutine(carSpawner.RepairByBonus());
        print("Bonuse reward got it!");
        StartCoroutine(CarBonusNextInstantiate());
    }

    #endregion

}

